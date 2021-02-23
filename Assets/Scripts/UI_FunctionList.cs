using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_FunctionList : MonoBehaviour
{
    [SerializeField, Header("游戏")]
    private Button gameBut;

    // Start is called before the first frame update
    void Start()
    {
        gameBut.onClick.AddListener(GameBut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameBut()
    {
        SceneManager.LoadScene("demo");
    }
}
