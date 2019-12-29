using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpInfoUI : BaseGameUI
{
    public Image hpBar;

    private void OnEnable()
    {
        GameManager.GameStartAction += RefreshUI;
        Player.DamagedAction += RefreshUI;
    }

    private void OnDisable()
    {
        GameManager.GameStartAction -= RefreshUI;
        Player.DamagedAction -= RefreshUI;
    }

    public override void RefreshUI()
    {
        int hp = GameManager.Instance.player.HP;
        int maxHp = GameManager.Instance.player.maxHP;

        hpBar.fillAmount = (float)hp / (float)maxHp;
    }
}
