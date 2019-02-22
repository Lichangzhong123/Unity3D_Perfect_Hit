using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//数据模型，每个模型必须有一个名称
public abstract class Model
{
    public virtual string Name//模板类使用名字来标记
    {
        get
        {
            return this.GetType().Name;
        }
    }
}
