using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower2 : TowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(150, 0.5f, 150);
    }
}
