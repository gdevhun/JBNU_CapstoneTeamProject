using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower2 : TowerBase
{
    // 스탯 조정
    private void Awake()
    {
        basicDamage = 150;
        attackSpeed = 0.5f;
        towerUpgradeBasicPrice = 150;
    }
}
