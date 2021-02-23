using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ReceiveData 
{

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="data"></param>
    void ReceiveMessage(DataPackage pack);

}
