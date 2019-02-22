using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//此类为MVC框架对外的接口，负责注册和储存MVC实例额，获取MVC实例，发送事件
public class MVC
{
    private MVC() { }
    public static readonly MVC instance = new MVC();
 
    //存储MVC
    public Dictionary<string, Model> Models = new Dictionary<string, Model>();//名字-模型
    public Dictionary<string, View> Views = new Dictionary<string, View>();//名字-视图
    public Dictionary<string, Type> CommandMap = new Dictionary<string, Type>();//事件名字-控制器(类型）

    //注册模板类
    public void RegisterModel(Model model)
    {
        Models[model.Name] = model;
    }
    //注册
    public void RegisterView(View view)
    {
        //防止重复注册
        if (Views.ContainsKey(view.Name))//判断某一个键是否存在
        {
            Views.Remove(view.Name);
        }
        //注册关心的事件
        view.AttentionEvents = view.GetAttentionEvents().ToList();

        //缓存
        Views[view.Name] = view;
    }

    public void RegisterController(string eventName, Type controllerType)
    {
        CommandMap[eventName] = controllerType;
    }
    //获取
    public T GetModel<T>()
        where T : Model
    {
        foreach (Model m in Models.Values)
        {
            if (m is T)
            {
                return (T)m;//类型强转
            }
        }
        return null;
    }
    public T GetView<T>()
        where T : View
    {
        foreach (View m in Views.Values)
        {
            if (m is T)
            {
                return (T)m;//类型强转
            }
        }
        return null;
    }

    //发送事件
    public void SendEvent(string myEventName,object data = null)
    {
        //控制器响应事件
        if (CommandMap.ContainsKey(myEventName))
        {
            Type t = CommandMap[myEventName];
            Controller c = Activator.CreateInstance(t) as Controller;//实例化一个类
            ///控制器执行
            c.Execute(data);
        }
        //视图响应事件
        foreach(View v in Views.Values)
        {
            //如果view中的List表中有eventName时返回true
            if (v.AttentionEvents.Contains(myEventName))
            {
                v.HandleEvent(myEventName, data);
            }
        }
    }
}
