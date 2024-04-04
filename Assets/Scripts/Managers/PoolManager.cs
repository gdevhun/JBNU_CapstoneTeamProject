using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBehaviour<PoolManager>
{
	#region
	public enum EnemyType
	{
		Enemy1,     // 1번 스테이지 에네미~ 40번 스테이지 에네미
		Enemy2,    Enemy3,     Enemy4,     Enemy5,      
		Enemy6,    Enemy7,     Enemy8,     Enemy9,      
		Enemy10,   Enemy11,    Enemy12,    Enemy13,      
		Enemy14,   Enemy15,    Enemy16,    Enemy17,      
		Enemy18,   Enemy19,    Enemy20,    Enemy21,     
		Enemy22,   Enemy23,    Enemy24,    Enemy25,     
		Enemy26,   Enemy27,    Enemy28,    Enemy29,      
		Enemy30,   Enemy31,    Enemy32,    Enemy33,      
		Enemy34,   Enemy35,    Enemy36,    Enemy37,      
		Enemy38,   Enemy39,    Enemy40,    
	}

	[System.Serializable]
	public class EnemyPrefab
	{   //에네미 프리팹 클래스
		public EnemyType type;
		public GameObject prefab;
	}
	#endregion
	public List<EnemyPrefab> enemyPrefabs; //에네미 프리팹 리스트
	private Dictionary<EnemyType, List<GameObject>> enemiesPool; //각 에네미 담을 딕셔너리


	/*인스펙터에서 할당 -> 초기화
	public GameObject[] EnemyPrefabs; //에네미프리팹
    public GameObject[] BossPrefabs;  //보스프리팹
    public GameObject[] TowerPrefabs; //타워프리팹
	public GameObject[] EffectPrefabs;  //이펙트 프리팹


	private List<GameObject>[] enemyPool; //에네미담을 리스트
	private List<GameObject>[] bossPool; //에네미담을 리스트
	private List<GameObject>[] towerPool; //에네미담을 리스트
	private List<GameObject>[] effectPool; //이펙트담을 리스트*/

	protected override void Awake()
    {
        base.Awake();
		
        //풀할 배열들 선언 길이 초기화 => 에네미, 보스, 타워, 이펙트

		// 딕셔너리 초기화
		enemiesPool = new Dictionary<EnemyType, List<GameObject>>();

		// 각 에네미 프리팹에 대한 풀 초기화
		foreach (EnemyPrefab enemyPrefab in enemyPrefabs)
		{
			List<GameObject> pool = new List<GameObject>();
			for (int i = 0; i < 30; i++) // 각 에네미 타입별로 30개씩 생성하여 풀에 추가
			{
				GameObject enemyObject = Instantiate(enemyPrefab.prefab);
				enemyObject.SetActive(false); // 활성화하지 않은 상태로 생성
				pool.Add(enemyObject);
			}
			enemiesPool.Add(enemyPrefab.type, pool); // 에네미 타입과 해당 타입의 풀을 딕셔너리에 추가

		}

	
		/*bossPool = new List<GameObject>[EnemyPrefabs.Length];  //4개
		towerPool = new List<GameObject>[EnemyPrefabs.Length];  //8개 레벨별로 3개 =>24개
		effectPool=new List<GameObject>[EffectPrefabs.Length];  //8개*/
	}
    void Start()
    {
		
	}
	public GameObject GetEnemy(EnemyType type)
	{
		GameObject selectedEnemy = null;
		List<GameObject> enemyList;
		if (enemiesPool.TryGetValue(type, out enemyList))
		{
			foreach (GameObject enemy in enemyList)
			{
				if (!enemy.activeSelf)
				{
					selectedEnemy = enemy;
					selectedEnemy.SetActive(true);
					break;
				}
			}
		}
		else
		{
			enemyList = new List<GameObject>();
			enemiesPool.Add(type, enemyList);
		}

		if (selectedEnemy == null)
		{
			// 풀에서 사용 가능한 적이 없으면 새로 생성하여 추가
			selectedEnemy = Instantiate(GetEnemyPrefab(type), transform);
			selectedEnemy.transform.parent = this.transform;
			enemyList.Add(selectedEnemy);
		}

		return selectedEnemy;
	}

	private GameObject GetEnemyPrefab(EnemyType type)
	{
		foreach (EnemyPrefab enemyPrefab in enemyPrefabs)
		{
			if (enemyPrefab.type == type)
			{
				return enemyPrefab.prefab;
			}
		}
		return null;
	}
}
