using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefab_FriendMsg : MonoBehaviour
{
    /// <summary>
    /// 好友信息
    /// </summary>
    [HideInInspector]
    internal FriendInfo friendinfo;

    [Header("聊天面板预制体")]
    private GameObject chatPanelPerfab;

    /// <summary>
    /// 聊天面板
    /// </summary>
    private UI_ChatPanelControl chatControl;

    private void Awake()
    {
        chatPanelPerfab = Resources.Load<GameObject>("ChatPanel");
    }

    // Start is called before the first frame update
    void Start()
    {
        //点击按钮
        GetComponent<Button>().onClick.AddListener(OnClickMsg);

        //实例化聊天面板
        chatControl = Instantiate(chatPanelPerfab.gameObject, UI_ChatPanelManagers.GetChatPanelInstance.GetComponent<RectTransform>().rect.size, Quaternion.identity).GetComponent<UI_ChatPanelControl>();

        //设置父级
        chatControl.transform.SetParent(UI_ChatPanelManagers.GetChatPanelInstance.transform);

        //传递好友信息
        chatControl.friendInfo = friendinfo;

        //添加到聊天面板管理
        UI_ChatPanelManagers.GetChatPanelInstance.AddChatPanel(friendinfo.friend_id, chatControl);

        //设置名字
        GetComponentInChildren<Text>().text = friendinfo.friend_name.ToString();


        //实例话消息
        foreach (FriendInfo.Content content in friendinfo.chat_message)
        {
            if (content.type == 0)
            {
                chatControl.CreateMyselfMsgText(content.msg);
            }
            else
            {
                chatControl.CreateFriendMsgText(content.msg);
            }
        }

    }


    /// <summary>
    /// 点击好友UI时关闭其他好友Ui
    /// </summary>
    void OnClickMsg()
    {
        //设置名字
        chatControl.SetChatObjectName(friendinfo.friend_name);

        //关闭其他所有聊天面板
        foreach (UI_ChatPanelControl chatPanel in UI_ChatPanelManagers.GetChatPanelInstance.ui_ChatPanels.Values)
        {
            if(chatPanel == chatControl)
            {
                chatPanel.gameObject.SetActive(true);
            }
            else
            {
                chatPanel.gameObject.SetActive(false);
            }
        }
    }

}
