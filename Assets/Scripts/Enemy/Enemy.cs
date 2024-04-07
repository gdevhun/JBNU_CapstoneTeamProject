using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;

    public float hp;
    public float power;
    public bool is_Dead;
    
    NavMeshAgent navmesh;


    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Enemy_Move();
    }

    void Enemy_Move()
    {    
        //�ӵ��� Navmeshagentâ���� ��������.
        navmesh.SetDestination(target.position);
        
    }
}
