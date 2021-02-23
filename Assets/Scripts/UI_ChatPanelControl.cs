using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChatPanelControl : MonoBehaviour
{
    [SerializeField, Tooltip("聊天滚动视图面板")]
    private Transform content;

    [SerializeField, Tooltip("聊天对象文本")]
    private Text T_ObjectName;

    [SerializeField, Tooltip("聊天对象相关按钮")]
    private Button B_ObjcetRelated;

    [SerializeField, Tooltip("聊天内容输入框")]
    private InputField I_Content;

    [SerializeField, Tooltip("发送按钮")]
    private Button B_Send;

    [SerializeField, Tooltip("我自己消息预制体")]
    private GameObject MsgMySelfPrefab;

    [SerializeField, Tooltip("朋友消息预制体")]
    private GameObject MsgFriendPrefab;

    /// <summary>
    /// 好友的信息
    /// </summary>
    internal FriendInfo friendInfo;

    // Start is called before the first frame update
    void Start()
    {       
        //发送按钮监听
        B_Send.onClick.AddListener(ButSendChatMsg);
        //添加回车监听
        //I_Content.onEndEdit.AddListener(EntSendChatMsg);

        GetComponent<RectTransform>().offsetMin = Vector2.zero;
        GetComponent<RectTransform>().offsetMax = Vector2.zero;
        GetComponent<RectTransform>().anchorMin = Vector2.zero;
        GetComponent<RectTransform>().anchorMax = Vector2.one;
        GetComponent<RectTransform>().localScale = Vector3.one;

        friendInfo.chat_message = new List<FriendInfo.Content>();

        gameObject.SetActive(false);
    }


    /// <summary>
    /// 设置聊天对象名字
    /// </summary>
    /// <param name="name"></param>
    public void SetChatObjectName(string name)
    {
        gameObject.SetActive(true);

        T_ObjectName.text = name;
    }

    /// <summary>
    /// 按钮发送消息
    /// </summary>
    public void ButSendChatMsg()
    {
        if (CreateMyselfMsgText(I_Content.text))//先生成UI消息
        {
            SendInfo.SendChatMessage(friendInfo.friend_id, I_Content.text);

            ChatHistoryManager.GetInstance().SaveChatHistory(friendInfo, 0, I_Content.text);
            //清空
            TextClear();
        }
    }

    /// <summary>
    /// 回车发送消息
    /// </summary>
    //public void EntSendChatMsg(string msg)
    //{
    //    if (CreateMyselfMsgText(msg))//先生成UI消息
    //    {
    //        foreach (int friendId in UI_ChatPanelManagers.GetChatPanelInstance.uI_ChatPanels.Keys)
    //        {
    //            //根据值找到对应的id
    //            if (UI_ChatPanelManagers.GetChatPanelInstance.uI_ChatPanels[friendId].Equals(this))
    //            {
    //                SendInfo.SendChatMessage(friendId, msg);
                   
    //                TextClear();
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 创建朋友消息文本
    /// </summary>
    public void CreateFriendMsgText(string msg)
    {
        GameObject msgUI = Instantiate(MsgFriendPrefab, content.position, Quaternion.identity);

        msgUI.GetComponentInChildren<Text>().text = msg;

        msgUI.transform.SetParent(content);
    }

    /// <summary>
    /// 创建我自己消息文本
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public bool CreateMyselfMsgText(string msg)
    {
        if (msg != "")
        {
            GameObject msgUI = Instantiate(MsgMySelfPrefab, content.position, Quaternion.identity);

            msgUI.GetComponentInChildren<Text>().text = msg;

            msgUI.transform.SetParent(content);

            I_Content.placeholder.GetComponent<Text>().text = "";

            return true;
        }
        else
        {
            InputNullPrompt();
            return false;
        }
    }


    //输入空提示
    private void InputNullPrompt()
    {
        //输入框text组件
        I_Content.placeholder.GetComponent<Text>().text = "输入内容不能为空";

        Invoke("TextClear",1f);
    }

    //清空
    private void TextClear()
    {
        I_Content.text = "";
        I_Content.placeholder.GetComponent<Text>().text = "";
        I_Content.ActivateInputField();
    }

}
