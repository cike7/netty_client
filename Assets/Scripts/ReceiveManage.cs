using System;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveManage
{
    private static ReceiveManage receive;

    /// <summary>
    /// 消息监听器
    /// </summary>
    private List<ReceiveData> listenerManage;

    //构造函数
    private ReceiveManage()
    {
        listenerManage = new List<ReceiveData>();
    }

    /// <summary>
    /// 访问实例
    /// </summary>
    /// <returns></returns>
    public static ReceiveManage GetInstance()
    {
        if(receive == null)
        {
            receive = new ReceiveManage();
        }

        return receive;
    }


    /// <summary>
    /// 接收数据
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveByte(byte[] data)
    {
        try
        {
            string json = System.Text.Encoding.UTF8.GetString(data).Replace("\u0000", "");

            Debug.Log(json);

            //给监听器发送指令
            foreach (ReceiveData listener in listenerManage)
            {
                listener.ReceiveMessage(LitJson.JsonMapper.ToObject<DataPackage>(json));
            }

        }
        catch (Exception e)
        {
            Debug.LogError("字符串解析错误："+e.Message);
        }
    }

    /// <summary>
    /// 添加监听器
    /// </summary>
    /// <param name="listener"></param>
    public void AddListeners(ReceiveData listener)
    {
        if (!listenerManage.Contains(listener))
        {
            listenerManage.Add(listener);
        }
    }

    /// <summary>
    /// 移除监听器
    /// </summary>
    /// <param name="listener"></param>
    public void RemoveIistListeners(ReceiveData listener)
    {
        if (listenerManage.Contains(listener))
        {
            listenerManage.Remove(listener);
        }
    }

    /// <summary>
    /// 清理
    /// </summary>
    public void Close()
    {
        //清理所有监听器
        listenerManage.RemoveRange(0, listenerManage.Count - 1);
        listenerManage.Clear();
        listenerManage = null;
    }

}
