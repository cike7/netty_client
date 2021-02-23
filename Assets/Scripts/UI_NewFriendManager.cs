using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NewFriendManager : MonoBehaviour, ReceiveData
{

    [SerializeField, Header("查找输入框")]
    private InputField findInput;

    [SerializeField, Header("查找按钮")]
    private Button findBut;

    [SerializeField, Header("结果显示")]
    private Transform content;

    /// <summary>
    /// 添加好友预制体
    /// </summary>
    private GameObject friendAddPrefab;

    /// <summary>
    /// 请求添加好友预制体
    /// </summary>
    private GameObject friendRequestPrefab;

    //查找好友
    private Prefab_AddFriend searchFriend;

    private void Awake()
    {
        friendAddPrefab = Resources.Load<GameObject>("FriendAddBut");

        friendRequestPrefab = Resources.Load<GameObject>("FriendRequestBut");
    }

    // Start is called before the first frame update
    void Start()
    {
        findBut.onClick.AddListener(FindBut);
        ReceiveManage.GetInstance().AddListeners(this);
    }


    /// <summary>
    /// 查找好友
    /// </summary>
    private void FindBut()
    {
        if (searchFriend != null) Destroy(searchFriend.gameObject);

        if (!UI_LobbyManager.myselfInfo.user_id.Equals(int.Parse(findInput.text)))
        {
            if (UI_LobbyManager.lobbyManager.friendInfos.ContainsKey(int.Parse(findInput.text)))
            {
                Debug.Log("已经是好友了");
            }
            else
            {
                //发送查找好友请求
                SendInfo.SendAddRequest(int.Parse(findInput.text), RequestType.friend_search, "");
                findInput.text = "";
            }
        }
        else
        {
            Debug.Log("不可以添加自己为好友");
        }
    }

    /// <summary>
    /// 查找好友
    /// </summary>
    /// <param name="pack"></param>
    public void ReceiveMessage(DataPackage pack)
    {
        if(pack.requestType == RequestType.friend_search)
        {
            if(pack.status == 1)
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    SearchResults(pack.friendList[0]);

                }, null);

                Debug.Log("查找id结果：" + LitJson.JsonMapper.ToJson(pack.friendList[0]));
            }
            else
            {
                Debug.Log("查找失败");
            }

        }
        else if(pack.requestType == RequestType.friend_add)
        {
            // 用Loom的方法在Unity主线程中调用
            Loom.QueueOnMainThread((param) =>
            {
                //接收好友发送过来的请求，所以好友的id应该是发送者的消息
                NewFriendRequest(pack.userInfo.user_id,System.Text.Encoding.UTF8.GetString(pack.data));

            }, null);

        }
        else if (pack.requestType == RequestType.friend_attest)
        {
            // 用Loom的方法在Unity主线程中调用
            Loom.QueueOnMainThread((param) =>
            {
                UpdateFriendList(pack);

            }, null);
        }
 
    }


    /// <summary>
    /// 查找结果
    /// </summary>
    private void SearchResults(FriendInfo info)
    {
        searchFriend = Instantiate(friendAddPrefab, content.position,Quaternion.identity).GetComponent<Prefab_AddFriend>();

        searchFriend.transform.SetParent(content);

        searchFriend.info = info;

    }


    /// <summary>
    /// 新好友请求
    /// </summary>
    public void NewFriendRequest(int friendId, string remarks)
    {
        Prefab_NewFriend friendRequest = Instantiate(friendRequestPrefab, content.position, Quaternion.identity).GetComponent<Prefab_NewFriend>();

        friendRequest.transform.SetParent(content);

        friendRequest.friendId = friendId;

        friendRequest.remarks = remarks;
    }

    /// <summary>
    /// 更新好友列表
    /// </summary>
    /// <param name="pack"></param>
    private void UpdateFriendList(DataPackage pack)
    {
        //所有网络好友信息
        foreach (FriendInfo friend in pack.friendList)
        {
            //调用委托实例通讯录列表
            UI_ContactsManager.setFriend(friend);

            if (! UI_LobbyManager.lobbyManager.friendInfos.ContainsKey(friend.friend_id))
                UI_LobbyManager.lobbyManager.friendInfos.Add(friend.friend_id, friend);
        }
    }

}
