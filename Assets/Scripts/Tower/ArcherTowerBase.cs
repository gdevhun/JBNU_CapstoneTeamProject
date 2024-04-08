using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTowerBase : TowerBase
{
    // 타겟 공격
    // 아쳐타워는 단일공격
    protected override IEnumerator Attack()
    {
        while (isTarget)
        {
            // 공격속도만큼 대기
            yield return new WaitForSeconds(attackSpeed);

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
                if(towerWeapon.CompareTag("ArcherWeapon123")) angle -= 45;
                towerWeapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // 몬스터 체력 감소
                // 몬스터 Enemy 스크립트 접근해서 실제로 까야함
                // target.GetComponent<Enemy>().health -= basicDamage;
                Debug.Log("단일 : " + target.name + ", 데미지 : " + basicDamage);

                // 타워 애니메이션
                towerAnim[i].SetTrigger("atkTrig");
            }
        }
    }
}
