using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDeActive : MonoBehaviour
{
    public AudioSource audioSource;
    
    void Update()
    {
        // 사운드 비활성화
        if(!audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
