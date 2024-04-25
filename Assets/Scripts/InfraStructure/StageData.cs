using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "StageData", order = 1)]
public class StageData : ScriptableObject
{
    public string description = "실제 StageTime = 스폰인터벌 * 스폰 넘 + 2 + [추가 시간]";
    public string stageInfo;  //스테이지정보
    public PoolManager.EnemyType enemyType; //스폰할때 넘길 파라미터
    public int stageSpawnInteval;  //각 스테이지가 가진 스폰간격
    public int stageSpawnNum;  //스폰 
    public int stageTime; //스테이지시간
    public int stageEnemyNum; //에네미 소환 
}
