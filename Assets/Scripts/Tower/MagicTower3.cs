using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower3 : MagicTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(1, 1.4f, 200);
    }

    // 몬스터 처리
    // 매직타워3
    // 매직타워 3은 지속시간이 짧지만 감소량이 많음
    protected override void MonsterInteraction()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, 4f);

        // 스플래쉬 + 속도감소 처리
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                StartCoroutine(Slow(hit.GetComponent<Enemy>()));
            }
        }
    }

    // 속도 감소
    private IEnumerator Slow(Enemy enemy)
    {
        enemy.moveSpeed *= 0.5f;  // 슬로우

        yield return new WaitForSeconds(basicDamage);  // 지속시간

        enemy.moveSpeed = enemy.originSpeed;  // 해제
    }

}
