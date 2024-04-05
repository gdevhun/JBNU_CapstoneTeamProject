using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerBase : TowerBase
{
    protected int splashDamage = 50; // 스플래쉬 데미지

    // 스플래쉬 공격
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            yield return new WaitForSeconds(attackSpeed); // 공격속도만큼 대기

            // 타겟 적 주변 두명에게 스플래쉬 데미지
            Debug.Log("단일 : " + target.name + ", 데미지 : " + basicDamage);
            Debug.Log("스플래쉬 : " + target.name + ", 데미지 : " + splashDamage);
            Debug.Log("스플래쉬 : " + target.name + ", 데미지 : " + splashDamage);
        }
    }
}
