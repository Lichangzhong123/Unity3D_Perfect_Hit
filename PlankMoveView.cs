using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankMoveView : View
{
    public float PipeRotateSpeed;

    public float targetDistance = 3f;//目标距离
    public Transform pipeRoot;
    public Transform head;

    private int rotateType;//移动方式
    private float rotateSpeed;
    private float pipeRS;//pipe旋转的速度

    private float rotateTime;
    private float minTime;
    private float maxTime;
    private Vector3 offset;//偏移

    private Vector3 hankTarget;
    private Vector3 pipeTarget;
    private float time;
    private bool inited = false;
    // private bool onToHead = false;
    private bool gameOver = false;
    private bool iswin = false;
    private Quaternion pipeStartRot;
    private Vector3 pipeStartPos;
    private Quaternion plankStartRot;
    private GameRootView gameRoot;
    private void LoadDatas()
    {
        gameRoot = MVC.instance.GetView<GameRootView>();
        pipeRoot = gameRoot.currentPipe.transform;
        pipeStartRot = pipeRoot.rotation;
        pipeStartPos = pipeRoot.position;
        plankStartRot = transform.rotation;
        head = transform.Find("Head");
        offset = pipeRoot.position - head.position;
        //游戏失败时的目标点赋值
        hankTarget = transform.position - new Vector3(0, targetDistance, 0);
        pipeTarget = pipeRoot.position - new Vector3(0, targetDistance, 0);
        InitData();
        inited = true;
    }
    void InitData()
    {
        PipeRotateSpeed = UnityEngine.Random.Range(0f, 3f);
        gameOver = false;
        iswin = false;
        rotateTime = 0;
        rotateSpeed = UnityEngine.Random.Range(0f, 1f);
        pipeRS = UnityEngine.Random.Range(0f, 3f);
        rotateType = UnityEngine.Random.Range(1, 3);
        maxTime = 2f - rotateSpeed;
        minTime = maxTime * 0.5f;
        inited = true;
    }
    void FixedUpdate()
    {
        if (iswin)
        {
            OnWin();
        }
        else if (inited)
        {
            if (gameOver == true)
            {
                OnGameOver();
            }
            PipeAndPlankMove();
        }
    }
    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.Game_Ready, MyEvents.Player_MoveEnd ,MyEvents.PipeAndProp_GetReady };
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case MyEvents.Game_Ready:
                {
                    LoadDatas();
                }
                break;
            case MyEvents.Player_MoveEnd:
                {
                    if ((data as bool[])[0])
                    {
                        iswin = true;
                        rotateTime = 0;
                    }
                    else
                    {
                        gameOver = true;
                    }
                }
                break;
            case MyEvents.PipeAndProp_GetReady:
                {
                    inited = false;
                    LoadDatas();
                }
                break;
        }
    }

    void OnGameOver()
    {
        time += Time.fixedDeltaTime / 5;
        //y轴下降3.5个单位
        transform.position = Vector3.Lerp(transform.position, hankTarget, time);
        pipeRoot.position = Vector3.Lerp(pipeRoot.position, pipeTarget, time);
    }
    void OnWin()
    {
        rotateTime += Time.fixedDeltaTime * 2;
        pipeRoot.position = Vector3.Lerp(pipeRoot.position, pipeStartPos, rotateTime);
        pipeRoot.rotation = Quaternion.Lerp(pipeRoot.rotation, pipeStartRot, rotateTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, plankStartRot, rotateTime);
    }
    void PipeAndPlankMove()
    {
        StartCoroutine(PipeRotade());
        switch (rotateType)
        {
            case 0:
                {
                    //无变化
                    pipeRoot.Rotate(Vector3.up * pipeRS);
                }
                break;
            case 1:
                {
                    //摇摆
                    transform.Rotate(Vector3.up * rotateSpeed);
                    pipeRoot.position = offset + head.position;
                    pipeRoot.Rotate(Vector3.up * pipeRS);
                }
                break;
            case 2:
                {
                    //z轴旋转
                    transform.Rotate(Vector3.forward * rotateSpeed);
                    pipeRoot.Rotate(Vector3.forward * rotateSpeed, Space.World);
                    pipeRoot.position = offset + head.position;
                }
                break;
        }
        rotateTime += Time.fixedDeltaTime;
        if (rotateTime >= minTime)
        {
            rotateSpeed = rotateSpeed * -1;
            minTime = maxTime;
            rotateTime = 0;
        }
    }
    IEnumerator PipeRotade()
    {
        pipeRoot.Rotate(Vector3.up * PipeRotateSpeed);
        yield return null;
    }
}
