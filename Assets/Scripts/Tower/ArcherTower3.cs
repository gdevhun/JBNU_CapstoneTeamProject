using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower3 : ArcherTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(70, 0.5f, 500);
    }

    // 아쳐타워3
    // 50% 확률로 연사공격
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

            // 연사
            if (Random.value < 0.5f)
            {
                yield return new WaitForSeconds(attackSpeed * 0.5f);
                Shot();
                SoundManager.Instance.PlaySFX(soundType);
            }
        }
    }
}
