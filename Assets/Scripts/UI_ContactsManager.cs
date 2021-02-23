using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void FriendInfoPanelActive();

public delegate void SetFriendMsg(FriendInfo info);

public class UI_ContactsManager : MonoBehaviour
{
    [SerializeField, Tooltip("滚动视图面板")]
    private RectTransform content;

    [SerializeField, Tooltip("新好友面板")]
    private RectTransform newFriendPanel;

    [SerializeField, Tooltip("好友信息面板")]
    private RectTransform friendInfoPanel;

    [SerializeField, Tooltip("搜索好友框")]
    private InputField I_search;

    [SerializeField, Tooltip("新好友按钮")]
    private Button B_newFriend;


    /// <summary>
    /// 朋友面板激活状态
    /// </summary>
    public static FriendInfoPanelActive friendPanelActive;

    /// <summary>
    /// 好友相关信息预制体
    /// </summary>
    private GameObject friendInfoPrefap;

    /// <summary>
    /// 委托好友信息
    /// </summary>
    public static SetFriendMsg setFriend;

    [Tooltip("新好友预制体")]
    private GameObject newFriend;

    private void Awake()
    {
        friendInfoPrefap = Resources.Load<GameObject>("FriendInfoPrefap");
        newFriend = Resources.Load<GameObject>("FriendRequestBut");
    }

    // Start is called before the first frame update
    void Start()
    {
        setFriend += SetFriendRelevantInfo;
        friendPanelActive += FriendInfoActive;
        B_newFriend.onClick.AddListener(NewFriendActive);
        NewFriendActive();
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 新朋友按钮
    /// </summary>
    void NewFriendActive()
    {
        newFriendPanel.gameObject.SetActive(true);
        friendInfoPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 朋友信息激活
    /// </summary>
    void FriendInfoActive()
    {
        friendInfoPanel.gameObject.SetActive(true);
        newFriendPanel.gameObject.SetActive(false);
    }


    /// <summary>
    /// 设置好友列表
    /// </summary>
    /// <param name="info"></param>
    private void SetFriendRelevantInfo(FriendInfo info)
    {
        //检查是否已经生成UI通讯录UI
        if (!UI_LobbyManager.lobbyManager.friendInfos.ContainsKey(info.friend_id))
        {
            if(info.friend_attest == 1)
            {
                Prefab_FriendRelevantInfo friend = Instantiate(friendInfoPrefap, content.position, Quaternion.identity).GetComponent<Prefab_FriendRelevantInfo>();

                //设置好友信息
                friend.info = info;

                friend.transform.SetParent(content);
            }
            else if (info.friend_attest == 0)
            {
                newFriendPanel.GetComponent<UI_NewFriendManager>().NewFriendRequest(info.friend_id, info.friend_remarks);

                Debug.Log("好友还未通过验证");
            }
        }
    }

}
