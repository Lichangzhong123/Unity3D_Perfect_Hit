using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCtroll : Controller
{
    private static DataCtroll instance = new DataCtroll();
    public static DataCtroll Instance
    {
        get { if (instance == null) { instance = new DataCtroll(); } return instance; }
    }

    public override void Execute(object data)
    {
        //MVC.instance.GetModel<LevelData>().OnPlayerMoveEnd();
    }
    public void PlayerMoveEnd()//payer=>levelData
    {
        MVC.instance.GetModel<LevelData>().OnPlayerMoveEnd();
    }
    public void OnProp(int score)//payer=>levelData
    {
        MVC.instance.GetModel<LevelData>().Addscore(score);
    }
    public void OnSwitchLeve()//levelData=>payer
    {
        MVC.instance.GetModel<PlayerListData>().OnSwitchLeve();
    }
}
