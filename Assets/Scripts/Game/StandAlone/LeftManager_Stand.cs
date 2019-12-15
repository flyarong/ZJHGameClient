using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LeftManager_Stand : ComputerBaseManager_Stand
{
    /// <summary>
    /// 比牌
    /// </summary>
    public override void Compare()
    {
        m_ZJHManager.LeftPlayerCompare();
    }

    public override void Win()
    {
        base.Win();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        m_ZJHManager.m_CurrentStakesIndex = 1;
        m_ZJHManager.SetNextPlayerStakes();
        m_IsComparing = false;
    }

    public override bool IsWin()
    {
        if (m_ZJHManager.IsLeftWin())
        {
            m_ZJHManager.LeftWin();
            return true;
        }
        return false;
    }

}
