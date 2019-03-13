using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Datas :Model {
    [SerializeField]
    public int HighScore = 0;//最高分
    [SerializeField]
    public int NowProgress = 0;//当前关卡进度
    private Datas() { }
    private static Datas instance;
    public static Datas Instance()
    {
        if (instance == null)
        {
            instance = new Datas();
        }
        return instance;
    }
}
