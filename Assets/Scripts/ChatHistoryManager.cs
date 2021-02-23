using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
///聊天记录插入本地 
/// </summary>
public class ChatHistoryManager
{
    //单例
    private static ChatHistoryManager chatHistoryManager;

    //本地路径
    private readonly string path = Application.persistentDataPath + UI_LobbyManager.myselfInfo.user_account + ".json";

    //聊天记录
    private ChatHistory chatHistory;

    //好友id
    private List<int> friendId;


    //构造函数
    private ChatHistoryManager()
    {
        //实例化聊天记录
        chatHistory = new ChatHistory
        {
            friends = new List<ChatHistory.Message>()
        };

        friendId = new List<int>();

        //判断文件是否存在
        if (!File.Exists(path))
        {
            //创建文件
            FileStream file = File.Create(path);
            //序列化需保存对象
            string data = LitJson.JsonMapper.ToJson(chatHistory);
            //转换字节
            byte[] vs = System.Text.Encoding.UTF8.GetBytes(data);
            //写入文件
            file.Write(vs, 0,vs.Length);
            //释放文件流
            file.Close();

        }
        else
        {
            string loadJson = File.ReadAllText(path);

            chatHistory = LitJson.JsonMapper.ToObject<ChatHistory>(loadJson);

            foreach(var k in chatHistory.friends)
            {
                friendId.Add(k.id);
            }
        }

    }

    //只能实例化一次
    public static ChatHistoryManager GetInstance()
    {
        if(chatHistoryManager == null)
        {
            chatHistoryManager = new ChatHistoryManager();
        }

        return chatHistoryManager;
    }


    /// <summary>
    /// 保存历史聊天记录
    /// </summary>
    /// <param name="flendinfo">好友消息</param>
    /// <param name="type">0是自己，1是好友</param>
    /// <param name="message">消息</param>
    public void SaveChatHistory(FriendInfo friend, int type, string message)
    {
        if (friendId.Contains(friend.friend_id))
        {
            for (int i = 0; i < chatHistory.friends.Count; i++)
            {
                if (chatHistory.friends[i].id == friend.friend_id)
                {
                    chatHistory.friends[i].info.SetMessage(type, message);
                }
            }
        }
        else
        {
            ChatHistory.Message friendMessage = new ChatHistory.Message
            {
                id = friend.friend_id,
                info = friend
            };

            friendMessage.info.SetMessage(type,message);

            //添加好友信息
            chatHistory.friends.Add(friendMessage);

            //添加id
            friendId.Add(friend.friend_id);
        }

    }

    //退出时保存聊天记录
    public void ExitAndSave()
    {
        Debug.Log(LitJson.JsonMapper.ToJson(chatHistory));

        File.WriteAllText(path, LitJson.JsonMapper.ToJson(chatHistory), Encoding.UTF8);

        Debug.Log("退出保存聊天记录");
    }
   

    /// <summary>
    /// 加载聊天记录
    /// </summary>
    /// <param name="flendId"></param>
    public List<FriendInfo> LoadChatHistory()
    {   
        string json = File.ReadAllText(path);

        if (json.Length < 1) return null;

        ChatHistory jsonHistory = LitJson.JsonMapper.ToObject<ChatHistory>(json);

        List<FriendInfo> friends = new List<FriendInfo>();

        for(int i= 0;i < jsonHistory.friends.Count; i++)
        {
            friends.Add(jsonHistory.friends[i].info);
        }

        return friends;

    }


    [System.Serializable]
    public class ChatHistory
    {
        public List<Message> friends;

        public class Message
        {
            public int id;
            public FriendInfo info;
        }
    }

}
