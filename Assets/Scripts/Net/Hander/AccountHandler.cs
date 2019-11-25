using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountHandler : BaseHandler
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.Register_SRES:
                Register_SRES((int)value);
                break;
            case AccountCode.Login_SRES:
                Login_SRES((int)value);
                break;
            case AccountCode.GetUserInfo_SRES:
                Models.GameModel.userDto = (UserDto)value;
                Debug.Log(Models.GameModel.userDto.UserName + " " + Models.GameModel.userDto.IconName + " " + Models.GameModel.userDto.CoinCount);
                SceneManager.LoadScene("Main");
                break;
            case AccountCode.GetRankList_SRES:
                EventCenter.Broadcast(EventDefine.ShowRankListDto, value as RankListDto);
                break;
            case AccountCode.UpdateCoinCount_SRES:
                Models.GameModel.userDto.CoinCount = (int)value;
                EventCenter.Broadcast(EventDefine.UpdateCoinCount, (int)value);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 登录服务器的响应
    /// </summary>
    /// <param name="value"></param>
    private void Login_SRES(int value)
    {
        if (value == -1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "用户不存在");
        }
        else if (value == -2)
        {
            EventCenter.Broadcast(EventDefine.Hint, "密码不正确");
        }
        else if (value == -3)
        {
            EventCenter.Broadcast(EventDefine.Hint, "该账号已在线");
        }
        else if (value == 0)
        {
            EventCenter.Broadcast(EventDefine.Hint, "登录成功");
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.GetUserInfo_CREQ, null);
        }
    }

    /// <summary>
    /// 注册服务器的响应
    /// </summary>
    /// <param name="value"></param>
    private void Register_SRES(int value)
    {
        if (value == -1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "用户名已被注册");
            return;
        }
        if (value == 0)
        {
            EventCenter.Broadcast(EventDefine.Hint, "注册成功");
        }
    }
}
