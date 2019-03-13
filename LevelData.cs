using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum GameCourse//游戏进程
{
    pause,//暂停
    start,//开始
    pass,//过关
    failure,//失败
    gaming,//游戏中
    end//结束
}
[Serializable]
public class LevelData : Model
{
    public static LevelData instance = new LevelData();
    public int highestScore = 0;//最高分
    public int score = 0;//实时分数
    public int levl = 0;//当前大关卡
    public int ratio = 0;//当前关卡的进度
    public bool isOpenMusic = false;//音乐开关(false关/true开)
    public bool isOpenShake = false;//震动开关(false关/true开)
    [SerializeField]
    public GameCourse gameCourse;
    private int readyNumb = 0;//准备好的数量
    [SerializeField]
    private readonly int needNumb = 2;
    public void Init()
    {
        gameCourse = GameCourse.pause;
        ratio = 0;
        score = 0;
        readyNumb = 0;
        MVC.instance.GetModel<PlayerListData>().Init(1,false);
        MVC.instance.SendEvent(MyEvents.Loaded_Data, gameCourse);
        OnROProgressChange();
    }
    public void OnPlayerMoveEnd()
    {
        gameCourse = GameCourse.failure;
        MVC.instance.SendEvent(MyEvents.Show_CanvasView, gameCourse);
    }
    public void SwitchLevels()
    {
        //切换关卡camera移动二阶段完成
        ratio++;
        OnROProgressChange();

        if (ratio >= 9)
        {
            Debug.Log("所有关卡已通过");
            MVC.instance.SendEvent(MyEvents.The_Game_Victory);
        }
        else
        {
            Debug.Log("当前游戏关卡完成");
            DataCtroll.Instance.OnSwitchLeve();
            MVC.instance.SendEvent(MyEvents.Reach_Standard);
        }
    }
    public void Addscore(int score)
    {
        this.score += score;
        if (this.score > highestScore)
        {
            highestScore = this.score;
            Save();
        }
        MVC.instance.SendEvent(MyEvents.Added_Score, new int[] { this.score, this.highestScore, score });
    }
    public void Setmusic(bool musicSwitch)
    {
        this.isOpenMusic = musicSwitch;
    }
    public void SetShake(bool shakeSwitch)
    {
        this.isOpenShake = shakeSwitch;
    }
    public void OnROProgressChange()
    {
        MVC.instance.SendEvent(MyEvents.Show_ROProgressView, new int[] { ratio, levl });
    }
    public void StartGame()
    {
        Debug.Log("开始按键事件响应");
        gameCourse = GameCourse.start;
        MVC.instance.SendEvent(MyEvents.Show_CanvasView, gameCourse);
        MVC.instance.SendEvent(MyEvents.Start_Game);
    }

    #region 文件数据的读写
    public void Load()
    {
        string path = Application.streamingAssetsPath + "\\LevlData.json";
        if (File.Exists(path))
        {
            string hs = File.ReadAllText(path);
            // LevelData datas = JsonUtility.FromJson<LevelData>(hs);
            JsonUtility.FromJsonOverwrite(hs, this);
            // MonoBehaviour.print(datas);
        }
        Debug.Log("文件读盘");
        Init();
    }
    public void Save()
    {
        string path = Application.streamingAssetsPath + "\\LevlData.json";
        string s = JsonUtility.ToJson(this);
        File.WriteAllText(path, s);
        Debug.Log("文件写盘");
    }
    #endregion
    public void ObjReady(string name)
    {
        Debug.Log(name + "：已经准备完成");
        readyNumb++;
        if (readyNumb == needNumb)
        {
            readyNumb = 0;
            Debug.Log("Game准备完成");
            MVC.instance.SendEvent(MyEvents.Game_Ready);
        }
    }
}