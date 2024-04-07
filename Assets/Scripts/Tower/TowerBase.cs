using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    // 타워 스탯 관련
    [HideInInspector] public int basicDamage = 100; // 타워 기본 데미지
    protected float attackSpeed = 1.0f; // 타워 기본 공격속도

    // 타워 업글 관련
    [HideInInspector] public int towerLv = 1; // 타워 레벨
    [HideInInspector] public int towerUpgradeBasicPrice = 100; // 타워 업그레이드 기본비용

    // 타워 타겟 관련 
    protected Transform target; // 타겟 몬스터
    protected bool isTarget = false; // 타겟이 설정되었는지 체크

    // 타워 공격 관련
    protected Coroutine attackCoroutine; // 현재 실행 중인 공격 코루틴
    [Header ("타워 공격")] [Space (10f)] [SerializeField] [Tooltip ("타워 무기 발사위치")] protected List<GameObject> atkPos; // 타워 무기 발사위치
    [SerializeField] [Tooltip ("타워 무기 애니메이션")] protected List<Animator> towerAnim; // 타워 애니메이션
    protected WaitForSeconds halfSeconds = new WaitForSeconds(0.5f); // 대기시간

    // 타워 타입 관련
    [Header ("타워 타입")] [Space (10f)] [Tooltip ("타워 타입")] public TowerType towerType; // 타워 타입
    [Tooltip ("타워 무기 타입")] public PoolManager.TowerWeaponType towerWeaponType; // 타워 무기 타입

    // 타워 스탯 초기화
    protected void InitTower(int dmg = 100, float speed = 1.0f, int price = 100)
    {
        basicDamage = dmg;
        attackSpeed = speed;
        towerUpgradeBasicPrice = price;
    }

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
        if (other.transform == target) isTarget = false; // 타겟이 설정되지 않은 상태
    }

    // 타겟 설정
    private void TargetEnemy(Transform enemy)
    {
        target = enemy; // 타겟 설정
        isTarget = true; // 타겟이 설정된 상태
        if (attackCoroutine != null) StopCoroutine(attackCoroutine); // 이전 공격 코루틴 중지
        attackCoroutine = StartCoroutine(Attack()); // 새로운 공격 코루틴 시작
    }

    // 타겟 공격
    // 베이스는 단일공격
    protected abstract IEnumerator Attack();
}
