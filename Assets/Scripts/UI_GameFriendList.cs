using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameFriendList : MonoBehaviour,ReceiveData
{
    public List<GameInfo> gameFriends;

    // Start is called before the first frame update
    void Start()
    {
        ReceiveManage.GetInstance().AddListeners(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //刷新好友列表
    void RefreshFriendList()
    {
        GameInfo info = new GameInfo
        {
            requestType = GameType.game_refresh
        };

        SendInfo.SendGameCommand(info);
    }


    public void ReceiveMessage(DataPackage pack)
    {
        if(pack.requestType == RequestType.game)
        {
            string stream = System.Text.Encoding.UTF8.GetString(pack.data);

            Debug.Log("在线好友："+stream);

            GamePackage package = LitJson.JsonMapper.ToObject<GamePackage>(stream);

            // 用Loom的方法在Unity主线程中调用
            Loom.QueueOnMainThread((param) =>
            {
                Loom.ins.Alert(this.transform, "好友在线列表");
            }, null);
        }
    }
}
