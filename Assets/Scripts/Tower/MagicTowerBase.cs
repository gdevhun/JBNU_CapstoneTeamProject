using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTowerBase : TowerBase
{
    // 매직 타워 무기 충돌 이펙트
    [Tooltip ("매직타워 무기 이펙트 타입")] public PoolManager.TowerWeaponEffectType towerWeaponEffectType;

    // 풀링한 무기 충돌 이펙트 저장
    [HideInInspector] public List<GameObject> towerWeaponEffectPrefabs = new List<GameObject>();

    // 공격 사운드 타입
    [SerializeField] [Tooltip ("매직타워 무기 사운드 타입")] protected SoundType soundType;

    // 타겟 공격
    // 매직타워는 스플래쉬 공격
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            // 공격속도만큼 대기
            yield return new WaitForSeconds(attackSpeed);

            // 사운드
            SoundManager.Instance.PlaySFX(soundType, 0.5f);

            // 매직타워시간만 잠시 대기 후
            yield return soundType == SoundType.매직타워시간 || soundType == SoundType.매직타워불 ? new WaitForSeconds(0.75f) : null;

            // 타워 애니메이션
            for(int i = 0; i < towerAnim.Count; i++)
            {
                towerAnim[i].SetTrigger("atkTrig");
            }

            // 타워 무기 충돌 이펙트
            GameObject towerWeaponEffect = PoolManager.Instance.GetTowerWeaponEffect(towerWeaponEffectType);
            towerWeaponEffect.transform.position = target.transform.position + transform.up * 14f;
            towerWeaponEffectPrefabs.Add(towerWeaponEffect);

            // 타워 무기 발사위치 개수만큼
            for(int i = 0; i < atkPos.Count; i++)
            {
                // 타워 무기 가져오기
                GameObject towerWeapon = PoolManager.Instance.GetTowerWeapon(towerWeaponType);
                Rigidbody2D towerWeaponRigid = towerWeapon.GetComponent<Rigidbody2D>();

                // 위치 및 회전 초기화
                towerWeapon.transform.position = atkPos[i].transform.position;
                towerWeapon.transform.rotation = towerWeapon.transform.rotation;

                // 타워 무기 발사
                Vector2 direction = (target.position - towerWeapon.transform.position).normalized;
                towerWeaponRigid.velocity = direction * 15f;

                // 무기 발사 각도
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                towerWeapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // 타겟팅된 몬스터 체력 감소
                // 몬스터 Enemy 스크립트 접근해서 실제로 까야함
                // target.GetComponent<Enemy>().health -= basicDamage;
                Debug.Log("단일 : " + target.name + ", 데미지 : " + basicDamage);

                // 타겟팅된 몬스터 주변 두명 몬스터 스플래쉬 데미지
                Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, 1f);

                int splashCnt = 0; // 스플래쉬 횟수

                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Enemy") && splashCnt < 2 && hit.gameObject != target.gameObject)
                    {
                        //몬스터 Enemy 스크립트 접근해서 실제로 까야함
                        //hit.GetComponent<Enemy>().health -= splashDamage;
                        Debug.Log("스플래쉬 : " + hit.gameObject.name + ", 데미지 : " + basicDamage / 2);
                        splashCnt++;
                    }
                }

                // 디버깅용
                Debug.DrawLine(target.position + new Vector3(-1f, 0f, 0f), target.position + new Vector3(1f, 0f, 0f), Color.red, 2f);
                Debug.DrawLine(target.position + new Vector3(0f, -1f, 0f), target.position + new Vector3(0f, 1f, 0f), Color.red, 2f);
            }

            // 잠시 대기 후
            yield return halfSeconds;
            
            // 타워 무기 충돌 이펙트 비활성화
            towerWeaponEffect.gameObject.SetActive(false);
        }
    }
}
