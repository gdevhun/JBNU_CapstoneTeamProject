using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower2 : MagicTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(20, 1.4f, 300);
    }

    // 몬스터 처리
    // 매직타워2
    // 속도감소
    protected override void MonsterInteraction()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, 4f);

        // 타겟팅된 몬스터 처리
        Enemy tartgetEnemy = target.GetComponent<Enemy>();
        tartgetEnemy.hp -= basicDamage;
        tartgetEnemy.navmesh.speed /= 2;

        // 타겟 주변 몬스터 처리
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.gameObject != target.gameObject)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                enemy.hp -= basicDamage / 2;
                enemy.navmesh.speed /= 2;
            }
        }
    }
}
