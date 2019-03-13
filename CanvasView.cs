using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasView : View
{
    public float posX = 240f;
    public float offset = 2f;
    //private Transform rop;//关卡进度条
    //public Transform bodyNumb;//身体个数显示
    public Transform addScore;//加分显示
    public Transform setBtn;//设置按钮
    public Transform startBtn;//开始按钮
    public Transform aginBtn;//重新开始按钮
    public Transform setDatas;//设置界面
    public Transform GameWin;

    private bool isSetBtn = false;
    // private Vector3 startPos;
    private void Awake()
    {
        Screen.SetResolution(540, 960, false);
    }
    void Start()
    {
        if (/*bodyNumb == null ||*/ setBtn == null || startBtn == null || setDatas == null)
        {
            //rop = transform.Find("Rate Of Progress");
            //bodyNumb = transform.Find("PlayerBodyNumb");
            setBtn = transform.Find("SetButton");
            startBtn = transform.Find("StartButton");
            aginBtn = transform.Find("AginButton");
            setDatas = transform.Find("Setdatas");
            addScore = transform.Find("AddScore");
            GameWin = transform.Find("GameWin");
        }
        //startPos = bodyNumb.localPosition;
        setDatas.gameObject.SetActive(false);
    }
    public void Update()
    {
        //float h = Input.GetAxis("Horizontal") * offset;
        //bool move = MVC.instance.GetView<PlayerListView>().isGaming;
        //if (move)
        //{
        //    Vector3 pos = bodyNumb.localPosition;
        //    pos.x += h;
        //    if (pos.x < -posX)
        //    {
        //        pos.x = -posX;
        //    }else if (pos.x > posX)
        //    {
        //        pos.x = posX;
        //    }
        //    bodyNumb.localPosition = pos;
        //}
    }
    public void InitCanvas(Datas data)//初始化Canvas
    {
        setBtn.gameObject.SetActive(true);
        startBtn.gameObject.SetActive(true);
        aginBtn.gameObject.SetActive(false);
        setDatas.gameObject.SetActive(false);
    }
    public void OnSetBtnClick()//接受设置按钮点击事件
    {
        MVC.instance.GetView<ScoresView>().SetHighScoreActive(isSetBtn);
        setDatas.gameObject.SetActive(!isSetBtn);
        isSetBtn = !isSetBtn;
    }
    public Sprite openMS;//记录music打开时的图片
    public Sprite closeMS;//记录music按键关闭时的图片
    public void OnMusicBtn()//接受声音设置按钮点击事件
    {
        bool isOpen = MVC.instance.GetModel<LevelData>().isOpenMusic;
        Image musicImage = setDatas.Find("Music").GetComponent<Image>();
        if (isOpen)
        {
            musicImage.sprite = openMS;
        }
        else
        {
            musicImage.sprite = closeMS;
        }
        isOpen = !isOpen;
        MVC.instance.SendEvent(MyEvents.Set_Btn, new object[] { true, isOpen });
    }
    public Sprite openSh; //记录music按键打开时的图片
    public Sprite closeSh;//记录music按键关闭时的图片
    public void OnShakeBtn()//接受震动设置按钮点击事件
    {
        bool isOpen = MVC.instance.GetModel<LevelData>().isOpenShake;
        Image shakeImage = setDatas.Find("Shake").GetComponent<Image>();
        if (isOpen)
        {
            shakeImage.sprite = openSh;
        }
        else
        {
            shakeImage.sprite = closeSh;
        }
        isOpen = !isOpen;
        MVC.instance.SendEvent(MyEvents.Set_Btn, new object[] { false, isOpen });
    }
    public void OnStartBtnClick()//接受开始按钮点击事件
    {
        setBtn.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(false);
        setDatas.gameObject.SetActive(false);

        MVC.instance.SendEvent(MyEvents.Start_Btn);
    }
    public void OnAginClick()
    {
        print("游戏重新开始");
        SceneManager.LoadScene("Start");
    }
    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.Loaded_Data, MyEvents.Show_CanvasView, MyEvents.Added_Score/*, MyEvents.PlayerGoTo_RunHead*/ , MyEvents.Start_Game/*,MyEvents.Reach_Standard*/, MyEvents.The_Game_Victory };
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            //case MyEvents.PlayerGoTo_RunHead:
            //    {
            //        Vector3 pos = bodyNumb.localPosition;
            //        pos.x = 0;
            //        bodyNumb.localPosition = pos;
            //    }
            //    break;
            case MyEvents.Loaded_Data:
                {
                    GameWin.gameObject.SetActive(false);
                    aginBtn.gameObject.SetActive(false);
                }
                break;
            case MyEvents.Added_Score:
                {
                    print("加分动画播放");
                    int score = (data as int[])[2];
                    addScore.GetComponent<Text>().text = score.ToString();
                    addScore.GetComponent<Animation>().Play();
                }
                break;
            case MyEvents.Show_CanvasView:
                {
                    GameCourse course = (GameCourse)data;
                    switch (course)
                    {
                        case GameCourse.failure:
                            {
                                aginBtn.gameObject.SetActive(true);
                            }
                            break;
                    }
                }
                break;
            case MyEvents.Start_Game:
                {
                }
                break;
            //case MyEvents.Reach_Standard:
            //    {
            //        bodyNumb.localPosition = startPos;
            //    }
            //    break;
            case MyEvents.The_Game_Victory:
                {
                    GameWin.gameObject.SetActive(true);
                    aginBtn.gameObject.SetActive(true);
                }
                break;
        }
    }
}
