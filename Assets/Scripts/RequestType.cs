using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestType
{

    /// <summary>
    /// 聊天
    /// </summary>
    public static int chat = 1;

    /// <summary>
    /// 电话请求
    /// </summary>
    public static byte phone_start = 2;

    /// <summary>
    /// 挂断电话
    /// </summary>
    public static byte phone_stop = 3;

    /// <summary>
    /// 添加好友
    /// </summary>
    public static byte friend_add = 4;

    /// <summary>
    /// 删除好友
    /// </summary>
    public static byte friend_delete = 5;

    /// <summary>
    /// 登入
    /// </summary>
    public static byte account_login = 6;

    /// <summary>
    /// 注册
    /// </summary>
    public static byte account_register = 7;

    /// <summary>
    /// 销户
    /// </summary>
    public static byte account_destroy = 8;

    /// <summary>
    /// 修改
    /// </summary>
    public static byte account_alter = 9;
    
    /// <summary>
    /// 天气
    /// </summary>
    public static byte weather = 10;

    /// <summary>
    /// 邮箱验证
    /// </summary>
    public static byte email_verify = 11;

    /// <summary>
    /// 查找好友
    /// </summary>
    public static byte friend_search = 12;

    /// <summary>
    /// 好友认证
    /// </summary>
    public static byte friend_attest = 13;

    /// <summary>
    /// 游戏
    /// </summary>
    public static byte game = 14;

}
