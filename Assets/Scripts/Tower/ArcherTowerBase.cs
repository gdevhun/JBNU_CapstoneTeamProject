using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerBase : TowerBase
{
    // 타겟 공격
    // 아쳐타워1,2,3 단일공격
    // 아쳐타워1,2 연사공격 X
    // 아쳐타워3 연사공격 O
    // 아쳐타워4 연사공격 X 광역공격 O
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            // 공격속도만큼 대기
            yield return new WaitForSeconds(attackSpeed);

            // 발사
            Shot();

            // 사운드
            SoundManager.Instance.PlaySFX(soundType);
        }
    }

    // 몬스터 처리
    protected override void MonsterInteraction()
    {
        // 단일 처리
        target.GetComponent<Enemy>().hp -= basicDamage;
    }
}
