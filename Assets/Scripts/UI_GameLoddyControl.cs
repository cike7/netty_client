using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameLoddyControl : MonoBehaviour
{
    [SerializeField, Tooltip("模式选择面板")]
    private RectTransform modeChoosePanel;

    [SerializeField, Tooltip("单人面板")]
    private RectTransform singlePanel;

    [SerializeField, Tooltip("多人面板")]
    private RectTransform multiplayerPanel;

    [SerializeField, Tooltip("设置面板")]
    private RectTransform settingPanel;

    [SerializeField, Tooltip("好友列表面板")]
    private RectTransform friendListPanel;

    //面板控制
    private List<RectTransform> panelControl = new List<RectTransform>();



    //单人模式
    private Button singleBut;
    //多人模式
    private Button multiplayerBut;
    //设置
    private Button settingBut;
    //创建房间
    private Button createRoomBut;
    //加入房间
    private Button inviteFriendsJoinRoomBut;
    //退出房间
    private Button quitRoomBut;
    //返回
    private Button exitRoomBut;
    //挑战
    private Button challengeModeBut;
    //炼狱
    private Button purgatoryModeBut;
    //无尽
    private Button endlessModeBut;
    //返回
    private Button exitModeBut;
    //退出游戏
    private Button quitGameBut;
    //退出游戏
    private Button exitSettingBut;
    //退出好友列表
    private Button exitFriendListBut;


    //需要移动的模块
    private RectTransform moveRect;
    //移动位置
    Vector2 vector = new Vector2(-410, 0);


    // Start is called before the first frame update
    void Start()
    {

        Initialization();

        //移出第一块模板
        PanelManage(modeChoosePanel);

    }

    //初始化
    void Initialization()
    {
        singleBut = modeChoosePanel.GetChild(0).GetComponent<Button>();
        multiplayerBut = modeChoosePanel.GetChild(1).GetComponent<Button>();
        settingBut = modeChoosePanel.GetChild(2).GetComponent<Button>();


        createRoomBut = multiplayerPanel.GetChild(0).GetComponent<Button>();
        inviteFriendsJoinRoomBut = multiplayerPanel.GetChild(1).GetComponent<Button>();
        quitRoomBut = multiplayerPanel.GetChild(2).GetComponent<Button>();
        exitRoomBut = multiplayerPanel.GetChild(3).GetComponent<Button>();

        challengeModeBut = singlePanel.GetChild(0).GetComponent<Button>();
        purgatoryModeBut = singlePanel.GetChild(1).GetComponent<Button>();
        endlessModeBut = singlePanel.GetChild(2).GetComponent<Button>();
        exitModeBut = singlePanel.GetChild(3).GetComponent<Button>();

        quitGameBut = settingPanel.GetChild(0).GetComponent<Button>();
        exitSettingBut = settingPanel.GetChild(1).GetComponent<Button>();

        exitFriendListBut = friendListPanel.GetChild(1).GetComponent<Button>();

        //添加需要移动的面板
        panelControl.Add(modeChoosePanel);
        panelControl.Add(singlePanel);
        panelControl.Add(multiplayerPanel);
        panelControl.Add(settingPanel);
        panelControl.Add(friendListPanel);

        //给按钮注册事件
        singleBut.onClick.AddListener(SingleBut);
        multiplayerBut.onClick.AddListener(MultiplayerBut);
        settingBut.onClick.AddListener(SettingBut);
        createRoomBut.onClick.AddListener(CreateRoomBut);
        inviteFriendsJoinRoomBut.onClick.AddListener(InviteFriendsJoinRoomBut);
        quitRoomBut.onClick.AddListener(QuitRoomBut);
        exitRoomBut.onClick.AddListener(ExitRoomBut);
        challengeModeBut.onClick.AddListener(ChallengeModeBut);
        purgatoryModeBut.onClick.AddListener(PurgatoryModeBut);
        endlessModeBut.onClick.AddListener(EndlessModeBut);
        exitModeBut.onClick.AddListener(ExitModeBut);
        quitGameBut.onClick.AddListener(QuitGameBut);
        exitSettingBut.onClick.AddListener(ExitSettingBut);
        exitFriendListBut.onClick.AddListener(ExitFriendListBut);

    }

    #region

    void SingleBut()
    {
        PanelManage(singlePanel);
    }
    
    void MultiplayerBut()
    {
        PanelManage(multiplayerPanel);
    }
    
    void SettingBut()
    {
        PanelManage(settingPanel);
    }

    //创建房间
    void CreateRoomBut()
    {
        GameInfo gameData = new GameInfo
        {
            roomNumber = 10000,
            requestType = GameType.game_create,
        };

        SendInfo.SendGameCommand(gameData);
    }

    void InviteFriendsJoinRoomBut()
    {
        PanelManage(friendListPanel);
    }

    void QuitRoomBut()
    {
        //离开房间----
        PanelManage(modeChoosePanel);
    }

    void ExitRoomBut()
    {
        PanelManage(modeChoosePanel);
    }

    void ChallengeModeBut()
    {

    }

    void PurgatoryModeBut()
    {

    }

    void EndlessModeBut()
    {

    }

    void ExitModeBut()
    {
        PanelManage(modeChoosePanel);
    }

    void QuitGameBut()
    {

    }

    void ExitSettingBut()
    {
        PanelManage(modeChoosePanel);
    }

    void ExitFriendListBut()
    {
        PanelManage(multiplayerPanel);
    }
    #endregion

    //移动管理
    private void PanelManage(RectTransform rect)
    {
        foreach(var i in panelControl)
        {
            if(i == rect)
            {
                moveRect = i;
            }
            else
            {
                i.anchoredPosition = new Vector2(-610, i.anchoredPosition.y);
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveRect != null)
        {
            if (moveRect.anchoredPosition.x < -410)
            {
                moveRect.anchoredPosition = Vector2.MoveTowards(moveRect.anchoredPosition, vector, 5);
            }
            else
            {
                moveRect = null;
            }
        }
    }

}
