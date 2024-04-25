using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawner : SingletonBehaviour<EnemySpawner>
{
    public TextMeshProUGUI nextStageIntervalSec;
    public TextMeshProUGUI thisStageInfo;
    public GameObject stageTimerImage; 
    [SerializeField] private Transform[] spawnPoints;
    private int _thisStageSpawnInterval; //현재스테이지 스폰간격
    private int _thisStageEnableSpawnPt;  //현재 스테이지 사용할 스폰포인트
    private int _thisStageEnemyNum; //간격별 에네미 소환 수
    private readonly string _stageName = "StageData";
    private int _thisStageNum = 1;
    public bool isCurWaveEnded = true;

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()  //전체적인 스폰 코루틴 함수
    {
        while (_thisStageNum != 30)
        {
            StartCoroutine(CountDownAndSpawn()); //웨이브시작
            yield return new WaitUntil(() => isCurWaveEnded); // 이전 스테이지가 종료될 때까지 대기
            _thisStageNum++;
        }
        Debug.Log("끝");
    }
    private void InitStageData()  //스테이지 데이터 초기화 함수
    {
        StageManager.Instance.LoadStageData(_stageName + _thisStageNum);
        
        if (!StageManager.Instance.isLoadedData) return; //데이터로드 오류
        
        thisStageInfo.text = StageManager.Instance.stageData.stageInfo; //HUD wave 텍스트 업뎃
        
        _thisStageSpawnInterval = StageManager.Instance.stageData.stageSpawnInteval; //인터벌 로드
        
        _thisStageEnemyNum = StageManager.Instance.stageData.stageSpawnNum; //스폰 횟수 로드
        
        _thisStageEnableSpawnPt = StageManager.Instance.stageData.stageEnableSpawnPt; //스폰포인트 사용
        
    }

    private IEnumerator CountDownAndSpawn() //브리크 텀 + 웨이브시작 코루틴
    {
        isCurWaveEnded = false;
        nextStageIntervalSec.gameObject.SetActive(true);
        int cnt = 10;
        while (cnt > 0)
        {   //10초에 걸쳐서 카운트 다운을 해주고 텍스트를 HUD에 표시해줌
            yield return new WaitForSeconds(1f);
            cnt--;
            nextStageIntervalSec.text = cnt.ToString();
        }
        nextStageIntervalSec.text = "10";
        yield return new WaitForSeconds(1f);
        nextStageIntervalSec.gameObject.SetActive(false);
        StartCoroutine(ActiveWaveStage()); //카운트가 끝나면 스폰 시작
        yield return null;
    }
    
    private IEnumerator ActiveWaveStage() //스폰시작 함수
    {   //실제 StageTime = 스폰인터벌 * 스폰 넘 + 2 + [추가 시간]
        
        InitStageData(); //현재 스폰에 대한 데이터 로드
        stageTimerImage.SetActive(true);
        
        for (int i = 0; i < _thisStageEnemyNum; i++)  //총 스폰 횟수
        {
            _thisStageEnableSpawnPt = StageManager.Instance.stageData.stageEnableSpawnPt;
            while (_thisStageEnableSpawnPt >= 0)
            {   //각각 스폰포인트에 소환. stageEnableSpawnPt=1 맨위 차례로 4까지.
                SpawnEnemyInSp(_thisStageEnableSpawnPt);
                _thisStageEnableSpawnPt--;
            }
            //인터벌 만큼 대기 후 다시 스폰
            yield return new WaitForSeconds(_thisStageSpawnInterval);
        }
        yield return new WaitForSeconds(2f);
        yield return null;
    }
    
    
    private void SpawnEnemyInSp(int sp)
    {
        GameObject enemy = PoolManager.Instance.GetEnemy(StageManager.Instance.stageData.enemyType);
        enemy.gameObject.transform.position = (spawnPoints[sp].position);
    }

    /*private void SpawnBoss()
    {

    }*/
}
