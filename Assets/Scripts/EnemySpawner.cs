
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private readonly int _thisStageSpawnInterval; //���罺������ ��������
    private readonly int _thisStageSpawnNum;  //���罺������ ���� Ƚ��
    private readonly int _thisStageTime;  //���� �������� �ð�
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
