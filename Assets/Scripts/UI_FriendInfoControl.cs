using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FriendInfoControl : MonoBehaviour
{
    [SerializeField, Tooltip("好友头像")]
    private Image i_head;

    [SerializeField, Tooltip("好友性别")]
    private Image i_sex;

    [SerializeField, Tooltip("好友名字")]
    private Text t_Name;

    [SerializeField, Tooltip("好友备注")]
    private Text t_Remarks;

    [SerializeField, Tooltip("好友地区")]
    private Text t_Address;

    [SerializeField, Tooltip("好友状态")]
    private Text t_Status;

    [SerializeField, Tooltip("好友签名")]
    private Text t_Sign;

    //好友id
    private int friendId;

    /// <summary>
    /// 委托好友信息
    /// </summary>
    public static SetFriendInfo setFriend;


    // Start is called before the first frame update
    void Start()
    {
        setFriend += SetFriendRelevantInfo;
        GetComponentInChildren<Button>().onClick.AddListener(ToFriendSendMessage);
    }

    /// <summary>
    /// 设置好友相关信息
    /// </summary>
    /// <param name="info"></param>
    private void SetFriendRelevantInfo(FriendInfo info)
    {
        if (info.friend_sex == 1)//男
        {
            i_sex.color = Color.blue;
        }
        else if(info.friend_sex == 2)//女
        {
            i_sex.color = Color.red;
        }
        else//未知
        {
            i_sex.color = Color.gray;
        }

        //名字
        t_Name.text = info.friend_name;

        //备注
        t_Remarks.text = info.friend_remarks;

        //地区
        t_Address.text = info.friend_address;

        if (info.friend_status == 1)
        {
            t_Status.text = "在线";
        }
        else if (info.friend_status == 0)
        {
            t_Status.text = "离线";
        }

        //签名
        t_Sign.text = info.friend_sign;

        //id
        friendId = info.friend_id;
    }

    /// <summary>
    /// 给朋友发消息
    /// </summary>
    private void ToFriendSendMessage()
    {
        //激活聊天面板
        UI_LobbyManager.lobbyManager.StartMessageActivity();

        if (UI_ChatPanelManagers.GetChatPanelInstance.ui_ChatPanels.ContainsKey(friendId))
        {
            UI_ChatPanelManagers.GetChatPanelInstance.ShowChatPanel(friendId);
        }
        else
        {
            UI_MessageControl.setFriend(UI_LobbyManager.lobbyManager.friendInfos[friendId]);
        }
    }

}
