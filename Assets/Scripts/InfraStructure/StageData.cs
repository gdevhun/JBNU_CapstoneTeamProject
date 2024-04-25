using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 1)]
public class StageData : ScriptableObject
{
    public string stageInfo;  //������������
    public int stageSpawnInteval;  //�� ���������� ���� ��������
    public int stageSpawnNum;  //���� 
    public int stageTime; //���������ð�

}
