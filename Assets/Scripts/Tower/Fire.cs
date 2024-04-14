using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public int fireDamage; // 불 데미지

    // 스톤타워 1 불
    // 광역 지속딜 + 도트딜 부여
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.hp -= fireDamage;
            if(!enemy.isDot) enemy.isDot = true;
        }
    }
}
