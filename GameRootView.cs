using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRootView : View
{
    public static GameRootView instance;
    [HideInInspector]
    public bool gameOver = false;//游戏失败
    [HideInInspector]
    public bool currentGameWin = false;//当前关卡胜利
    public Transform pipePos;//pipe的坐标
    public Transform scene;//游戏场景列表
    public Transform fakeScene;//游戏场景下标
    public Transform runWay;
    public GameObject[] pipePrefabList;//pipe的预制体列表
    [HideInInspector]
    public GameObject currentPipe = null;//当前使用的pipe

    public GameObject[] propPrefabs;//道具的预制体列表
    public Transform propePos;//记录pipe的生成坐标
    public Material[] runMaterials;
    public Material[] runHeadMaterials;
    public Material playerMaterial;

    [HideInInspector]
    public List<Vector3> propListPos;
    private bool changed = false;
    private List<GameObject> propLists;
    public Transform CureentScene//返回当前使用的场景
    {
        get { return scene; }
    }
    private void Awake()
    {
        instance = this;
        propLists = new List<GameObject>();
        playerMaterial.color = Color.green;
    }
    void Start()
    {
        propListPos = propePos.GetComponent<SpawnPropListPos>().GetpropPosList();
        MVC.instance.RegisterView(this);
        MVC.instance.RegisterController(MyEvents.Init_Game, typeof(InitGame));
        MVC.instance.SendEvent(MyEvents.Init_Game);
    }
    private void OnDestroy()
    {
        MVC.instance.RegisterController(MyEvents.Quit_Game, typeof(QuitGame));
        MVC.instance.SendEvent(MyEvents.Quit_Game);
    }
    private void InitGame()
    {
        gameOver = false;
        currentGameWin = false;
    }
    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.Loaded_Data, MyEvents.Player_MoveEnd, MyEvents.Camera_MoveEND, MyEvents.Show_CanvasView };
    }
    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case MyEvents.Loaded_Data:
                {
                    InitGame();
                    SpawnPipe(scene);
                    SpawnProp(scene.Find("PropList"), true);
                    MVC.instance.SendEvent(MyEvents.PipeAndProp_GetReady);
                }
                break;
            case MyEvents.Player_MoveEnd:
                {
                    bool[] boo = data as bool[];
                    Color color = Color.green;
                    if (boo[1])//双头
                    {
                        color = new Color(0, 1, 1, 1);
                    }
                    SetfakeScene(boo[1]);
                    playerMaterial.color = color;
                }
                break;
            case MyEvents.Camera_MoveEND:
                {

                    int i = (int)data;
                    if (i == 0 && changed == false)
                    {
                        SetMaterial();
                        changed = true;
                    }
                    else if (i == 1)
                    {
                        changed = false;
                        InitGame();
                        SpawnPipe(scene);
                        SpawnProp(scene.Find("PropList"), false);
                        MVC.instance.SendEvent(MyEvents.PipeAndProp_GetReady);
                    }
                }
                break;
            case MyEvents.Show_CanvasView:
                {
                    GameCourse game = (GameCourse)data;
                    if (game == GameCourse.failure)
                    {
                        playerMaterial.color = Color.green;
                    }
                }
                break;
        }
    }
    void SetMaterial()
    {
        int i = Random.Range(0, runMaterials.Length);
        runWay.GetComponent<Renderer>().material = runMaterials[i];
        runWay.Find("RunWayHead").GetComponent<Renderer>().material = runHeadMaterials[i];
    }
    float ChangeColor(int numb)
    {
        return (numb / 255);
    }
    void SetfakeScene(bool isTowHead)
    {
        fakeScene.Find("FakePlayer/Centre").gameObject.SetActive(!isTowHead);
        fakeScene.Find("FakePlayer/Right").gameObject.SetActive(isTowHead);
        fakeScene.Find("FakePlayer/Left").gameObject.SetActive(isTowHead);

    }
    void SpawnPipe(Transform pipePerent)
    {
        if (pipePrefabList.Length == 0)
        {
            print("pipePrefabList:" + "为空");
            return;
        }
        if (currentPipe != null)
        {
            Destroy(currentPipe.gameObject);
        }
        int index = Random.Range(0, pipePrefabList.Length);
        currentPipe = Instantiate(pipePrefabList[index], pipePerent);
        currentPipe.transform.localPosition = pipePos.transform.position;
        currentPipe.transform.localRotation = pipePos.transform.rotation;
    }
    void SpawnProp(Transform parent, bool isOnce)
    {
        List<int> indexList = new List<int>();
        int max = 15;
        if (propLists.Count > 0)
        {
            for (int i = 0; i < propLists.Count; i++)
            {
                Destroy(propLists[i]);
            }
            propLists.Clear();
        }
        if (isOnce)
        {
            max = 7;
        }
        int numb = Random.Range(4, max);
        for (int i = 0; i < numb; i++)
        {
            int index = Random.Range(1, propListPos.Count);
            int type = Random.Range(0, propPrefabs.Length);
            bool exsit = false;
            for (int j = 0; j < indexList.Count; j++)
            {
                if (indexList[j] == index)
                {
                    exsit = true;
                    break;
                }
            }
            if (numb < 6 || isOnce)
                type = 0;
            if (exsit == false)
            {
                GameObject prop = Instantiate(propPrefabs[type]);
                prop.transform.position = propListPos[index] + propListPos[0];
                prop.transform.SetParent(parent, false);
                indexList.Add(index);
                propLists.Add(prop);
            }
        }
    }
}
