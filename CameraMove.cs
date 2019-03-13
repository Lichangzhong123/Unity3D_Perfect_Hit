using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform player;
    private Vector3 offece;
    public bool isRunWay = true;
    public bool isend = false;
    private Transform entrance;
    private GameRootView game;
    private GameObject aginBtn;
    private Transform pipeRoot;
    private bool isScendPhases = false;
    private float time;
    private Vector3 lookPos;
    // Use this for initialization
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        offece = transform.position - player.position;
        game = transform.GetComponent<GameRootView>();
        aginBtn = GameObject.Find("AginButton");
        entrance = GameObject.Find("Entrance").transform;
        pipeRoot = GameObject.Find("Pipe_Root0").transform;
        lookPos = pipeRoot.Find("EndPos").position;
    }
    void Start()
    {
        aginBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunWay)
        {
            transform.position = player.position + offece;
        }
        else if (isend == true)
        {
            if (entrance.GetComponent<Entrance>().bodyNmb > 0)
            {
                print("过关");
                game.currentGameWin = true;
            }
            else if (game.gameOver == false)
            {
                print("失败");
                game.gameOver = true;
                aginBtn.SetActive(true);
            }
            isend = false;
        }
        else if (game.currentGameWin)
        {
            if (isScendPhases == false)
            {
                time += Time.deltaTime;
                transform.LookAt(lookPos);
                Vector3 target = pipeRoot.position + pipeRoot.up * 5;
                transform.position = Vector3.Slerp(transform.position, pipeRoot.position, time / 3f);
                if ((pipeRoot.position - transform.position).magnitude < 0.1f)
                {
                    time = 0;
                    isScendPhases = true;
                    print("开始下一局游戏");
                }
            }
            //else
            //{
            //    time += Time.deltaTime;
            //    transform.position = Vector3.Lerp(transform.position, pipeRoot.position, time / 2f);
            //    if ((pipeRoot.position - transform.position).magnitude < 0.1f)
            //    {
            //        print("开始下一局游戏");
            //    }
            //}
        }
    }
}
