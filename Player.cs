using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocity = 30f;
    public bool isHead = false;
    public GameObject parent;
    public GameObject child;
    public GameObject palyerPrefab;
    public Transform lastPos;
    public Vector3 lastPosition;
    //public float speed;
    public bool isRunWay = true;
    private Transform bodyNmb;
    private Transform addScore;
    private Transform playerList;
    public bool isRunWayHead = false;
    public Vector3 endPos;
    public Vector3 startPos;
    private float startTime;
    private int numb;
    // Use this for initialization
    void Start()
    {
        playerList = GameObject.Find("PlayerList").transform;
        bodyNmb = GameObject.Find("PlayerBodyNumb").transform;
        addScore = GameObject.Find("AddScore").transform;
        //InvokeRepeating("SetLastPos", 1, 0.2f);
    }
    void SetLastPos()
    {
        lastPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (isRunWay && isHead == true)
        {
            //float h = Input.GetAxis("Horizontal");
            //rgb.velocity = new Vector3(h * speed, 0, speed) * speed * Time.deltaTime;
        }
        else if (isRunWay && isHead == false)
        {
            Vector3 end = parent.GetComponent<Player>().lastPos.position;
            transform.position = Vector3.Lerp(transform.position, end, 0.5f);
        }
        if (isRunWayHead)
        {
            //startTime += Time.deltaTime;
            //UseSlerp();
            string s = gameObject.name + "施加力";
            print(s);

            UseVelocity();
        }
    }
    void UseSlerp()
    {
        Vector3 center = (startPos + endPos) * 0.5f;
         center -= new Vector3(0, 8, 0);

        Vector3 startCenter = startPos - center;
        Vector3 endCenter = endPos - center;

        transform.position = Vector3.Slerp(startCenter, endCenter, startTime / 1.2f);
        transform.position += center;
        if ((endPos - transform.position).magnitude < 0.5f)
        {
            isRunWayHead = false;
        }
    }
    void UseVelocity()
    {
        transform.GetComponent<Rigidbody>().velocity = playerList.GetComponent<PlayerList>().derection.normalized * velocity;
        isRunWayHead = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "RunWayHead")
        {
            Vector3 derection = playerList.GetComponent<PlayerList>().derection;
            playerList.GetComponent<PlayerList>().speed = 6f;

            Camera.main.GetComponent<CameraMove>().isRunWay = false;

            playerList.GetComponent<PlayerList>().isGaming = false;
            playerList.GetComponent<PlayerList>().isRunHead = true;
            isRunWayHead = true;
            startPos = transform.position;
        }
        else if (collision.gameObject.tag == "Cone")
        {
            DamageThis();
        }
    }
    public void DamageThis()
    {
        string s = gameObject.name + "破碎";
        print(s);
        playerList.GetComponent<PlayerList>().damageNumb++;
        Destroy(gameObject);
    }
    private void SpawnBody()
    {
        numb++;
        GameObject playerBody = Instantiate(palyerPrefab, transform.parent);
        playerBody.name = "Player" + numb.ToString();
        playerBody.GetComponent<Player>().isHead = false;

        SetRelation();
        Player perentPlayer = playerBody.GetComponent<Player>().parent.GetComponent<Player>();
        playerBody.transform.position = perentPlayer.lastPos.position;
    }
    void SetRelation()
    {
        int count = transform.parent.childCount;
        for (int i = 1; i < count; i++)
        {
            Player player = transform.parent.GetChild(i - 1).GetComponent<Player>();
            Player player2 = transform.parent.GetChild(i).GetComponent<Player>();
            if (player2.isHead == false && player2.parent == null)
            {
                player2.parent = player.gameObject;
                player.child = player2.gameObject;
            }
            else
            {

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Prop" && isHead == true)
        {
            Destroy(other.gameObject);
            SpawnBody();
            //bodyNmb.GetComponent<BodyNmbView>().BodyChange(1);
            Animation anim = addScore.GetComponent<Animation>();
            anim.Play();
        }
    }

}
