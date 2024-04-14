using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower2 : TowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(50, 1.5f, 400);
    }

    // 타겟 공격
    // 스톤타워 2는 길을 막는 돌 생성
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            // 공격속도만큼 대기
            yield return new WaitForSeconds(attackSpeed);

            //타워 애니메이션
            towerAnim[0].SetTrigger("atkTrig");

            // 잠시 대기 후
            yield return halfSeconds;

            // 타워 무기 가져오기
            GameObject towerWeapon = PoolManager.Instance.GetTowerWeapon(towerWeaponType);
            towerWeapon.GetComponent<Stone>().stoneHP = basicDamage;

            // 위치 및 회전 초기화
            towerWeapon.transform.position = target.transform.position;
            towerWeapon.transform.rotation = towerWeapon.transform.rotation;

            // 사운드
            SoundManager.Instance.PlaySFX(SoundType.스톤타워돌);
        }
    }
}
