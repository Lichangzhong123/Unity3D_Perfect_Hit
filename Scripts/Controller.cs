using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//执行命令（一段功能代码）
public abstract class Controller//抽象类，方法不需要实现也可以有实现，接口类没有实现代码和字段
{
    public abstract void Execute(object data);//命令需要传入信息
}
