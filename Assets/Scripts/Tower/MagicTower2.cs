using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower2 : MagicTowerBase
{
    // 스탯 조정
    private void Awake()
    {
        InitTower(65, 1.4f, 300);
    }
}
