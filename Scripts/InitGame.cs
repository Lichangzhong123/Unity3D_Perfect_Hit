using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InitGame : Controller
{
    public override void Execute(object data)
    {
        MVC.instance.RegisterModel(LevelData.instance);//注册levelData数据
        MVC.instance.RegisterModel(PlayerListData.instance);//注册

        MVC.instance.RegisterController(MyEvents.Start_Btn, typeof(GameStartCtroll));//注册开始游戏控制器
        MVC.instance.RegisterController(MyEvents.Set_Btn, typeof(CanvasCtroll));//注册设置游戏数据事件(处理音乐与震动设置)
        MVC.instance.RegisterController(MyEvents.PipeAndProp_GetReady, typeof(GameRootCtroll));//注册pipe生成完成事件
        MVC.instance.RegisterController(MyEvents.Camera_MoveEND, typeof(CameraMoveCtroll));//注册数据控制事件（playerListData与levelData之间）

        CanvasView canvasView = GameRootView.instance.transform.Find("Canvas").GetComponent<CanvasView>();
        MVC.instance.RegisterView(canvasView);//注册Canvas显示界面(主要为功能按键)

        ROProgressView rO = GameRootView.instance.transform.Find("Canvas/Rate Of Progress").GetComponent<ROProgressView>();
        MVC.instance.RegisterView(rO);//注册进度条显示界面

        ScoresView scores = GameRootView.instance.transform.Find("Canvas/Scores").GetComponent<ScoresView>();
        MVC.instance.RegisterView(scores);//注册分数显示界面

        PlayerListView playerListView = GameRootView.instance.transform.Find("Game/PlayerList").GetComponent<PlayerListView>();
        MVC.instance.RegisterView(playerListView);//注册playerList界面

        CameraMoveView cameraMoveView = GameRootView.instance.transform.Find("Main Camera").GetComponent<CameraMoveView>();
        MVC.instance.RegisterView(cameraMoveView);//注册camera界面

        BodyNmbView bodyNmbView = GameRootView.instance.transform.Find("Canvas/PlayerBodyNumb").GetComponent<BodyNmbView>();
        MVC.instance.RegisterView(bodyNmbView);

        PlankMoveView plankMoveView = GameRootView.instance.transform.Find("Game/Plank").GetComponent<PlankMoveView>();
        MVC.instance.RegisterView(plankMoveView);

        MVC.instance.GetModel<LevelData>().Load();//加载游戏数据
    }
}
