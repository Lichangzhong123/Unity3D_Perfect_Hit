using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtroll : Controller {
    public override void Execute(object data)
    {
        MonoBehaviour.print(typeof(PlayerCtroll).Name);
    }
}
