using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListData : Model
{
    public enum Where
    {
        onRunWay,
        onRunHead
    }
    public static PlayerListData instance = new PlayerListData();

    public float moveSpeed = 10f;//在跑道移动的速度

    public float velocity = 23f;//施加的速度（在跑道斜坡时）

    public bool isTwoHead = false;//是否为两个头

    public int headIndex = 0;//当前是第几个身体为头部

    public int bodyNumb = 0;//身体个数(实时的)

    public int entranceNumb = 0;//过关成功的身体个数

    public int damageNumb = 0;//被摧毁的身体个数

    public bool isEnd = false;//是否为结束（结束判定为所有的身体都已经过RunHead）

    public Where where = Where.onRunWay;

    private bool toHead = false;
    public void Init(int numb, bool twoHead)
    {
        bodyNumb = numb;
        headIndex = 0;
        isTwoHead = twoHead;
        endBodyNumb = 0;
        damageNumb = 0;
        entranceNumb = 0;
        where = Where.onRunWay;
        isEnd = false;
        toHead = false;
        Debug.Log("PlayerListData:Init()+bodyNumb：" + bodyNumb.ToString());
    }
    public void OnSwitchLeve()
    {
        Init(bodyNumb, isTwoHead);
        MVC.instance.SendEvent(MyEvents.PlayerList_Init, bodyNumb);
    }
    public void OnBodyPass()//身体通过
    {
        if (toHead == false)
        {
            return;
        }
        entranceNumb++;
        BodyChange(0, false);
    }
    public void OnBarrier()//遇到障碍
    {
        bool invincible = false;
        int i = -1;
        if (isTwoHead)
        {
            invincible = true;
            i = 0;
        }
        BodyChange(i, true);
        Debug.Log("遇到障碍物");
        MVC.instance.SendEvent(MyEvents.Player_OnBarrier, new object[] { invincible, headIndex });
    }
    public void OnProp()//遇到道具
    {
        BodyChange(1, false);
        int i = 1;
        if (isTwoHead)
            i *= 2;
        DataCtroll.Instance.OnProp(i);//通知levelData加分
        MVC.instance.SendEvent(MyEvents.Player_OnProp);
    }
    public void OnCone()//遇到尖刺
    {
        if (toHead == false)
        {
            return;
        }
        Debug.Log("PlayerOnCone");
        damageNumb++;
        BodyChange(-1, false);
        MVC.instance.SendEvent(MyEvents.Player_OnCone);
    }
    public void BodyChange(int i, bool isOnBarrier)//身体改变
    {
        bodyNumb = bodyNumb + i;
        if (isOnBarrier && bodyNumb <= 0)
        {
            VictoryCheck();
        }
        if ((entranceNumb + damageNumb) == endBodyNumb && isEnd)
        {
            //player移动完成
            VictoryCheck();
            //Init();
        }
        string s = string.Format("bodyNumb:{0},damageNumb:{1},entranceNumb:{2}", bodyNumb, damageNumb, entranceNumb);
        Debug.Log(s);
        MVC.instance.SendEvent(MyEvents.PlayerBody_Change, bodyNumb);
    }
    private int endBodyNumb;//记录当第一个player到达runhead时的当前身体个数
    public void GoToHead()//到达跑道头部位置
    {
        toHead = true;
        where = Where.onRunHead;
        //Debug.Log(headIndex.ToString() + ":号小球到达head");
        MVC.instance.SendEvent(MyEvents.PlayerGoTo_RunHead, headIndex);
        if (endBodyNumb < bodyNumb)
        {
            endBodyNumb = bodyNumb;
            Debug.Log("GoToHead endBodyNumb:" + endBodyNumb);
        }
        headIndex++;
    }
    public void PlayerALLFly()
    {
        Debug.Log("所有小球已飞出");
        isEnd = true;
        MVC.instance.SendEvent(MyEvents.PlayerBody_ALLFly);
    }
    void VictoryCheck()//判断是否过关
    {      
        bool isWin = true;
        if (endBodyNumb <= damageNumb || bodyNumb <= 0)
        {
            isWin = false;
            DataCtroll.Instance.PlayerMoveEnd();//过关失败
        }
        else
        {
            if (bodyNumb > 5 && entranceNumb == endBodyNumb)
            {
                isTwoHead = true;
            }
            else
            {
                isTwoHead = false;
            }
        }
        Debug.Log("player移动完成：" + isWin.ToString());
        //过关成功
        //首先根据信息改变材质
        MVC.instance.SendEvent(MyEvents.Player_MoveEnd, new bool[] { isWin, isTwoHead });
    }
}
