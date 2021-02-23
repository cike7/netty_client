using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefab_FriendMatching : MonoBehaviour
{
    [SerializeField, Tooltip("玩家信息")]
    private Image image;

    [SerializeField, Tooltip("确定按钮")]
    private Button prepareBut;

    public GameInfo info;

    //准备状态
    public bool prepareStatus;

    // Start is called before the first frame update
    void Start()
    {
        prepareBut.onClick.AddListener(PrepareBut);
    }

    void PrepareBut()
    {
        if (prepareStatus)
        {
            prepareStatus = false;
            GetComponentInChildren<Button>().GetComponent<Image>().color = Color.yellow;
            GetComponentInChildren<Text>().text = "准备";
        }
        else
        {
            prepareStatus = true;
            GetComponentInChildren<Button>().GetComponent<Image>().color = Color.green;
            GetComponentInChildren<Text>().text = "取消";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
