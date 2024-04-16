using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    public bool _isGameOver; //게임오버 BOOL 변수
    public Image nexusHpBar; //Image
    private int _nexusHp; //넥서스 hp
    protected void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }
    private int NexusHp
    {
        get => _nexusHp;

        set
        {
            _nexusHp = value;
            InitNexusHpBar();
            if (_nexusHp <= 0)
            {
                _nexusHp = 0;
                _isGameOver = true;
                PlayerGameOver();
            }
        }
    }
    public void NexusDamaged(int dmg)
    {
        NexusHp -= dmg;
    }
    private void InitNexusHpBar()
    {
        nexusHpBar.fillAmount = _nexusHp;
    }

    private void PlayerGameOver()
    {
        if (_isGameOver)
        {
            
        }

        return;
    }

    
    public void PauseGameBtn()
    {   //게임 일시정지 버튼
        Time.timeScale = 0f;
    }

    public void PlayAgainGameBtn()
    {   //게임 재개 버튼
        Time.timeScale = 1f;
    }
}
