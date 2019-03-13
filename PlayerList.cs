using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    public float velocity = 25f;
    public float speed = 10f;
    public bool isTwoHead = false;
    public Transform leftHead;
    public Transform rightHead;
    public Transform centreHead;
    private Transform Players;
    private Transform bodyNmb;
    private Transform addScore;
    private Transform entrance;
    private Vector3 pipeRootPos;
    public bool isStart = true;
    public bool isGaming = false;
    public bool isRunHead = false;
    public Vector3 derection;
    private int nowHead = 0;
    public int damageNumb = 0;
    private bool isEnd = false;
    private Transform targetPos;
    private Vector3 offset;
    // Use this for initialization
    void Start()
    {
        Players = GameObject.Find("Players").transform;
        bodyNmb = GameObject.Find("PlayerBodyNumb").transform;
        addScore = GameObject.Find("AddScore").transform;
        entrance = GameObject.Find("Entrance").transform;
        pipeRootPos = GameObject.Find("Pipe1_Root").transform.position;
        targetPos = GameObject.Find("TargetPos").transform;
        offset = pipeRootPos - GameObject.Find("StartPos").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal") / 4;
        Vector3 end = transform.position + new Vector3(h, 0, speed * Time.deltaTime);
        derection = (end - transform.position).normalized;
        if (isStart)
        {
            IsStart();
        }
        else if (isGaming)
        {
            transform.position = Vector3.Lerp(transform.position, end, 1f);
            Vector3 pos = transform.position;
            //Players.GetChild(0).GetComponent<Player>().lastPosition = Players.GetChild(0).position;
            if (isTwoHead)
            {
                if (transform.position.x < 1.0f)
                {
                    pos.x = 1.0f;
                }
                else if (transform.position.x > 6.0f)
                {
                    pos.x = 6.0f;
                }
                transform.position = pos;
                if (Players.GetChild(nowHead).GetComponent<Player>().isHead)
                    Players.GetChild(nowHead).position = leftHead.position;
                if (Players.GetChild(nowHead / 2).GetComponent<Player>().isHead)
                    Players.GetChild(nowHead / 2).position = rightHead.position;
            }
            else
            {
                if (transform.position.x < 0.5f)
                {
                    pos.x = 0.5f;
                }
                else if (transform.position.x > 6.5f)
                {
                    pos.x = 6.5f;
                }
                transform.position = pos;
                if (Players.GetChild(nowHead).GetComponent<Player>().isHead)
                    Players.GetChild(nowHead).position = centreHead.position;
            }
        }//player到达head位置
        else if (isRunHead == true && isEnd == false)
        {
            Transform player = Players.GetChild(nowHead);
            if (player != null)
            {
                targetPos.position = player.position + offset;
                targetPos.RotateAround(player.position, Vector3.up, 35f * h);
                player.GetComponent<Player>().endPos = targetPos.position;

                // player.GetComponent<Rigidbody>().velocity = derection.normalized * velocity;

                if ((nowHead + 1) < Players.childCount)
                {
                    Players.GetChild(nowHead + 1).GetComponent<Player>().isHead = true;
                    transform.position = Players.GetChild(nowHead + 1).transform.position;
                    isGaming = true;
                }
                isRunHead = false;
            }
            if (nowHead < Players.childCount - 1)
            {
                nowHead++;
            }
            else
            {
                isEnd = true;
                print("最后一个飞出");
            }
        }
        else if (isEnd)
        {
            int entranceNumb = entrance.GetComponent<Entrance>().bodyNmb;
            int allBody = entranceNumb + damageNumb;
            if (allBody == nowHead + 1)
            {
                string s = "bodyNumb" + nowHead.ToString();
                print(s);
                print(allBody);
                print("此局已结束");
                isEnd=false;
                Camera.main.GetComponent<CameraMove>().isend = true;
            }
        }
    }
    void IsStart()
    {
        Players.GetChild(0).GetComponent<Player>().isHead = true;

        if (isTwoHead)
        {
            Players.GetChild(Players.childCount / 2).GetComponent<Player>().isHead = true;
        }
    }
    //public void SetChildenRelation()
    //{
    //    int childCount = Players.childCount;
    //    if (isTwoHead == false)
    //    {
    //        for (int i = 0; i < childCount - 1; i++)
    //        {
    //            Players.GetChild(i + 1).GetComponent<Player>().parent = Players.GetChild(i).gameObject;
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < childCount / 2 - 1; i++)
    //        {
    //            Players.GetChild(i + 1).GetComponent<Player>().parent = Players.GetChild(i).gameObject;
    //            Players.GetChild(childCount / 2 + i + 1).GetComponent<Player>().parent = Players.GetChild(childCount / 2 + i).gameObject;
    //        }
    //    }
    //}

    #region
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "RunWayHead")
    //    {
    //        speed = 4f;
    //        isGaming = false;
    //        isRunHead = true;
    //    }
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
      
    //}
    #endregion

    //private void SpawnBody()
    //{
    //    GameObject playerBody = Instantiate(palyerPrefab, Players);
    //    SetChildenRelation();
    //}
}
