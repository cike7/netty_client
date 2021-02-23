using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefab_AddFriend : MonoBehaviour
{
    [SerializeField, Header("名字")]
    private Text nameText;

    [SerializeField, Header("性别")]
    private Text sexText;

    [SerializeField, Header("地区")]
    private Text addressText;

    [SerializeField, Header("签名")]
    private Text signText;

    [SerializeField, Header("验证消息")]
    private InputField remarksInput;

    [SerializeField, Header("添加按钮")]
    private Button addBut;


    //好友信息
    internal FriendInfo info;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = info.friend_name;

        if(info.friend_sex == 1)
        {
            sexText.text = "性别：" + "男";
        }
        else if(info.friend_sex == 2)
        {
            sexText.text = "性别：" + "女";
        }
        else
        {
            sexText.text = "性别："+"未知";
        }

        addressText.text = "地区：" + info.friend_address;
        signText.text = "签名："+ info.friend_sign;

        addBut.onClick.AddListener(AddButton);
    }

    void AddButton()
    {
        //发送好友添加请求
        SendInfo.SendAddRequest(info.friend_id, RequestType.friend_add, remarksInput.text);

        Destroy(gameObject,0.5f);

    }

}
