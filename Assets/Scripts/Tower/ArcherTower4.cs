using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower4 : TowerBase
{
    // 스탯 조정
    private void Awake()
    {
        basicDamage = 500;
        attackSpeed = 2.5f;
    }

    // 타겟 공격
    // 아쳐타워 4는 광역 공격
    protected override IEnumerator Attack()
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
            towerWeapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 광역 몬스터 체력 감소
            Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, 1f);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    //hit.GetComponent<Enemy>().health -= basicDamage;
                    Debug.Log("광역 : " + hit.gameObject.name + ", 데미지 : " + basicDamage);
                }
            }

            // 디버깅용
            Debug.DrawLine(target.position + new Vector3(-1f, 0f, 0f), target.position + new Vector3(1f, 0f, 0f), Color.red, 2f);
            Debug.DrawLine(target.position + new Vector3(0f, -1f, 0f), target.position + new Vector3(0f, 1f, 0f), Color.red, 2f);

            // 1초 대기 후 타워 무기 비활성화
            yield return new WaitForSeconds(1f);
            towerWeapon.gameObject.SetActive(false);
        }
    }
}
