using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefab_NewFriend : MonoBehaviour
{
    [SerializeField, Header("接受按钮")]
    private Button acceptBut;

    [SerializeField, Header("拒绝按钮")]
    private Button rejectBut;

    [SerializeField, Header("备注消息")]
    private Text remarksText;

    //存储好友的ID
    internal int friendId;

    //好友备注
    internal string remarks;

    // Start is called before the first frame update
    void Start()
    {
        acceptBut.onClick.AddListener(AcceptButton);
        rejectBut.onClick.AddListener(RejectButton);

        remarksText.text = "留言：" + remarks;
    }

    void AcceptButton()
    {
        print("接受");
        SendInfo.SendAuthenticationRequest(friendId, 1);
        Destroy(gameObject, 0.5f);
    }

    void RejectButton()
    {
        print("拒绝");
        SendInfo.SendAuthenticationRequest(friendId,0);
        Destroy(gameObject,0.5f);
    }

}
