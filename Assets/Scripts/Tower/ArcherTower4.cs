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

    // 광역 공격
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            yield return new WaitForSeconds(attackSpeed); // 공격속도만큼 대기

            // 타겟 주변 적 모두 광역데미지
            Debug.Log("광역 : " + target.name + ", 데미지 : " + basicDamage);
            Debug.Log("광역 : " + target.name + ", 데미지 : " + basicDamage);
            Debug.Log("광역 : " + target.name + ", 데미지 : " + basicDamage);
            Debug.Log("광역 : " + target.name + ", 데미지 : " + basicDamage);
        }
    }
}
