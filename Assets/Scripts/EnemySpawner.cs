
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private readonly int _thisStageSpawnInterval; //현재스테이지 스폰간격
    private readonly int _thisStageSpawnNum;  //현재스테이지 스폰 횟수
    private readonly int _thisStageTime;  //현재 스테이지 시간
    void Start()
    {
        SpawnEnemy();
    }

    private void InitStageData()
    {
        
    }
    private void SpawnEnemy()
    {
        GameObject Enemy = PoolManager.Instance.GetEnemy(PoolManager.EnemyType.Enemy1);
        Enemy.gameObject.transform.SetPositionAndRotation(spawnPoints[1].position, Quaternion.identity);
    }

    /*private IEnumerator SpawnEnemy()
    {
        yield break;
    }*/
}
