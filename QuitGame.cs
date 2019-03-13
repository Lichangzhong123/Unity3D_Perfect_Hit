using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : Controller {
    public override void Execute(object data)
    {
        MVC.instance.GetModel<LevelData>().Save();
    }
}
