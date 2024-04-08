using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManager : SingletonBehaviour<GoldManager>
{
    [SerializeField] private int playerGold;
    [SerializeField] TMP_Text totalGold;
    void Start()
    {
        playerGold = 0;
        InitUIGold();
    }

    public void InitUIGold()
    {
        totalGold.text = playerGold.ToString();
    }
    //골드 획득 함수
    public void AcquireGold(int amount)
    {   //Enemy가 죽어서 비활성화시킬때 함수 호출하면됨.
        playerGold += amount;
        InitUIGold();
    }

    public void UseGold(int amount)
    {
        // 사용하려는 돈이 플레이어의 소지금보다 작거나 같은지 확인
        if (amount <= playerGold)
        {
            playerGold -= amount; // 돈 사용

        }
        InitUIGold();
        return;
    }

}