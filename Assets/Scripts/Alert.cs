using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    [HideInInspector]
    public string msg;
    // Start is called before the first frame update
    void Start()
    {
        //宽度和高度
        GetComponent<RectTransform>().sizeDelta = new Vector2(1024, 160);
        //Pox 描点坐标
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
        //设置提示内容
        GetComponentInChildren<Text>().text = msg;
        
        Destroy(gameObject,3f);
    }

}
