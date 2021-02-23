using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void SetFriendInfo(FriendInfo info);

public class UI_MessageControl : MonoBehaviour
{
    /// <summary>
    /// 好友预制体
    /// </summary>
    private GameObject friendMsgPrefap;

    [SerializeField, Tooltip("滚动视图面板")]
    private Transform content;

    /// <summary>
    /// 委托好友信息
    /// </summary>
    public static SetFriendInfo setFriend;

    private void Awake()
    {
        friendMsgPrefap = Resources.Load<GameObject>("FriendMsgPrefap");
    }

    // Start is called before the first frame update
    void Start()
    {
        setFriend += SetFriendRelevantInfo;
    }


    /// <summary>
    /// 实例化好友消息名单
    /// </summary>
    /// <param name="info"></param>
    /// <param name="insert">是否写入本地聊天记录</param>
    private void SetFriendRelevantInfo(FriendInfo info)
    {
        //如果本地已经存在UI
        if (UI_ChatPanelManagers.GetChatPanelInstance.ui_ChatPanels.ContainsKey(info.friend_id))
        {
            //实例话消息
            foreach (FriendInfo.Content content in info.chat_message)
            {
                if (content.type == 0)
                {
                    UI_ChatPanelManagers.GetChatPanelInstance.ui_ChatPanels[info.friend_id].CreateMyselfMsgText(content.msg);
                }
                else
                {
                    UI_ChatPanelManagers.GetChatPanelInstance.ui_ChatPanels[info.friend_id].CreateFriendMsgText(content.msg);
                }
            }
        }
        else
        {
            //实例聊天消息UI
            Prefab_FriendMsg friend = Instantiate(friendMsgPrefap, content.position, Quaternion.identity).GetComponent<Prefab_FriendMsg>();
            //设置好友信息
            friend.friendinfo = info;
            //设置父级
            friend.transform.SetParent(content);
        }
    }

}
