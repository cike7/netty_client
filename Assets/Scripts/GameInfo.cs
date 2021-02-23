using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    //网络房间号
    public int roomNumber;

    //职位 0房主，1队员
    public int post;

    // 请求类型
    public byte requestType;

    // 指令集合
    public Command command;

    // 位置
    public Position position;

    //旋转
    public Rotation rotation;

    /// <summary>
    /// 指令集合
    /// </summary>
    public class Command
    {

    }


    /// <summary>
    /// 位置
    /// </summary>
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }


    /// <summary>
    /// 旋转
    /// </summary>
    public class Rotation
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

}
