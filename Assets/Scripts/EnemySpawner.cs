using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] SpawnPoints;
    void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        GameObject Enemy = PoolManager.Instance.GetEnemy(PoolManager.EnemyType.Enemy1);
        Enemy.gameObject.transform.SetPositionAndRotation(SpawnPoints[1].position, Quaternion.identity);
    }

    /*private IEnumerator SpawnEnemy()
    {
        yield break;
    }*/
}
