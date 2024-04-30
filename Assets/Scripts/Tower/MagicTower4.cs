using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower4 : MagicTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(500, 1.2f, 800);
    }
}
