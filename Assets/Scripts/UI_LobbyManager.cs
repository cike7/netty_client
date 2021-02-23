using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyManager : MonoBehaviour
{
    /// <summary>
    /// 自己信息，后续不变
    /// </summary>
    public static UserInfo myselfInfo;

    /// <summary>
    /// 暂存包数据，等待场景加载脚本执行start方法
    /// </summary>
    private static DataPackage package;

    [SerializeField, Header("导航栏面板")]
    private AppBarPanel appBarPanel;

    [Header("活动面板")]
    [SerializeField, Tooltip("设置面板")]
    private RectTransform settingPanel;

    [SerializeField, Tooltip("功能面板")]
    private RectTransform functionPanel;

    [SerializeField, Tooltip("聊天面板")]
    private RectTransform chatPanel;

    [SerializeField, Tooltip("好友信息面板")]
    private RectTransform friendInfoPanel;

    [SerializeField, Tooltip("通讯录面板")]
    private RectTransform contactsPanel;

    [SerializeField, Tooltip("消息面板")]
    private RectTransform messagePanel;

    private UI_LobbyManager(){
        lobbyManager = this;
    }

    /// <summary>
    /// 大厅面板管理单例
    /// </summary>
    public static UI_LobbyManager lobbyManager;

    /// <summary>
    /// 好友id及信息
    /// </summary>
    //[HideInInspector]
    internal Dictionary<int, FriendInfo> friendInfos;

    private void Awake()
    {
        friendInfos = new Dictionary<int, FriendInfo>();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (package == null) return;

        //设置自己的信息
        myselfInfo = package.userInfo;

    }

    // Start is called before the first frame update
    void Start()
    {
        appBarPanel.B_Head.GetComponent<Button>().onClick.AddListener(GetHeadImageInfo); 
        appBarPanel.B_Mes.GetComponent<Button>().onClick.AddListener(StartMessageActivity);
        appBarPanel.B_Contacts.GetComponent<Button>().onClick.AddListener(StartContactsActivity);
        appBarPanel.B_Function.GetComponent<Button>().onClick.AddListener(StartFunctionActivity);
        appBarPanel.B_Setting.GetComponent<Button>().onClick.AddListener(StartSettingActivity);

     
        if (package == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("login");
        }
        else
        {
            //初始化本地消息
            Invoke("LoadLocalHistories", 0.3f);
            //初始化网络消息
            Invoke("InitializeUIMessage", 0.5f);
        }

    }

    /// <summary>
    /// 加载历史聊天记录
    /// </summary>
    void LoadLocalHistories()
    {
        if (ChatHistoryManager.GetInstance().LoadChatHistory() != null)
        {
            //加载本地所有好友信息
            foreach (FriendInfo localFriend in ChatHistoryManager.GetInstance().LoadChatHistory())
            {
                //调用委托实例消息列表
                UI_MessageControl.setFriend(localFriend);

                //调用委托实例通讯录列表
                UI_ContactsManager.setFriend(localFriend);

                if (!friendInfos.ContainsKey(localFriend.friend_id))
                {
                    friendInfos.Add(localFriend.friend_id, localFriend);
                }
            }
        }
    }


    /// <summary>
    /// 初始化好友UI消息名单
    /// </summary>
    void InitializeUIMessage()
    {
        if (package == null) return;

        //所有网络好友信息
        foreach (FriendInfo friend in package.friendList)
        {
            //调用委托实例消息列表
            if (!LitJson.JsonMapper.ToJson(friend.chat_message).Equals("[]"))
            {
                //调用委托实例消息列表
                UI_MessageControl.setFriend(friend);

                InsertOfflineMessage(friend);
            }

            //调用委托实例通讯录列表
            UI_ContactsManager.setFriend(friend);

            if (!friendInfos.ContainsKey(friend.friend_id))
            {
                friendInfos.Add(friend.friend_id, friend);
            }
        }
    }

    /// <summary>
    /// 插入离线留言消息
    /// </summary>
    /// <param name="info"></param>
    private void InsertOfflineMessage(FriendInfo info)
    {
        //检查是否已经生成UI通讯录UI
        if (!lobbyManager.friendInfos.ContainsKey(info.friend_id))
        {
            if (info.friend_attest == 1)
            {
                foreach (var msg in info.chat_message)
                {
                    //插入历史聊天记录
                    ChatHistoryManager.GetInstance().SaveChatHistory(friendInfos[info.friend_id], 1, msg.msg);
                }
            }
            else
            {
                Debug.Log("不是好友不保存");
            }
        }
    }


    void GetHeadImageInfo()
    {
        print("得到头像信息");
    }


    /// <summary>
    /// 激活聊天面板
    /// </summary>
    public void StartMessageActivity()
    {
        chatPanel.gameObject.SetActive(true);
        messagePanel.gameObject.SetActive(true);
        contactsPanel.gameObject.SetActive(false);
        functionPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        friendInfoPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 激活通讯录面板
    /// </summary>
    private void StartContactsActivity()
    {
        contactsPanel.gameObject.SetActive(true);
        friendInfoPanel.gameObject.SetActive(false);
        chatPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(false);
        functionPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 激活功能面板
    /// </summary>
    private void StartFunctionActivity()
    {
        functionPanel.gameObject.SetActive(true);
        contactsPanel.gameObject.SetActive(false);
        chatPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);
        friendInfoPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 激活设置面板
    /// </summary>
    private void StartSettingActivity()
    {
        settingPanel.gameObject.SetActive(true);
        functionPanel.gameObject.SetActive(false);
        contactsPanel.gameObject.SetActive(false);
        chatPanel.gameObject.SetActive(false);
        messagePanel.gameObject.SetActive(false);
        friendInfoPanel.gameObject.SetActive(false);
    }


    /// <summary>
    /// 得到网络初始化数据
    /// </summary>
    /// <param name="msg"></param>
    public static void GetInitializeData(DataPackage pack)
    {
        package = pack;
    }

    /// <summary>
    /// 侧边导航栏面板
    /// </summary>
    [System.Serializable,SerializeField]
    private class AppBarPanel
    {
        [SerializeField, Tooltip("头像按钮")]
        internal Button B_Head;

        [SerializeField, Tooltip("消息按钮")]
        internal Button B_Mes;

        [SerializeField, Tooltip("通讯录按钮")]
        internal Button B_Contacts;

        [SerializeField, Tooltip("功能按钮")]
        internal Button B_Function;

        [SerializeField, Tooltip("设置按钮")]
        internal Button B_Setting;
    }

}


