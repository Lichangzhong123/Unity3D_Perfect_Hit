using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListView : View
{
    public float moveSpeed;//在跑道移动的速度
    [HideInInspector]
    public Vector3 pipeRootPos;//入口坐标
    [HideInInspector]
    public Vector3 offset;//距离差距
    [HideInInspector]
    public Vector3 derection;//前进的方向

    public Transform Players;//player的父节点
    public Transform leftHead;//左边头
    public Transform rightHead;//右边头
    public Transform centerHead;//中间头
    public Transform targetPos;//入口目标点
    public Transform pipeRoot;
    public Transform flyPos;
    public GameObject playerPrefab;//player预制体
    private Vector3 startPos;
    private int endNumb = 0;//结束的个数
    private int maxEndNumb;
    private int spawnIndex = 0;//生成下标（用来标记双头下标）
                               //private GameObject[] headPlayers;//头部的player节点
    [HideInInspector]
    public bool isGaming = false;
   // [HideInInspector]
    public List<Transform> playerLeft;//player的列表如果isTwoHead=false时默认使用playerLeft
    //[HideInInspector]
    public List<Transform> playerRight;//player的列表如果isTwoHead=true时使用两个List

    private void Start()
    {
        startPos = transform.position;
    }
    void OnEnterScence()
    {
        GameRootView gameRoot = MVC.instance.GetView<GameRootView>();
        pipeRootPos = gameRoot.currentPipe.transform.position;
        targetPos = transform.parent.Find("TargetPos");
        offset = pipeRootPos - transform.parent.Find("FlyPos").transform.position;
    }
    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.Loaded_Data, MyEvents.Start_Game, MyEvents.Player_OnProp, MyEvents.Player_OnBarrier, MyEvents.PlayerGoTo_RunHead, MyEvents.PlayerBody_ALLFly, MyEvents.Camera_MoveEND, MyEvents.PlayerList_Init };
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case MyEvents.Loaded_Data:
                {
                    GameObject player = Instantiate(playerPrefab, Players);
                    playerLeft.Add(player.transform);
                    player.transform.position = centerHead.position;
                    player.GetComponent<PlayerView>().isHead = true;
                    SetMaxNumb();
                    PlayerListCtroll.Instance.PlayerReady();
                }
                break;
            case MyEvents.Start_Game:
                {
                    print("游戏开始");
                    isGaming = true;
                }
                break;
            case MyEvents.PlayerList_Init:
                {
                    spawnIndex = 0;
                    SetMaxNumb();
                    int bodyNumb = (int)data;
                    print("切换关卡，初始化playerbodyNumb：" + bodyNumb.ToString());
                    SetInitPlayer(bodyNumb);
                    PlayerListCtroll.Instance.PlayerReady();
                    Invoke("SetGaming", 1f);
                }
                break;
            case MyEvents.Player_OnProp:
                {
                    SpawnBody();
                }
                break;
            case MyEvents.Player_OnBarrier:
                {
                    bool invincible = (bool)(data as object[])[0];
                    int index = (int)(data as object[])[1];
                    if (invincible == false)
                        BodyFly(index, true);
                }
                break;
            case MyEvents.PlayerGoTo_RunHead:
                {
                    int headIndex = (int)data;
                    BodyFly(headIndex, false);
                }
                break;
            case MyEvents.PlayerBody_ALLFly:
                {
                    spawnIndex = 0;
                    endNumb = 0;
                    isGaming = false;
                    playerLeft.Clear();
                    playerRight.Clear();

                    Vector3 pos = transform.position;
                    pos.x = startPos.x;
                    transform.position = pos;
                }
                break;
            case MyEvents.Camera_MoveEND:
                {
                    transform.position = startPos;
                }
                break;

        }
    }
    void SetMaxNumb()
    {
        if (MVC.instance.GetModel<PlayerListData>().isTwoHead)
        {
            maxEndNumb = 2;
        }
        else
        {
            maxEndNumb = 1;
        }
    }
    void SetGaming()
    {
        isGaming = true;
    }
    void SetInitPlayer(int bodyNumb)//设置初始化playerList主要为开始下一关的初始化
    {
        int j = 0;
        PlayerListData playerData = MVC.instance.GetModel<PlayerListData>();

        GameObject player2 = Instantiate(playerPrefab, Players);
        player2.GetComponent<PlayerView>().isHead = true;
        playerLeft.Add(player2.transform);
        player2.transform.position = centerHead.position;
        j++;
        if (playerData.isTwoHead)
        {
            GameObject player1 = Instantiate(playerPrefab, Players);
            player1.GetComponent<PlayerView>().isHead = true;
            playerRight.Add(player1.transform);

            player2.transform.position = leftHead.position;
            player1.transform.position = rightHead.position;
            j++;
        }

        for (int i = j; i < bodyNumb; i++)
        {
            Invoke("SpawnBody", 0.3f * i);
        }
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal") / 8;
        Vector3 end = transform.position + new Vector3(h, 0, moveSpeed * Time.deltaTime);
        derection = (end - transform.position).normalized;
        PlayerListData listData = MVC.instance.GetModel<PlayerListData>();
        int nowHead = listData.headIndex;
        if (listData.bodyNumb <= 0)
        {
            isGaming = false;
        }
        if (isGaming)
        {
            transform.position = Vector3.Lerp(transform.position, end, 1f);
            Vector3 pos = transform.position;
            if (listData.isTwoHead)
            {
                print("playerList移动: 双头");
                if (transform.position.x < 1.0f)
                {
                    pos.x = 1.0f;
                }
                else if (transform.position.x > 6.0f)
                {
                    pos.x = 6.0f;
                }
                transform.position = pos;
                if (nowHead >= 0)
                {
                    if (nowHead < playerLeft.Count)
                        playerLeft[nowHead].position = leftHead.position;
                    if (nowHead < playerRight.Count)
                        playerRight[nowHead].position = rightHead.position;
                }
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
                if (nowHead >= 0 && nowHead < playerLeft.Count)
                    playerLeft[nowHead].position = centerHead.position;
            }
        }
    }
    void UseVelocity(Transform player)//给player施加一个速度
    {
        Vector3 vector = derection.normalized * MVC.instance.GetModel<PlayerListData>().velocity;
        if (vector.x > 3)
        {
            vector.x = 3;
        }
        else if (vector.x < -3)
        {
            vector.x = -3;
        }
        player.GetComponent<Rigidbody>().velocity = vector;
    }
    void BodyFly(int index, bool isOnBarrier)//身体飞出
    {
        if (playerRight.Count != 0)
            if (index < playerRight.Count)
            {
                if ((index + 1) < playerRight.Count)
                {
                    playerRight[index + 1].GetComponent<PlayerView>().isHead = true;
                    playerRight[index + 1].GetComponent<PlayerView>().parent = null;
                }
                else
                {
                    endNumb++;
                }
                UseVelocity(playerRight[index]);
            }
        if (playerLeft.Count != 0)
            if (index < playerLeft.Count)
            {
                if ((index + 1) < playerLeft.Count)
                {
                    playerLeft[index + 1].GetComponent<PlayerView>().isHead = true;
                    playerLeft[index + 1].GetComponent<PlayerView>().parent = null;
                    Vector3 setPos = playerLeft[index + 1].position;
                    if (MVC.instance.GetModel<PlayerListData>().isTwoHead)
                    {
                        if (index + 1 < playerRight.Count)
                            setPos += (playerRight[index + 1].position - setPos) / 2;
                    }
                    transform.position = setPos;
                }
                else
                {
                    endNumb++;
                }
                if (isOnBarrier)
                {
                    DestroyPlayerBody(index);
                }
                else
                    UseVelocity(playerLeft[index]);

            }
        if (endNumb == maxEndNumb)
        {
            PlayerListCtroll.instance.PlayerALLFly();
        }
    }
    void DestroyPlayerBody(int index)//删除player的身体
    {
        Transform player = playerLeft[index];
        playerLeft.Remove(player);
        Destroy(player.gameObject);
    }
    void SpawnBody()//生成palyer身体
    {
        GameObject playerBody = Instantiate(playerPrefab, Players);

        playerBody.GetComponent<PlayerView>().isHead = false;
        AddPlayerToList(playerBody);
    }
    void AddPlayerToList(GameObject player)//添加player到列表中
    {
        PlayerListData listData = MVC.instance.GetModel<PlayerListData>();
        Vector3 lastPos;
        if (listData.isTwoHead && (spawnIndex % 2 != 0))
        {
            player.GetComponent<PlayerView>().parent = playerRight[playerRight.Count - 1];
            lastPos = playerRight[playerRight.Count - 1].Find("LastPos").position;
            playerRight.Add(player.transform);
        }
        else
        {
            player.GetComponent<PlayerView>().parent = playerLeft[playerLeft.Count - 1];
            lastPos = playerLeft[playerLeft.Count - 1].Find("LastPos").position;
            playerLeft.Add(player.transform);
        }
        player.transform.position = lastPos;
        spawnIndex++;
    }
}
