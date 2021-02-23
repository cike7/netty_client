using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class SendInfo : MonoBehaviour
{
    private static Socket socket;

    //发送数据包
    private static DataPackage data;

    //我自己信息
    private static UserInfo myUser;

    // Start is called before the first frame update
    void Start()
    {
        socket = ConnectInfo.GetInstance().GetSocket;

        data = new DataPackage();

        myUser = new UserInfo();
    }


    /// <summary>
    /// 发送登入注册请求消息
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <param name="type">请求类型</param>
    public static void SendRequestMessage(UserInfo user, byte type)
    {
        data.userInfo = user;
        data.requestType = type;
        SendToServer(LitJson.JsonMapper.ToJson(data));//序列化字符串
    }

    /// <summary>
    /// 发送添加好友请求
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <param name="type">请求类型</param>
    public static void SendAddRequest(int friendId, byte type,string remarks)
    {
        if (myUser.user_id == 0)
        {
            myUser.user_id = UI_LobbyManager.myselfInfo.user_id;
        }

        data.userInfo = myUser;
        data.friendId = friendId;
        data.requestType = type;
        data.data = System.Text.Encoding.UTF8.GetBytes(remarks);
        SendToServer(LitJson.JsonMapper.ToJson(data));//序列化字符串
    }

    /// <summary>
    /// 发送注册验证请求消息
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <param name="type">请求类型</param>
    public static void SendRequestMessage(UserInfo user, string code, byte type)
    {
        data.userInfo = user;
        data.requestType = type;
        data.data = System.Text.Encoding.UTF8.GetBytes(code);
        SendToServer(LitJson.JsonMapper.ToJson(data));//序列化字符串
    }


    /// <summary>
    /// 发送聊天请求消息
    /// </summary>
    /// <param name="user">聊天对象用户信息</param>
    /// <param name="type">请求类型</param>
    public static void SendChatMessage(int friendId,string msg)
    {
        if(myUser.user_id == 0)
        {
            myUser.user_id = UI_LobbyManager.myselfInfo.user_id;
        }

        data.userInfo = myUser;
        data.requestType = RequestType.chat;
        data.friendId = friendId;
        data.data = System.Text.Encoding.UTF8.GetBytes(msg);
        SendToServer(LitJson.JsonMapper.ToJson(data));//序列化字符串

    }


    /// <summary>
    /// 发送好友认证请求
    /// </summary>
    /// <param name="friendId"></param>
    /// <param name="attest"></param>
    public static void SendAuthenticationRequest(int friendId,int attest)
    {
        if (myUser.user_id == 0)
        {
            myUser.user_id = UI_LobbyManager.myselfInfo.user_id;
        }

        data.userInfo = myUser;
        data.requestType = RequestType.friend_attest;
        data.friendId = friendId;
        data.status = attest;
        SendToServer(LitJson.JsonMapper.ToJson(data));//序列化字符串
    }

    /// <summary>
    /// 发送操作指令
    /// </summary>
    public static void SendChatMsgCommand(DataPackage com)
    {
        SendToServer("");
    }

    /// <summary>
    /// 发送游戏数据
    /// </summary>
    public static void SendGameCommand(GameInfo game)
    {
        if (myUser.user_id == 0)
        {
            myUser.user_id = UI_LobbyManager.myselfInfo.user_id;
        }

        data.requestType = RequestType.game;

        string str = LitJson.JsonMapper.ToJson(game);

        data.data = System.Text.Encoding.UTF8.GetBytes(str);

        SendToServer(LitJson.JsonMapper.ToJson(data));//序列化字符串

    }


    //向服务端发送一条字符
    private static void SendToServer(string str)
    {
        Debug.Log(str);

        byte[] data = System.Text.Encoding.UTF8.GetBytes(str + "\n");

        if (!socket.Connected)
        {
            socket.Close();
            return;
        }
        try
        {
            IAsyncResult asyncSend = socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            //5秒延时
            bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                Debug.LogError("发送消息服务器失败");
            }
        }
        catch
        {
            //Loom.ins.Alert(this,);
            Debug.LogError("发送信息错误");
        }
    }

    private static void SendCallback(IAsyncResult asyncConnect)
    {
        Debug.Log("发送成功！");
    }

}
