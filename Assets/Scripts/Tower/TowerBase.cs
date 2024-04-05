using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    protected Transform target; // 타겟 몬스터
    protected bool isTarget = false; // 타겟이 설정되었는지 체크
    protected int basicDamage = 100; // 타워 기본 데미지
    protected float attackSpeed = 1.0f; // 타워 기본 공격속도
    protected Coroutine attackCoroutine; // 현재 실행 중인 공격 코루틴

    [SerializeField]
    protected PoolManager.TowerWeaponType towerWeaponType; // 타워 무기 타입

    // 타겟 설정
    // 일단 타겟이 나갔을때만 처리
    // 나중에 타겟 몬스터의 스크립트 Die 코루틴에서 죽었는지 체크하고 타겟 재탐색 해야함
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isTarget) TargetEnemy(other.transform);
    }

    // 타겟 나감
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == target) isTarget = false;
    }

    // 타겟 설정
    private void TargetEnemy(Transform enemy)
    {
        target = enemy;
        isTarget = true;
        if (attackCoroutine != null) StopCoroutine(attackCoroutine); // 이전 공격 코루틴 중지
        attackCoroutine = StartCoroutine(Attack()); // 새로운 공격 코루틴 시작
    }

    // 타겟 공격
    // 베이스는 단일공격
    protected virtual IEnumerator Attack()
    {
        while (isTarget)
        {
            // 공격속도만큼 대기
            yield return new WaitForSeconds(attackSpeed);

            // 타워 무기 가져오기
            GameObject towerWeapon = PoolManager.Instance.GetTowerWeapon(towerWeaponType);
            Rigidbody2D towerWeaponRigid = towerWeapon.GetComponent<Rigidbody2D>();

            // 위치 및 회전 초기화
            towerWeapon.transform.position = transform.position;
            towerWeapon.transform.rotation = towerWeapon.transform.rotation;

            // 타워 무기 발사
            Vector2 direction = (target.position - towerWeapon.transform.position).normalized;
            towerWeaponRigid.velocity = direction * 5f;

            // 무기 발사 각도
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            if(towerWeapon.CompareTag("ArcherWeapon123")) angle -= 45;
            towerWeapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 몬스터 체력 감소
            // target.GetComponent<Enemy>().health -= basicDamage;
            Debug.Log("단일 : " + target.name + ", 데미지 : " + basicDamage);

            // 1초 대기 후 타워 무기 비활성화
            yield return new WaitForSeconds(1f);
            towerWeapon.gameObject.SetActive(false);
        }
    }
}
