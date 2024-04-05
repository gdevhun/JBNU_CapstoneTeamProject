using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower3 : TowerBase
{
    // 스탯 조정
    private void Awake()
    {
        basicDamage = 200;
        attackSpeed = 0.3f;
        towerUpgradeBasicPrice = 200;
    }
}
