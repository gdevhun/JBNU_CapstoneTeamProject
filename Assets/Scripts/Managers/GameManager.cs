using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    private bool _isGameOver;
    private float _nexusHp;
    public Image nexusHpBar;

    public void NexusDamaged(float dmg)
    {
        _nexusHp -= dmg;
        if (_nexusHp > 0)
        {
            InitNexusHpBar();
        }
        else
        {
            _nexusHp = 0;
            InitNexusHpBar();
            _isGameOver = true;
        }
        
    }
    private void InitNexusHpBar()
    {
        nexusHpBar.fillAmount = _nexusHp;
    }
}
