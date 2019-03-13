using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEvents
{
    //const 字段只能在该字段的声明中初始化.readonly字段可以在声明或构造函数中初始化
    public const string Camera_MoveEND = "Camera_MoveEND";//摄像机移动完
    public const string NONE = "NONE";//空事件
    #region GameRoot控制事件
    public const string Init_Game = "InitGame";//初始化游戏
    public const string Quit_Game = "QuiGame";//离开游戏
    public const string PipeAndProp_GetReady = "Pipe_GetReady";//pipe准备完成
    #endregion
    #region Canvas控制事件(按键事件)
    public const string Start_Btn = "StartBtn"; //开始按钮
    public const string Agin_Btn = "AginBtn";//重新开始按钮
    public const string Set_Btn = "SetBtn";//设置按钮
    #endregion
    #region LevelData控制事件
    public const string Loaded_Data = "LoadedData";//加载游戏数据完成
    public const string Game_Ready = "Game_Ready";//初始化游戏完成
    public const string Start_Game = "StartGame";//开始游戏
    public const string Added_Score = "AddedScore";//分数添加完成
    public const string Show_ROProgressView = "ShowROProgressView";//关卡数据改变,显示关卡进度条
    public const string Show_CanvasView = "ShowCanvasView";//显示(更新)canvas界面
    public const string Reach_Standard = "ReachAStandard";//过一小关
    public const string Failure_Standard = "FailureStandard";//过关失败
    public const string The_Game_Victory = "TheGameVictory";//游戏最终胜利
    #endregion
    #region PlayerList控制事件
    public const string PlayerList_Init = "PlayerList_Init";//初始化playerListView
    public const string Player_GetReady = "Player_GetReady";//player准备以完成
    public const string Player_OnCone = "On_Cone";//碰到转盘尖刺
    public const string Player_OnBarrier = "On_Barrier";//碰到障碍物
    public const string Player_OnProp = "On_Prop";//player碰到道具
    public const string PlayerGoTo_RunHead = "GoTo_RunHead";//到达跑道头位置
    public const string PlayerBody_Pass = "Body_Pass";//有身体通过
    public const string PlayerBody_Damage = "Body_Damage";//有身体毁坏
    public const string PlayerBody_Change = "Body_Change";//身体个数改变
    public const string Player_MoveEnd = "Player_MoveEnd";//所有player运动结束
    public const string PlayerBody_FlyOut = "Body_FlyOut";//有身体飞出
    public const string PlayerBody_ALLFly = "Body_ALLFly";//有身体飞出
    public const string AllBody_Pass = "AllBody_Pass";//全部身体通过
    #endregion
}
