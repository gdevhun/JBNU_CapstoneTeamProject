using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public int fireDamage; // 불 데미지

    // 스톤타워 1 불
    // 도트딜 부여
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(!enemy.isDot) StartCoroutine(Dot(enemy));
        }
    }

    private IEnumerator Dot(Enemy enemy)
    {
        enemy.dotDamage = enemy.maxHp / 100 * fireDamage; // 도트딜

        enemy.isDot = true; // 적용

        yield return new WaitForSeconds(5f); // 지속시간

        enemy.isDot = false; // 해제
    }
}
