using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower3 : ArcherTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(30, 0.3f, 200);
    }
}
