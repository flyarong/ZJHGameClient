using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RightManager_Stand : ComputerBaseManager_Stand
{
    /// <summary>
    /// 比牌
    /// </summary>
    public override void Compare()
    {
        m_ZJHManager.RightPlayerCompare();
    }
}
