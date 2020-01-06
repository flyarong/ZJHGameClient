using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol.Code;
using Protocol.Dto;

public class MatchHandler : BaseHandler {

    public override void OnReceive(int subCode, object value)
    {
        Debug.Log("--比赛接收消息--"  + subCode);
        switch (subCode)
        {
            case MatchCode.Enter_SRES:
                EnterRoomSRES(value as MatchRoomDto);
                break;
            case MatchCode.Enter_BRO:
                EnterRoomBRO(value as UserDto);
                break;
            case MatchCode.Leave_BRO:
                LeaveBRO((int)value);
                break;
            case MatchCode.Read_BRO:
                ReadyBRO((int)value);
                break;
            case MatchCode.UnReady_BRO:
                UnReadyBRO((int)value);
                break;
            case MatchCode.StartGame_BRO:
                StartGame_BRO();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 开始游戏的广播
    /// </summary>
    private void StartGame_BRO()
    {
        EventCenter.Broadcast(EventDefine.Hint, "开始发牌");
        EventCenter.Broadcast(EventDefine.StartGame);
    }

    /// <summary>
    /// 取消准备的服务器广播
    /// </summary>
    /// <param name="unReadyUserId"></param>
    private void UnReadyBRO(int unReadyUserId)
    {
        Models.GameModel.matchRoomDto.UnReady(unReadyUserId);
        //刷新左右玩家的显示
        EventCenter.Broadcast(EventDefine.RefreshUI);
    }

    /// <summary>
    /// 有玩家准备了,服务器发的广播
    /// </summary>
    /// <param name="readyUserId"></param>
    private void ReadyBRO(int readyUserId)
    {
        Models.GameModel.matchRoomDto.Ready(readyUserId);
        //刷新左右玩家的显示
        EventCenter.Broadcast(EventDefine.RefreshUI);
    }

    /// <summary>
    /// 客户端请求进入房间，服务器的响应
    /// </summary>
    /// <param name="dto"></param>
    private void EnterRoomSRES(MatchRoomDto dto)
    {
        Models.GameModel.matchRoomDto = dto;
        ResetPosition();
        //刷新左右边玩家的UI显示
        EventCenter.Broadcast(EventDefine.RefreshUI);
    }

    /// <summary>
    /// 他人进入房间服务器的广播
    /// </summary>
    /// <param name="dto"></param>
    private void EnterRoomBRO( UserDto dto)
    {
        Models.GameModel.matchRoomDto.Enter(dto);
        ResetPosition();
        
        //刷新左右边玩家的UI显示
        EventCenter.Broadcast(EventDefine.RefreshUI);
        EventCenter.Broadcast(EventDefine.Hint, "有玩家"+dto.UserName+"进入房间");
    }

    /// <summary>
    /// 给房间内的玩家排序
    /// </summary>
    private void ResetPosition()
    {
        MatchRoomDto dto = Models.GameModel.matchRoomDto;
        dto.ResetPosition(Models.GameModel.userDto.UserId);
    }

    /// <summary>
    /// 有玩家离开服务器的广播
    /// </summary>
    /// <param name="leaveUserId"></param>
    private void LeaveBRO(int leaveUserId)
    {
        Models.GameModel.matchRoomDto.Leave(leaveUserId);
        ResetPosition();

        //刷新左右边玩家的UI显示
        EventCenter.Broadcast(EventDefine.RefreshUI);
    }
}
