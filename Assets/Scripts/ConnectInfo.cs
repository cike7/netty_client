using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class ConnectInfo
{
    private static ConnectInfo socketUtils;

    private Socket socket;

    public Socket GetSocket { get { return this.socket; } }

    /// <summary>
    /// 访问单例
    /// </summary>
    /// <returns></returns>
    public static ConnectInfo GetInstance()
    {
        if (socketUtils == null)
        {
            socketUtils = new ConnectInfo();
        }

        return socketUtils;
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    private ConnectInfo()
    {
        //加载ini配置文件
        //MyIni ini = new MyIni(Application.dataPath + "/Setting.ini");

        //采用TCP方式连接
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //服务器IP地址
        IPAddress address = IPAddress.Parse("127.0.0.1"/*ini.ReadIniContent("IP", "ip")*/);

        //服务器端口
        IPEndPoint endpoint = new IPEndPoint(address, int.Parse("20530"/*ini.ReadIniContent("Post", "post")*/));

        //异步连接,连接成功调用connectCallback方法
        IAsyncResult result = socket.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), socket);

        //这里做一个超时的监测，当连接超过5秒还没成功表示超时
        bool success = result.AsyncWaitHandle.WaitOne(5000, true);

        if (!success)
        {
            //超时
            CloseSocket();
            Debug.Log("连接超时");
        }
        else
        {
            //与socket建立连接成功，开启线程接受服务端数据。
            Thread thread = new Thread(new ThreadStart(ReceiveSorket));
            //后台运行
            thread.IsBackground = true;

            thread.Start();
        }
    }

    /// <summary>
    /// 连接回调
    /// </summary>
    /// <param name="asyncConnect"></param>
    private void ConnectCallback(IAsyncResult asyncConnect)
    {       
        Debug.Log("启动连接回调");
    }

    /// <summary>
    /// 接收回调
    /// </summary>
    private void ReceiveSorket()
    {
        //在这个线程中接受服务器返回的数据
        while (true)
        {
            if (!socket.Connected)
            {
                //与服务器断开连接跳出循环
                Debug.LogError("与服务器断开连接");
                if (socket != null) socket.Close();
                break;
            }
            try
            {
                //接受数据保存至bytes当中
                byte[] bytes = new byte[4096];

                int i = socket.Receive(bytes, 0, bytes.Length, SocketFlags.None);

                if (i <= 0)
                {
                    if (socket != null) socket.Close();
                    break;
                }

                //接收消息管理器
                ReceiveManage.GetInstance().ReceiveByte(bytes);       

            }
            catch (Exception e)
            {
                Debug.LogError("客户端套接字错误." + e);
                if(socket != null) socket.Close();
                break;
            }
        }
    }

    /// <summary>
    /// 关闭Socket
    /// </summary>
    public void CloseSocket()
    {
        if (socket != null && socket.Connected)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Debug.Log("关闭连接");
        }
        socket = null;
    }

    #region 读取ini信息类

    private class MyIni
    {
        public string path;//ini文件的路径

        public MyIni(string path)
        {
            if (!File.Exists(path))//判断路径是否正确
            {
                Debug.LogError("配置文件不存在或者路径错误：" + path);
                return;
            }

            this.path = path;
        }

        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string value, string path);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string deval, StringBuilder stringBuilder, int size, string path);

        //写入ini文件
        //public void WriteIniContent(string section, string key, string value)
        //{
        //    WritePrivateProfileString(section, key, value, this.path);
        //}

        //读取Ini文件
        public string ReadIniContent(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.path);
            return temp.ToString();
        }

    }

    #endregion


}

