using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 1)]
public class StageData : ScriptableObject
{
    public string stageInfo;  //스테이지정보
    public int stageSpawnInteval;  //각 스테이지가 가진 스폰간격
    public int stageSpawnNum;  //스폰 
    public int stageTime; //스테이지시간

}
