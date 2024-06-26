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
    private int _thisStageSpawnInterval; //���罺������ ��������
    private int _thisStageEnableSpawnPt;  //���� �������� ����� ��������Ʈ
    private int _thisStageEnemyNum; //���ݺ� ���׹� ��ȯ ��
    private readonly string _stageName = "StageData";
    private int _thisStageNum = 1;
    public bool isCurWaveEnded = true;

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()  //��ü���� ���� �ڷ�ƾ �Լ�
    {
        while (_thisStageNum != 11)
        {
            StartCoroutine(CountDownAndSpawn()); //���̺����
            yield return new WaitUntil(() => isCurWaveEnded); // ���� ���������� ����� ������ ���
            _thisStageNum++;
        }
        Debug.Log("��");
    }
    private void InitStageData()  //�������� ������ �ʱ�ȭ �Լ�
    {
        StageManager.Instance.LoadStageData(_stageName + _thisStageNum);
        
        if (!StageManager.Instance.isLoadedData) return; //�����ͷε� ����
        
        thisStageInfo.text = StageManager.Instance.stageData.stageInfo; //HUD wave �ؽ�Ʈ ����
        
        _thisStageSpawnInterval = StageManager.Instance.stageData.stageSpawnInteval; //���͹� �ε�
        
        _thisStageEnemyNum = StageManager.Instance.stageData.stageSpawnNum; //���� Ƚ�� �ε�
        
        _thisStageEnableSpawnPt = StageManager.Instance.stageData.stageEnableSpawnPt; //��������Ʈ ���
        
    }

    private IEnumerator CountDownAndSpawn() //�긮ũ �� + ���̺���� �ڷ�ƾ
    {
        isCurWaveEnded = false;
        nextStageIntervalSec.gameObject.SetActive(true);
        int cnt = 10;
        while (cnt > 0)
        {   //10�ʿ� ���ļ� ī��Ʈ �ٿ��� ���ְ� �ؽ�Ʈ�� HUD�� ǥ������
            yield return new WaitForSeconds(1f);
            cnt--;
            nextStageIntervalSec.text = cnt.ToString();
        }
        nextStageIntervalSec.text = "10";
        yield return new WaitForSeconds(1f);
        nextStageIntervalSec.gameObject.SetActive(false);
        StartCoroutine(ActiveWaveStage()); //ī��Ʈ�� ������ ���� ����
        yield return null;
    }
    
    private IEnumerator ActiveWaveStage() //�������� �Լ�
    {   //���� StageTime = �������͹� * ���� �� + 2 + [�߰� �ð�]
        
        InitStageData(); //���� ������ ���� ������ �ε�
        stageTimerImage.SetActive(true);

        // 배경음
        if(StageManager.Instance.stageData.enemyType == PoolManager.EnemyType.MiddleBoss) SoundManager.Instance.PlayBGM(SoundType.보스BGM3);
        else if(StageManager.Instance.stageData.enemyType == PoolManager.EnemyType.LastBoss) SoundManager.Instance.PlayBGM(SoundType.보스BGM2);
        else if(StageManager.Instance.stageData.enemyType == PoolManager.EnemyType.Enemy6) SoundManager.Instance.PlayBGM(SoundType.일반BGM);
        
        for (int i = 0; i < _thisStageEnemyNum; i++)  //�� ���� Ƚ��
        {
            _thisStageEnableSpawnPt = StageManager.Instance.stageData.stageEnableSpawnPt;
            while (_thisStageEnableSpawnPt >= 0)
            {   //���� ��������Ʈ�� ��ȯ. stageEnableSpawnPt=1 ���� ���ʷ� 4����.
                SpawnEnemyInSp(_thisStageEnableSpawnPt);
                _thisStageEnableSpawnPt--;
            }
            //���͹� ��ŭ ��� �� �ٽ� ����
            yield return new WaitForSeconds(_thisStageSpawnInterval);
        }
        yield return new WaitForSeconds(2f);
        yield return null;
    }
    
    
    private void SpawnEnemyInSp(int sp)
    {
        Enemy enemy = PoolManager.Instance.GetEnemy(StageManager.Instance.stageData.enemyType).GetComponent<Enemy>();
        enemy.gameObject.transform.position = (spawnPoints[sp].position);
        enemy.movePointNum = sp + 1; // �̵� ��� ����
        enemy.FirstMoveTarget();

        if(enemy.enemyGold == 2000 || enemy.enemyGold == 10000)
        {
            GameManager.Instance.bossEnemy = enemy;
            GameManager.Instance.isBossStage = true;
            GameManager.Instance.bossHpPanel.SetActive(true);

            if(enemy.enemyGold == 10000) GameManager.Instance.lastBossImage.sprite = GameManager.Instance.lastBossSprite;
        }
    }

    /*private void SpawnBoss()
    {

    }*/
}
