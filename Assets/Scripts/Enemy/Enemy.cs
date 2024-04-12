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
   
    public int spawnpoint_num; //1. 가장위, 2. 중간, 3. 아래 , 4. 맨 아래

    //총 3개의 루트 1) 1 -> 3->  5 , 2) 2 -> 5, 3)4 -> 5
    public int movepoint_num;  // 1. 중간  2. 마지막포인트 위  3. 마지막포인트 가운데  4. 마지막포인트 아래  5. 넥서스  
    private GameObject[] movepoints;// 인스펙터창에 자동으로 설정됨.
    private string[] movepoints_name = { "move_point1", "move_point2", "move_point3", "move_point4", "move_point5" };

    private NavMeshAgent navmesh;
    private Rigidbody2D rigid;
    private Animator anim;
    private Coroutine co;

    // attack코루틴 한번만 실행할수있게 하도록 하는 변수
    public bool isattack = false;
    public bool isdead = false;

    //이동포인트와 장애물(넥서스 포함) 감지
    private GameObject position;
    public GameObject hit_target;



    void Awake()
    {
        movepoints = new GameObject[5];
        navmesh = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        GetMovePoints();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isattack  &&  !isdead)
        {
            Enemy_Move();
        }

        if(hp == 0)
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
        for (int i = 0; i < 5; i++)
        {          
            point = GameObject.Find("MovePoints").transform.Find(movepoints_name[i]).gameObject;
            movepoints[i] = point;
        }

    }

    void Change_MovePosition()
    {
        if (movepoint_num == 1)
        {
            movepoint_num = 3;
        }
        else if (movepoint_num == 2 || movepoint_num == 3 || movepoint_num == 4)
        {
            movepoint_num = 5;
        }   
    }

    void Scan() //장애물 및 Movepoint 감지
    {
        Vector2 v2 = rigid.velocity.normalized;
        Debug.DrawRay(rigid.position, Vector2.right * 1, new Color(0, 1, 0));
        RaycastHit2D rayHit_obstacle = Physics2D.Raycast(rigid.position, Vector2.right, 1f, LayerMask.GetMask("Hit"));
        RaycastHit2D rayHit_movepoint = Physics2D.Raycast(rigid.position, Vector2.right, 1f, LayerMask.GetMask("MovePoint"));

        try
        {
            if (rayHit_movepoint.collider != null)
            {
                position = rayHit_movepoint.transform.gameObject;
                Change_MovePosition();
            }


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
            Debug.Log("null값 오류문 반환");
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
            while (hit_object.GetComponent<Stone>().stoneHP > 0 && !isdead) // 때리는 도중에 죽는 경우도 있을수 있기에 조건확인.
            {
                hit_object.GetComponent<Stone>().stoneHP -= power;
                if (hit_object.GetComponent<Stone>().stoneHP <= 0)
                {
                    Remove_Obstacle(hit_object);
                    break;
                }
                yield return new WaitForSeconds(0.5f);

            }
        }


        //if tag 넥서스
        //else if (hit_object.gameObject.tag == "Nexus")
        //{
            //while (hit_object.GetComponent<Stone>().stoneHP > 0)
            //{
                // 넥서스체력갖고오기
                //if (hit_object.GetComponent<Stone>().stoneHP <= 0)
                //{
                   // Remove_Obstacle(hit_object);
                    //Debug.Log(gameObject.name + "hi");
                   // break;
               // }
               // yield return new WaitForSeconds(0.5f);

           // }
        //}

    }

    //장애물 풀에 false상태로 반환하고 enemy 이동 다시 수행.
    // 1.병사가 장애물 hp 0만들 시점에, 2. 한 병사가 먼저 장애물을 없애고 나면 나머지 병사들도 움직이게 하기 위한 및 초기화를 위해 Scan에도.
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

    IEnumerator Enemy_dead()
    {
        if (isdead)
        {
            //애니메이션 겹침방지
            anim.SetBool("isAttack", false);
            anim.SetBool("isDead", true);
            navmesh.enabled = false;
            rigid.velocity = Vector2.zero;
            
            yield return new WaitForSeconds(1f);

            //죽었으니 setfalse하고모든 조건변수 초기화.
            gameObject.SetActive(false);
            navmesh.enabled = true;
            isdead = false;
            hp = 100;
            anim.SetBool("isDead", false);
        }

    }

}
