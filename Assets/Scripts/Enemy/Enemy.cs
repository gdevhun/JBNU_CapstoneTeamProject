using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    public Transform target;

    public int hp = 100;
    public int power;
    public int Boss_power;
    public int dotDamage; // 몬스터한테 입힐 도트 데미지
    public float originSpeed; // 몬스터 원래 속도
   
    public int spawnpoint_num; //1. ������, 2. �߰�, 3. �Ʒ� , 4. �� �Ʒ�

    //�� 3���� ��Ʈ 1) 1 -> 3->  5 , 2) 2 -> 5, 3)4 -> 5
    public int movepoint_num;  // 1. �߰�  2. ����������Ʈ ��  3. ����������Ʈ ���  4. ����������Ʈ �Ʒ�  5. �ؼ���  
    private GameObject[] movepoints;// �ν�����â�� �ڵ����� ������.
    private string[] movepoints_name = { "move_point1", "move_point2", "move_point3", "move_point4", "move_point5", "move_point6", "move_point7", "move_point8"};

    public NavMeshAgent navmesh;
    private Rigidbody2D rigid;
    private Animator anim;
    private Coroutine co;

    // attack�ڷ�ƾ �ѹ��� �����Ҽ��ְ� �ϵ��� �ϴ� ����
    public bool isattack = false;
    public bool isdead = false;
    public bool isDot = false; // 도트딜을 입는 상태인지 체크

    //�̵�����Ʈ�� ��ֹ�(�ؼ��� ����) ����
    private GameObject position;
    public GameObject hit_target;


    public int  boss_attack_num;
    void Awake()
    {
        movepoints = new GameObject[8];
        navmesh = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originSpeed = navmesh.speed; // 몬스터 원래 속도 백업
        dotDamage = hp / 100; // 몬스터한테 입힐 도트 데미지 
    }
    void Start()
    {
        GetMovePoints();
        StartCoroutine(DotDamaged()); // 도트 데미지
    }

    // Update is called once per frame
    void Update()
    {
        if(!isattack  &&  !isdead)
        {
            Enemy_Move();
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



    void Enemy_Move()
    {
        navmesh.SetDestination(movepoints[movepoint_num - 1].transform.position);
    }

    void GetMovePoints()
    {
        GameObject point;
        for (int i = 0; i < 8; i++)
        {          
            point = GameObject.Find("MovePoints").transform.Find(movepoints_name[i]).gameObject;
            movepoints[i] = point;
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
       navmesh.enabled = false;
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
                        gameObject.GetComponent<Boss>().Boss_skill1(Boss_power * 2, hit_object);
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


        //if tag �ؼ���
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
                        gameObject.GetComponent<Boss>().Boss_skill1(0, hit_object);
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
        navmesh.enabled = true;
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
            navmesh.enabled = false;
            rigid.velocity = Vector2.zero;
            
            yield return new WaitForSeconds(1f);

            //�׾����� setfalse�ϰ���� ���Ǻ��� �ʱ�ȭ.
            gameObject.SetActive(false);
            navmesh.enabled = true;
            isdead = false;
            hp = 100;
            anim.SetBool("isDead", false);
            navmesh.speed = originSpeed; // 죽으면 다시 원래 속도로
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
