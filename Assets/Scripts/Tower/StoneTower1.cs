using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower1 : TowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(50, 5f, 200);
    }

    // 타겟 공격
    // 스톤타워 1은 지속딜 불 생성
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            // 공격속도만큼 대기
            yield return new WaitForSeconds(attackSpeed);

            
        }
    }
}
