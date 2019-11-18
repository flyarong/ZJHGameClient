using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountHandler : BaseHandler
{
    public override void OnReceive(int subCode, object value)
    {
        Debug.Log("--wangzhi--为啥没接收到消息--"+subCode);
        switch (subCode)
        {
            case AccountCode.Register_SRES:
                Register_SRES((int)value);
                break;
            default:
                break;
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
