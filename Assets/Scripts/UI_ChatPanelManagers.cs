using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChatPanelManagers : MonoBehaviour, ReceiveData
{
    [HideInInspector]//聊天界面id和面板
    internal Dictionary<int, UI_ChatPanelControl> ui_ChatPanels;

    /// <summary>
    /// 聊天面板实例
    /// </summary>
    public static UI_ChatPanelManagers GetChatPanelInstance;

    private void Awake()
    {
        GetChatPanelInstance = this;
        ui_ChatPanels = new Dictionary<int, UI_ChatPanelControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ReceiveManage.GetInstance().AddListeners(this);
    }

    /// <summary>
    /// 添加面板
    /// </summary>
    /// <param name="id"></param>
    /// <param name="uI_ChatPanel"></param>
    public void AddChatPanel(int id, UI_ChatPanelControl uI_ChatPanel)
    {
        //不包含则添加
        if (!ui_ChatPanels.ContainsValue(uI_ChatPanel))
            ui_ChatPanels.Add(id, uI_ChatPanel);
    }

    /// <summary>
    /// 移除面板
    /// </summary>
    /// <param name="id"></param>
    public void RemoveChatPanel(int id)
    {
        //包含则移除
        if (ui_ChatPanels.ContainsKey(id))
        {
            //移除面板
            ui_ChatPanels.Remove(id);
            //销毁
            Destroy(ui_ChatPanels[id]);
        }
    }

    /// <summary>
    /// 显示对应id的聊天面板
    /// </summary>
    /// <param name="id"></param>
    public void ShowChatPanel(int id)
    {
        foreach (UI_ChatPanelControl uI_ChatPanel in ui_ChatPanels.Values)
        {
            if (uI_ChatPanel == ui_ChatPanels[id])
            {
                uI_ChatPanel.gameObject.SetActive(true);
            }
            else
            {
                uI_ChatPanel.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 将接收消息
    /// 查找消息好友id
    /// </summary>
    /// <param name="pack"></param>
    public void ReceiveMessage(DataPackage pack)
    {
        if (pack.requestType == RequestType.chat)
        {
            // 用Loom的方法在Unity主线程中调用
            Loom.QueueOnMainThread((param) =>
            {
                if (UI_LobbyManager.myselfInfo.user_id.Equals(pack.userInfo.user_id))
                {
                    //判断聊天面板是否存在好友聊天面板
                    if (ui_ChatPanels.ContainsKey(pack.friendId))
                    {
                        string msg = System.Text.Encoding.UTF8.GetString(pack.data);

                        //创建消息
                        ui_ChatPanels[pack.friendId].CreateFriendMsgText(msg);

                        //插入历史聊天记录
                        ChatHistoryManager.GetInstance().SaveChatHistory(UI_LobbyManager.lobbyManager.friendInfos[pack.friendId], 1, msg);
                    }
                    else
                    {
                        //判断通讯录里是否有好友
                        if (UI_LobbyManager.lobbyManager.friendInfos.ContainsKey(pack.friendId))
                        {
                            //创建面板
                            UI_MessageControl.setFriend(UI_LobbyManager.lobbyManager.friendInfos[pack.friendId]);

                            /*
                             * 加入Dictionary，但是刷新不出来，第二次接收消息时才能刷新
                             */
                            ui_ChatPanels[pack.friendId].CreateFriendMsgText(System.Text.Encoding.UTF8.GetString(pack.data));
                        }
                    }
                }
                else
                {
                    Debug.LogError("不是自己好友的消息");
                    Debug.Log("聊天：" + LitJson.JsonMapper.ToJson(pack));
                }

            }, null);
        }

    }


}

