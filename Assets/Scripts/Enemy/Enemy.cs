using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{


    public int hp = 100;
    public int power;
    public int Boss_power;
    public int dotDamage; // 몬스터한테 입힐 도트 데미지

    public float originSpeed = 0.1f; // 몬스터 원래 속도
    public float moveSpeed;

    public GameObject target; //목표위치


    private Rigidbody2D rigid;
    private Animator anim;
    private Coroutine co;

    // attack�ڷ�ƾ �ѹ��� �����Ҽ��ְ� �ϵ��� �ϴ� ����
    public bool isattack = false;
    public bool isdead = false;
    public bool isDot = false; // 도트딜을 입는 상태인지 체크

    //�̵�����Ʈ�� ��ֹ�(�ؼ��� ����) ����
    public GameObject hit_target;


    public int  boss_attack_num;

    public int movepoint_num;  // 1~4가지 동선 결정.
    public MovePoints movepoints;


    private int next_position = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //originSpeed = navmesh.speed; // 몬스터 원래 속도 백업
        dotDamage = hp / 100; // 몬스터한테 입힐 도트 데미지 
        movepoints = GameObject.Find("MovePoints").gameObject.GetComponent<MovePoints>();
    }
    void Start()
    {
        StartCoroutine(DotDamaged()); // 도트 데미지
        first_movetarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isattack  &&  !isdead)
        {
            Enemy_Move(movepoint_num);
        }

        if(hp <= 0)
        {
            isdead = true;
            StartCoroutine("Enemy_dead");
        }
    }

    void FixedUpdate()
    {
        if (!isdead)
        {
            Scan();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        change_moveptarget(collision);

    }


    void Enemy_Move(int movenum)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed);
    }

    //첫번째 이동 위치 입력.
    void first_movetarget()
    {
        switch (movepoint_num)
        {
            case 1:
                target = GameObject.Find("MovePoints").transform.Find("move_point6").gameObject;
                break;
            case 2:
                target = GameObject.Find("MovePoints").transform.Find("move_point7").gameObject;
                break;
            case 3:
                target = GameObject.Find("MovePoints").transform.Find("move_point3").gameObject;
                break;
            case 4:
                target = GameObject.Find("MovePoints").transform.Find("move_point1").gameObject;
                break;
        }
    }

    void change_moveptarget(Collider2D collision)
    {
        try
        {
            if (collision.gameObject.tag == "MovePoints")
            {
                if (movepoint_num == 1 && collision.gameObject == movepoints.movepoints_1[next_position])
                {
                    target = movepoints.movepoints_1[next_position + 1];
                    next_position++;
                }
                else if (movepoint_num == 2 && collision.gameObject == movepoints.movepoints_2[next_position])
                {
                    target = movepoints.movepoints_2[next_position + 1];
                    next_position++;
                }
                else if (movepoint_num == 3 && collision.gameObject == movepoints.movepoints_3[next_position])
                {
                    target = movepoints.movepoints_3[next_position + 1];
                    next_position++;
                }
                else if (movepoint_num == 4 && collision.gameObject == movepoints.movepoints_4[next_position])
                {
                    target = movepoints.movepoints_4[next_position + 1];
                    next_position++;
                }
            }
        }
        catch
        {
            Debug.Log("배열index처리");
        }
    }



    void Scan() //��ֹ� �� Movepoint ����
    {
        Vector2 v2 = rigid.velocity.normalized;
        Debug.DrawRay(rigid.position, Vector2.right * 1, new Color(0, 1, 0));
        RaycastHit2D rayHit_obstacle = Physics2D.Raycast(rigid.position, Vector2.right, 1f, LayerMask.GetMask("Hit"));

        try
        {
            if (rayHit_obstacle.collider != null)
            {

                hit_target = rayHit_obstacle.transform.gameObject;
                if (!isattack)
                {
                    co = StartCoroutine("Enemy_attack", hit_target);
                }
            }
            else if (rayHit_obstacle.collider == null)
            {
                hit_target = null;
                Remove_Obstacle(hit_target);
            }
        }
        catch
        {
            Debug.Log("null�� ������ ��ȯ");
        }


    }

    IEnumerator Enemy_attack(GameObject hit_object)
    {
       anim.SetBool("isAttack", true);
       isattack = true;
       rigid.velocity = Vector2.zero;



        if (hit_object.gameObject.tag == "Stone")
        {
            while (!isdead) // ������ ���߿� �״� ��쵵 ������ �ֱ⿡ ����Ȯ��.
            {
                boss_attack_num++;
                if(boss_attack_num == 5 && gameObject.layer ==  10) //layer 10 Boss
                {
                    if (gameObject.GetComponent<Boss>().isskill == false)
                    {
                        gameObject.GetComponent<Boss>().isskill = true;
                        gameObject.GetComponent<Boss>().Boss_skill1(Boss_power, hit_object);
                    }
                    boss_attack_num = 0;
                    continue;
                }
                hit_object.GetComponent<Stone>().stoneHP -= power;
                if (hit_object.GetComponent<Stone>().stoneHP <= 0)
                {
                    Remove_Obstacle(hit_object);
                    break;
                }
                yield return new WaitForSeconds(0.5f);

            }
        }


        //gameManager함수만 정의되면 boss_skill함수 실행가능. 테스트결과 gameManager함수없이 실행 됨.
        else if (hit_object.gameObject.tag == "Nexus")
        {
            while (!GameManager.Instance._isGameOver && !isdead)  //때리는 도중에 죽을수 있기 때문에 isdead변수 추가.
            {
                boss_attack_num++;
                if (boss_attack_num == 5 && gameObject.layer == 10) //layer 10 Boss
                {
                    if (gameObject.GetComponent<Boss>().isskill == false)
                    {
                        gameObject.GetComponent<Boss>().isskill = true;
                        gameObject.GetComponent<Boss>().Boss_skill1(0, hit_object); // 0으로 해서 대미지 x 애니메니션 실행만
                        GameManager.Instance.NexusDamaged(Boss_power);
                    }
                    boss_attack_num = 0;
                    continue;
                }
                GameManager.Instance.NexusDamaged(power);
                yield return new WaitForSeconds(0.5f);
            }

            /*while (GameObject.Find("GameManager").gameObject.GetComponent<GameManager>()._nexusHp > 0 && !isdead)
            {
                GameObject.Find("GameManager").gameObject.GetComponent<GameManager>()._nexusHp -= power;
                if (GameObject.Find("GameManager").gameObject.GetComponent<GameManager>()._nexusHp <= 0)
                {
                    //Remove_Obstacle(hit_object);
                    break;
                }
                yield return new WaitForSeconds(0.5f);

            }*/
        }

    }

    //��ֹ� Ǯ�� false���·� ��ȯ�ϰ� enemy �̵� �ٽ� ����.
    // 1.���簡 ��ֹ� hp 0���� ������, 2. �� ���簡 ���� ��ֹ��� ���ְ� ���� ������ ����鵵 �����̰� �ϱ� ���� �� �ʱ�ȭ�� ���� Scan����.
    void Remove_Obstacle(GameObject hit_object)
    {
        anim.SetBool("isAttack", false);
        isattack = false;
        if(hit_object != null)
        {
            hit_object.gameObject.SetActive(false);
        }
    }

    //체력0되면 Enemy는 알아서 fasle 되면서 풀로 들어감.
    IEnumerator Enemy_dead()
    {
        if (isdead)
        {
            //�ִϸ��̼� ��ħ����
            anim.SetBool("isAttack", false);
            anim.SetBool("isSkill1", false);
            anim.SetBool("isDead", true);
            rigid.velocity = Vector2.zero;
            
            yield return new WaitForSeconds(1f);

            //�׾����� setfalse�ϰ���� ���Ǻ��� �ʱ�ȭ.
            gameObject.SetActive(false);
            isdead = false;
            hp = 100;
            anim.SetBool("isDead", false);
            isDot = false; // 죽으면 도트딜 없는 상태로
            boss_attack_num = 0;
        }

    }


    // 도트데미지 : 체력 1%씩 감소
    IEnumerator DotDamaged()
    {
        while (true)
        {
            if(isDot)
            {
                hp -= dotDamage;
                yield return new WaitForSeconds(0.1f);

                continue;
            }

            yield return null;
        }
    }
}
