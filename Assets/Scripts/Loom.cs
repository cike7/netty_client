using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

/// <summary>
/// 子线程加入主线程执行
/// 联网功能大多数操作都是基于子线程完成，执行结果需要主线程操作
/// </summary>
public class Loom : MonoBehaviour
{
    //是否已经初始化
    static bool isInitialized;

    //单例
    private static Loom _ins;

    /// <summary>
    /// 初始化
    /// </summary>
    public static Loom ins
    {
        get { Initialize(); return _ins; }
    }

    //提示弹窗预制体
    private GameObject alertPrefab;

    void Awake()
    {
        //赋值
        _ins = this;
        isInitialized = true;

        //加载提示预制体
        alertPrefab = Resources.Load<GameObject>("Alert");       
    }

    //初始化
    public static void Initialize()
    {
        //判断是否已经初始化
        if (!isInitialized)
        {
            //判读程序是否已运行
            if (!Application.isPlaying) return;
            //标记初始化
            isInitialized = true;
            //新建一个空物体
            var obj = new GameObject("Loom");
            //添加此组件
            _ins = obj.AddComponent<Loom>();
            //添加发送组件
            obj.AddComponent<SendInfo>();
            //切换场景不会被删除
            DontDestroyOnLoad(obj);
        }
    }

    /// <summary>
    /// 无延迟执行集合
    /// </summary>
    List<NoDelayedQueueItem> listNoDelayActions = new List<NoDelayedQueueItem>();

    /// <summary>
    /// 有延迟执行集合
    /// </summary>
    List<DelayedQueueItem> listDelayedActions = new List<DelayedQueueItem>();


    //当前执行的无延时函数链表
    List<NoDelayedQueueItem> currentActions = new List<NoDelayedQueueItem>();

    //当前执行的有延时函数链表
    List<DelayedQueueItem> currentDelayed = new List<DelayedQueueItem>();


    //单个执行单元结构体（无延迟）
    struct NoDelayedQueueItem
    {
        /// <summary>
        /// 返回object的委托
        /// </summary>
        public Action<object> action;
        //参数
        public object param;
    }


    //单个执行单元（有延迟）
    struct DelayedQueueItem
    {
        /// <summary>
        /// 返回object的委托
        /// </summary>
        public Action<object> action;
        //参数
        public object param;
        //延迟时间
        public float time;
    }


    //加入到主线程执行队列（无延迟）
    public static void QueueOnMainThread(Action<object> taction, object param)
    {
        //加入到主线程执行队列
        QueueOnMainThread(taction, param, 0f);
    }

    //加入到主线程执行队列（有延迟）
    public static void QueueOnMainThread(Action<object> action, object param, float time)
    {
        if (time != 0)
        {
            //
            lock (ins.listDelayedActions)
            {
                //添加到有延迟任务集合
                ins.listDelayedActions.Add(new DelayedQueueItem { time = Time.time + time, action = action, param = param });
            }
        }
        else
        {
            //
            lock (ins.listNoDelayActions)
            {
                //添加到无延迟任务集合
                ins.listNoDelayActions.Add(new NoDelayedQueueItem { action = action, param = param });
            }
        }
    }


    void Update()
    {
        //判断集合元素
        if (listNoDelayActions.Count > 0)
        {
            //因为队列任务都是子线程添加进来，所以给执行队列加锁防止资源访问异常
            lock (listNoDelayActions)
            {
                //清空执行函数
                currentActions.Clear();
                //将指定集合的元素添加到 List 的末尾
                currentActions.AddRange(listNoDelayActions);
                //清空执行集合
                listNoDelayActions.Clear();
            }

            //遍历执行集合
            for (int i = 0; i < currentActions.Count; i++)
            {
                //执行委托任务
                currentActions[i].action(currentActions[i].param);
            }
        }


        if (listDelayedActions.Count > 0)
        {
            lock (listDelayedActions)
            {
                currentDelayed.Clear();
                currentDelayed.AddRange(listDelayedActions.Where(d => Time.time >= d.time));

                for (int i = 0; i < currentDelayed.Count; i++)
                {
                    listDelayedActions.Remove(currentDelayed[i]);
                }
            }

            for (int i = 0; i < currentDelayed.Count; i++)
            {
                currentDelayed[i].action(currentDelayed[i].param);
            }
        }
    }

    void OnDisable()
    {
        if (_ins == this)
        {
            _ins = null;
        }
    }

    private void OnDestroy()
    {
        print("关闭程序");

        //关闭套接字连接
        ConnectInfo.GetInstance().CloseSocket();

        //清理监听器
        ReceiveManage.GetInstance().Close();

        //退出时保存聊天记录
        ChatHistoryManager.GetInstance().ExitAndSave();

    }


    /// <summary>
    /// 弹窗提示
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="msg"></param>
    public void Alert(Transform parent,string msg)
    {
        GameObject alert = Instantiate(alertPrefab, transform.position, Quaternion.identity);

        alert.GetComponent<Alert>().msg = msg;

        alert.transform.SetParent(parent);
    }


}