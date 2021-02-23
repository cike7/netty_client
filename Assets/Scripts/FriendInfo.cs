using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendInfo
{
    /// <summary>
    /// ID
    /// </summary>
    public int friend_id;

    /// <summary>
    /// 好友名字
    /// </summary>
    public string friend_name;

    /// <summary>
    /// 在线状态
    /// </summary>
    public int friend_status;

    /// <summary>
    /// 好友验证
    /// </summary>
    public int friend_attest;

    /// <summary>
    /// 备注
    /// </summary>
    public string friend_remarks;

    /// <summary>
    /// 地区
    /// </summary>
    public string friend_address;

    /// <summary>
    /// 签名
    /// </summary>
    public string friend_sign;

    /// <summary>
    /// 性别
    /// </summary>
    public int friend_sex;

    /// <summary>
    /// 聊天记录
    /// </summary>
    public List<Content> chat_message = new List<Content>();

    /// <summary>
    /// 设置消息
    /// </summary>
    /// <param name="type">0:自己 1:朋友</param>
    /// <param name="message">聊天消息</param>
    public void SetMessage(int type,string message)
    {
        Content content = new Content();
        content.type = type;
        content.msg = message;
        chat_message.Add(content);
    }

    public class Content
    {
        /// <summary>
        /// 0:自己 1:朋友
        /// </summary>
        public int type;

        public string msg;
    }
}
