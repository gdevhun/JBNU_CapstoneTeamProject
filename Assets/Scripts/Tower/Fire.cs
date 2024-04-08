using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [HideInInspector] public int fireDamage; // 불 데미지

    // 스톤타워 1 불
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) Debug.Log("지속 : " + other.gameObject.name + ", 데미지 : " + fireDamage);
    }
}
