using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel
{
    /// <summary>
    /// 用户信息传输模型
    /// </summary>
    public UserDto userDto { get; set; }

    /// <summary>
    /// 底注
    /// </summary>
    public int BottomStakes { get; set; }

    /// <summary>
    /// 顶注
    /// </summary>
    public int TopStakes { get; set; }
}
