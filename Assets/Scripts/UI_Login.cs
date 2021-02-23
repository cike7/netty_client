using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Login : MonoBehaviour ,ReceiveData
{
    [SerializeField, Tooltip("注册")]
    private GameObject enrollPanel;

    [SerializeField, Tooltip("提示文本")]
    private Text hintText;

    [SerializeField, Tooltip("邮箱")]
    private InputField emilInput;

    [SerializeField, Tooltip("密码")]
    private InputField passwordInput;

    [SerializeField, Tooltip("忘记密码")]
    private Button forgetPasswordBut;

    [SerializeField, Tooltip("登录")]
    private Button loginBut;

    [SerializeField, Tooltip("注册")]
    private Button enrollBut;

    /// <summary>
    /// 用户实例
    /// </summary>
    public static UserInfo UserInstance;


    // Start is called before the first frame update
    void Start()
    {
        ReceiveManage.GetInstance().AddListeners(this);

        UserInstance = new UserInfo();

        loginBut.onClick.AddListener(LoginBut);
        enrollBut.onClick.AddListener(EnrollBut);

        //实例化
        Loom.Initialize();

        enrollPanel.SetActive(false);
    }

    private void LoginBut()
    {
        UserInstance.user_account = emilInput.text;
        UserInstance.user_password = passwordInput.text;

        SendInfo.SendRequestMessage(UserInstance, RequestType.account_login);

        emilInput.text = "";
        passwordInput.text = "";

    }

    private void EnrollBut()
    {
        this.gameObject.SetActive(false);
        enrollPanel.SetActive(true);

    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="pack"></param>
    public void ReceiveMessage(DataPackage pack)
    {
        if (pack.requestType == RequestType.account_login)
        {            
            if (pack.status == 1)
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    //AsyncOperation async = SceneManager.LoadSceneAsync("lobby");
                    //async.allowSceneActivation = false;

                    ReceiveManage.GetInstance().RemoveIistListeners(this);
                    UI_LobbyManager.GetInitializeData(pack);
                    SceneManager.LoadScene("lobby");

                }, null);

            }
            else
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    hintText.text = "登录失败,密码错误或账号已存在";
                    Invoke("ClearText", 2f);

                }, null);
            }
        }
    }

    /// <summary>
    /// 清理文本
    /// </summary>
    public void ClearText()
    {
        hintText.text = "";
    }

}
