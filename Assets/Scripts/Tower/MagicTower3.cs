using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower3 : MagicTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(30, 1.3f, 400);
    }

    // 몬스터 처리
    // 매직타워3
    // 프리즈
    protected override void MonsterInteraction()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, 4f);

        // 스플래쉬 + 프리즈 처리
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                enemy.hp -= hit.gameObject == target.gameObject ? basicDamage : basicDamage / 2;
                enemy.moveSpeed *= 0.5f;
            }
        }
    }
}
