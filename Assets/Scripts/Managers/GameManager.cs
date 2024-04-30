using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    public bool _isGameOver; //게임오버 BOOL 변수
    public Image nexusHpBar; //Image
    private float _nexusHp; //넥서스 hp
    public GameObject gameoverPanel, gameWinPanel; // 게임 패널

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        _nexusHp = 1000;
    }
    public float NexusHp
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
                PauseGameBtn();
                gameoverPanel.SetActive(true);
            }
        }
    }
    public void NexusDamaged(int dmg)
    {
        NexusHp -= dmg;
    }
    private void InitNexusHpBar()
    {
        nexusHpBar.fillAmount = _nexusHp / 1000f;
    }

    public void PlayerGameOver()
    {
        if (_isGameOver)
        {
            SoundManager.Instance.PlayBGM(SoundType.메뉴BGM);
            SceneManager.LoadScene("MenuScene");
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
