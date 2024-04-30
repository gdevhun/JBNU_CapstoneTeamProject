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
    public Enemy bossEnemy; // 보스 Enemy 스크립트
    public GameObject bossHpPanel; // 보스 체력 패널
    public Image bossHpBar; // 보스 체력바
    public bool isBossStage; // 보스 스테이지
    public Sprite lastBossSprite; // 최종보스 스프라이트
    public Image lastBossImage; // 최종보스 이미지

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        _nexusHp = 1000;
    }

    private void Update()
    {
        if(!isBossStage) return;
        InitBossHpBar();
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
    private void InitBossHpBar()
    {
        bossHpBar.fillAmount = (float)bossEnemy.hp / (float)bossEnemy.maxHp;
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
