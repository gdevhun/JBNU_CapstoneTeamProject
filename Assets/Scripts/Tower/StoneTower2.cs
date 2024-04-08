using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower2 : TowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(200, 7f, 200);
    }

    // 타겟 공격
    // 스톤타워 2는 길을 막는 돌 생성
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            // 공격속도만큼 대기
            yield return new WaitForSeconds(attackSpeed);

            
        }
    }
}
