using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public TextMeshProUGUI nextStageIntervalSec;
    public TextMeshProUGUI thisStageInfo;
    public Image waveTimeImage;
    [SerializeField] private Transform[] spawnPoints;
    private int _thisStageSpawnInterval; //���罺������ ��������
    private int _thisStageSpawnNum;  //���罺������ ���� Ƚ��
    private int _thisStageTime;  //���� �������� �ð�
    private int _thisStageEnemyNum; //���ݺ� ���׹� ��ȯ ��
    private readonly string _stageName = "StageData";
    private int _thisStageNum = 1;
    private bool _isCurWaveEnded = true;

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()  //��ü���� ���� �ڷ�ƾ �Լ�
    {
        while (_thisStageNum != 30)
        {
            StartCoroutine(CountDownAndSpawn()); //���̺����
            yield return new WaitUntil(() => _isCurWaveEnded); // ���� ���������� ����� ������ ���
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
        _thisStageSpawnNum = StageManager.Instance.stageData.stageSpawnNum; //���� Ƚ�� �ε�
        _thisStageTime = StageManager.Instance.stageData.stageTime; //���������ð� �ε�
        _thisStageEnemyNum = StageManager.Instance.stageData.stageSpawnNum; //���׹� ��ȯ�� �ε�
        waveTimeImage.fillAmount = 0f;
        
    }

    private IEnumerator CountDownAndSpawn() //�긮ũ �� + ���̺���� �ڷ�ƾ
    {
        _isCurWaveEnded = false;
        nextStageIntervalSec.gameObject.SetActive(true);
        int cnt = 10;
        while (cnt > 0)
        {   //10�ʿ� ���ļ� ī��Ʈ �ٿ��� ���ְ� �ؽ�Ʈ�� HUD�� ǥ������
            yield return new WaitForSeconds(1f);
            cnt--;
            nextStageIntervalSec.text = cnt.ToString();
        }
        yield return new WaitForSeconds(1f);
        nextStageIntervalSec.gameObject.SetActive(false);
        StartCoroutine(ActiveWaveStage()); //ī��Ʈ�� ������ ���� ����
        yield return null;
    }
    
    private IEnumerator ActiveWaveStage() //�������� �Լ�
    {   //���� StageTime = �������͹� * ���� �� + 2 + [�߰� �ð�]
        waveTimeImage.fillAmount += (Time.deltaTime/_thisStageTime);
        InitStageData(); //���� ������ ���� ������ �ε�
        
        for (int i = 0; i < _thisStageSpawnNum; i++)  //�� ���� Ƚ��
        {
            while (_thisStageEnemyNum != 0)
            {   //�ϳ��� ������ ������ �ֳ׹� ��
                SpawnEnemyInRanSp();
                _thisStageEnemyNum--;
            }
            //���͹� ��ŭ ��� �� �ٽ� ����
            yield return new WaitForSeconds(_thisStageSpawnInterval);
        }
        yield return new WaitForSeconds(2f);
        if(waveTimeImage.fillAmount >= 0.99) _isCurWaveEnded = true; //���罺������ ����
        yield return null;
    }

    private void SpawnEnemyInRanSp()
    {
        GameObject enemy = PoolManager.Instance.GetEnemy(StageManager.Instance.stageData.enemyType);
        int randomSp = Random.Range(0, 4); //sp 0,1,2,3, ��������
        
        enemy.gameObject.transform.SetPositionAndRotation(spawnPoints[randomSp].position, Quaternion.identity);
        
    }

    /*private void SpawnBoss()
    {

    }*/
}
