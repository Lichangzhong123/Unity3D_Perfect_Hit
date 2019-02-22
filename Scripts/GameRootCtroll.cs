using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRootCtroll : Controller {
    public override void Execute(object data)
    {
        MVC.instance.GetModel<LevelData>().ObjReady("pipe_root");
    }
}
