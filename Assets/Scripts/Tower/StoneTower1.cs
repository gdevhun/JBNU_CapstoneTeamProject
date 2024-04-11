using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower1 : TowerBase
{
    // 풀링한 불
    [HideInInspector] public List<GameObject> FirePrefabs = new List<GameObject>();

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

            // 타워 애니메이션
            towerAnim[0].SetTrigger("atkTrig");

            // 잠시 대기 후
            yield return halfSeconds;

            // 타워 무기 가져오기
            GameObject towerWeapon = PoolManager.Instance.GetTowerWeapon(towerWeaponType);
            FirePrefabs.Add(towerWeapon);

            // 데미지 설정
            towerWeapon.GetComponent<Fire>().fireDamage = basicDamage;

            // 위치 및 회전 초기화
            towerWeapon.transform.position = target.transform.position;
            towerWeapon.transform.rotation = towerWeapon.transform.rotation;

            for(int i = 0; i < 4; i++)
            {
                // 사운드
                SoundManager.Instance.PlaySFX(SoundType.스톤타워불);

                // 잠시 대기 후
                yield return halfSeconds;
            }

            // 불 비활성화
            towerWeapon.gameObject.SetActive(false);
        }
    }
}
