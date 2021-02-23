using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefab_FriendOnline : MonoBehaviour
{
    [SerializeField, Header("好友名字")]
    private Text nameText;

    [SerializeField, Header("邀请")]
    private Button inviteBut;

    /// <summary>
    /// 游戏信息
    /// </summary>
    internal GameInfo info;

    //禁用30s
    private float timer = 30;

    //禁用邀请
    private bool disable = false;

    // Start is called before the first frame update
    void Start()
    {
        if(info != null)
        {
            //nameText.text = UI_LobbyManager.myselfInfo.user_account;
        }
        inviteBut.onClick.AddListener(InviteBut);
    }

    void Update()
    {
        if (disable)
        {
            inviteBut.GetComponentInChildren<Text>().text = (int)timer + "";

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                disable = false;
                timer = 30;
                inviteBut.GetComponentInChildren<Text>().text = "邀请";
            }

        }
   
    }


    /// <summary>
    /// 邀请
    /// </summary>
    void InviteBut()
    {
        if(!disable)
            disable = true;

        SendInfo.SendGameCommand(info);
    }

}
