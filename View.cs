using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//视图类：
//1.每个从View继承的类都必须提供一个名字
//2.都必须注册自己关心的事件
//3.都需要处理自己关心的事件
public abstract class View : MonoBehaviour
{
    //视图标识
    public virtual string Name
    {
        get
        {
            return this.GetType().Name;
        }
    }
    //关心事件列表,存储为其类名
    [HideInInspector]
    public List<string> AttentionEvents = new List<string>();//保存关心事件的列表

    //注册关心的事件
    public abstract IList<string> GetAttentionEvents();//抽象方法，派生类必须实现此方法

    //事件处理函数
    public abstract void HandleEvent(string eventName, object data);
}
