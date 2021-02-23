using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameMatching : MonoBehaviour, ReceiveData
{
    [SerializeField, Header("开始游戏")]
    private Button playGameBut;

    //房间玩家
    public List<Prefab_FriendMatching> prefab_Friends;


    // Start is called before the first frame update
    void Start()
    {
        playGameBut.onClick.AddListener(PlayGameBut);

        ReceiveManage.GetInstance().AddListeners(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //开始游戏
    void PlayGameBut()
    {
        foreach(var i in prefab_Friends)
        {
            if(i.prepareStatus == false)
            {
                //Loom.ins.Alert(this.transform,"还有玩家为未准备");
                Debug.Log("还有玩家为未准备");
                return;
            }
            else
            {

            }
        }
    }

    public void ReceiveMessage(DataPackage pack)
    {
        if (pack.requestType == RequestType.game)
        {
            string stream = System.Text.Encoding.UTF8.GetString(pack.data);

            GameInfo gameInfo = LitJson.JsonMapper.ToObject<GameInfo>(stream);

            if (gameInfo.requestType == GameType.game_invite)
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    Loom.ins.Alert(this.transform,"好友邀请");
                }, null);

            }
            else if (gameInfo.requestType == GameType.game_join)
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    Loom.ins.Alert(this.transform, "在房间好友");
                }, null);
            }

            Debug.Log(stream);

            //GamePackage package = LitJson.JsonMapper.ToObject<GamePackage>(stream);

        }
    }
}
