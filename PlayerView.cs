using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    public bool isHead = false;//标记当前player是否为头部

    public Transform parent;//标记当前player的前一个player

    public float bodySpeed = 2f;

    public Transform particalPerent;
    public GameObject[] particalList;

    private void Start()
    {
        particalPerent = GameRootView.instance.transform.Find("ParticalPerent");
    }

    void Update()
    {
        if (isHead == false)
        {
            Vector3 end = parent.Find("LastPos").position;
            transform.position = Vector3.Lerp(transform.position, end, bodySpeed * Time.deltaTime);
        }
    }
    public void OnCone()
    {
        PlayerListCtroll.Instance.OnCone();
        SpawnPartical(1, 5, transform.position);
        Destroy(this.gameObject);
    }
    public void OnEntrance()
    {
        print("有身体通过关口");
        PlayerListCtroll.Instance.OnEntrance();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isHead)
        {
            if (collision.gameObject.tag == "RunWayHead")
            {
                PlayerListCtroll.Instance.OnRunWayHead();
            }
            else if (collision.gameObject.tag == "Cone")
            {
                OnCone();
            }
            else if (collision.gameObject.tag == "Entrance")
            {
                print("身体进入关口");
                OnEntrance();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isHead == true)
            if (other.gameObject.tag == "Prop")
            {
                Destroy(other.gameObject);
                PlayerListCtroll.Instance.OnProp();
            }
            else if (other.gameObject.tag == "Entrance")
            {
                OnEntrance();
            }
            else if (other.gameObject.tag == "Barrier")
            {
                print("Player_onBarrier");
                PlayerListCtroll.Instance.OnBarrier();
                if (MVC.instance.GetModel<PlayerListData>().isTwoHead)
                {
                    SpawnPartical(0, 1, transform.position + Vector3.up);
                    Destroy(other.gameObject);
                }
            }
    }
    void SpawnPartical(int index, int num, Vector3 spawnPos)//生成粒子特效
    {
        if (index < particalList.Length && index >= 0)//增加粒子生成个数
        {
            GameObject partical = Instantiate(particalList[index]);
            partical.transform.position = spawnPos;
            partical.transform.SetParent(particalPerent, true);
        }
    }
}
