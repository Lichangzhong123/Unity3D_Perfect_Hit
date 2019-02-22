using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartCtroll : Controller {
    public override void Execute(object data)
    {
        MVC.instance.GetModel<LevelData>().StartGame();
    }
}
