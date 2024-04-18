using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Animator anim;

    public bool isskill;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    public void Boss_skill1(int power , GameObject hitObject)
    {
        
        anim.SetBool("isSkill1", true);
        Debug.Log("hhhi");
        if(hitObject.gameObject.tag == "Stone")
        {
            hitObject.GetComponent<Stone>().stoneHP -= power;
        }

        Invoke("Stop_Skill", 0.5f);
    }

    private void Stop_Skill()
    {
        anim.SetBool("isSkill1", false);
        isskill = false;
    }

}
