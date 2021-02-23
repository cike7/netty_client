using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameType
{
    /// <summary>
    /// 创建房间
    /// </summary>
    public static byte game_create = 1;

    /// <summary>
    /// 邀请
    /// </summary>
    public static byte game_invite = 2;

    /// <summary>
    /// 加入
    /// </summary>
    public static byte game_join = 3;

    /// <summary>
    /// 离开
    /// </summary>
    public static byte game_quit = 4;

    /// <summary>
    ///  战斗
    /// </summary>
    public static byte game_battle = 5;

    /// <summary>
    /// 刷新好友列表
    /// </summary>
    public static byte game_refresh = 6;
}
