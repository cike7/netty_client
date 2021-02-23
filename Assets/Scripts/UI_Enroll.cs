using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enroll : MonoBehaviour, ReceiveData
{
    [SerializeField, Tooltip("登入")]
    private GameObject loginPanel;

    [SerializeField, Tooltip("提示文本")]
    private Text hintText;

    [SerializeField, Tooltip("名字")]
    private InputField nameInput;

    [SerializeField, Tooltip("邮箱")]
    private InputField emilInput;

    [SerializeField, Tooltip("密码")]
    private InputField passwordInput;

    [SerializeField, Tooltip("确认密码")]
    private InputField verifyPasswordInput;

    [SerializeField, Tooltip("登录")]
    private Button loginBut;

    [SerializeField, Tooltip("注册")]
    private Button enrollBut;


    [SerializeField, Tooltip("验证码面板")]
    private GameObject securityCodePanel;

    [SerializeField, Tooltip("验证码错误提示")]
    private Text codeHintText;

    [SerializeField, Tooltip("验证码")]
    private InputField codeInput;

    [SerializeField, Tooltip("确定")]
    private Button confirmBut;

    [SerializeField, Tooltip("取消")]
    private Button cancelBut;



    // Start is called before the first frame update
    void Start()
    {
        ReceiveManage.GetInstance().AddListeners(this);

        loginBut.onClick.AddListener(LoginBut);
        enrollBut.onClick.AddListener(EnrollBut);


        confirmBut.onClick.AddListener(ConfirmBut);
        cancelBut.onClick.AddListener(CancelBut);

        securityCodePanel.SetActive(false);

    }


    //返回登入
    private void LoginBut()
    {
        this.gameObject.SetActive(false);
        loginPanel.SetActive(true);
    }

    //注册
    private void EnrollBut()
    {
        //格式确认
        //if ()
        //{

        if(passwordInput.text == verifyPasswordInput.text)
        {
            UI_Login.UserInstance.user_account = emilInput.text;
            UI_Login.UserInstance.user_password = passwordInput.text;
            UI_Login.UserInstance.user_name = nameInput.text;
        }
        else
        {
            hintText.text = "两次输入密码不正确！";
            verifyPasswordInput.text = "";
        }  

        //}

        SendInfo.SendRequestMessage(UI_Login.UserInstance, RequestType.account_register);

        emilInput.text = "";
        passwordInput.text = "";
        nameInput.text = "";
        verifyPasswordInput.text = "";
    }

    //验证码确定
    private void ConfirmBut()
    {
        SendInfo.SendRequestMessage(UI_Login.UserInstance, codeInput.text, RequestType.email_verify);
    }

    //验证码取消
    private void CancelBut()
    {
        securityCodePanel.SetActive(false);
        this.gameObject.SetActive(true);
    }


    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="pack"></param>
    public void ReceiveMessage(DataPackage pack)
    {
        if (pack.requestType == RequestType.account_register)
        {
            if (pack.status == 1)
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    securityCodePanel.SetActive(true);
                }, null);
                Debug.Log("请求成功");

            }
            else
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    hintText.text = "登录失败,密码错误或账号已存在";
                    Invoke("ClearText", 2f);

                }, null);
                Debug.Log("注册失败该用户已存在");
            }
        }


        if (pack.requestType == RequestType.email_verify)
        {
            if (pack.status == 1)
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) => 
                {
                    codeInput.text = "";
                    securityCodePanel.SetActive(false);
                    gameObject.SetActive(false);
                    loginPanel.SetActive(true);

                }, null);
                Debug.Log("注册成功");

            }
            else if(pack.status == 2)
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    codeHintText.text = "验证码错误";
                    codeInput.text = "";
                }, null);
            }
            else
            {
                // 用Loom的方法在Unity主线程中调用
                Loom.QueueOnMainThread((param) =>
                {
                    codeHintText.text = "注册失败";

                }, null);
                Debug.Log("验证码错误");
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
