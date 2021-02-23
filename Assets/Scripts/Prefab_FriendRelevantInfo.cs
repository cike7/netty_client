using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 好友名片消息
/// </summary>
public class Prefab_FriendRelevantInfo : MonoBehaviour
{
    /// <summary>
    /// 好友名片信息
    /// </summary>
    internal FriendInfo info;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowFriendRelevantInfo);

        GetComponentInChildren<Text>().text = info.friend_name.ToString();
    }


    /// <summary>
    /// 显示好友相关信息
    /// </summary>
    void ShowFriendRelevantInfo()
    {
        //设置好友信息
        UI_FriendInfoControl.setFriend(info);

        //关闭新好友面板显示好友信息面板
        UI_ContactsManager.friendPanelActive();
    }
}
