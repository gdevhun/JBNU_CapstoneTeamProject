using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower2 : ArcherTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(100, 0.75f, 200);
    }
}
