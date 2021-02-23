using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPackage
{
    /// <summary>
    /// 用户账号
    /// </summary>
    public UserInfo userInfo;

    /// <summary>
    /// 服务端反馈状态
    /// </summary>
    public int status;

    /// <summary>
    /// 请求类型
    /// </summary>
    public int requestType;

    /// <summary>
    /// 好友列表
    /// </summary>
    public List<FriendInfo> friendList;

    /// <summary>
    /// 请求好友信息
    /// </summary>
    public int friendId;

    /// <summary>
    /// 发送数据
    /// </summary>
    public byte[] data;
}
