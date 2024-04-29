using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 1)]
public class StageData : ScriptableObject
{
    public string description = "���� StageTime = �������͹� * ���� �� + 2 + [�߰� �ð�]";
    public string stageInfo;  //������������
    public PoolManager.EnemyType enemyType; //�����Ҷ� �ѱ� �Ķ����
    public int stageSpawnInteval;  //�� ���������� ���� ��������
    public int stageSpawnNum;  //���� 
    public int stageTime; //���������ð�
    public int stageEnableSpawnPt; //���׹� ��ȯ 
    public int stageGold; // 스테이지 골드
}
