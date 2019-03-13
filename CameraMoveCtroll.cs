using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveCtroll : Controller
{
    public override void Execute(object data)
    {
        int i = (int)data;
        if (i == 1)
        {
            MVC.instance.GetModel<LevelData>().SwitchLevels();
        }
    }
}
