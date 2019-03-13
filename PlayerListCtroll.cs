using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListCtroll : Controller
{
    public static PlayerListCtroll instance = new PlayerListCtroll();
    public static PlayerListCtroll Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerListCtroll();
            }
            return instance;
        }
    }
    public override void Execute(object data)
    {
        MonoBehaviour.print(typeof(PlayerListCtroll).Name);
    }
    public void OnProp()
    {
        MVC.instance.GetModel<PlayerListData>().OnProp();
    }
    public void OnCone()
    {
        MVC.instance.GetModel<PlayerListData>().OnCone();
    }

    internal void OnEntrance()
    {
        MVC.instance.GetModel<PlayerListData>().OnBodyPass();
    }

    public void OnRunWayHead()
    {
        MVC.instance.GetModel<PlayerListData>().GoToHead();
    }
    public void OnBarrier()
    {
        MVC.instance.GetModel<PlayerListData>().OnBarrier();
    }
    public void PlayerReady()
    {
        MVC.instance.GetModel<LevelData>().ObjReady("Player");
    }
    public void PlayerALLFly()
    {
        MVC.instance.GetModel<PlayerListData>().PlayerALLFly();
    }
}
