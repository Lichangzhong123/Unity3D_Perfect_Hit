using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCtroll : Controller
{
    public override void Execute(object data)//处理设置按钮中的界面操作处理
    {
        bool isSetMS = (bool)(data as object[])[0];
        bool value= (bool)(data as object[])[1];
        if (isSetMS)
        {
            MVC.instance.GetModel<LevelData>().Setmusic(value);
        }
        else
        {
            MVC.instance.GetModel<LevelData>().SetShake(value);
        }
    }
}
