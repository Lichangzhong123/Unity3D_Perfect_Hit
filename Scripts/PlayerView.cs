using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    public bool isHead = false;//标记当前player是否为头部

    public Transform parent;//标记当前player的前一个player

    public float bodySpeed = 2f;

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
        Destroy(this.gameObject);
    }
    public void OnEntrance()
    {
        PlayerListCtroll.Instance.OnEntrance();
        print("有身体通过关口");
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
            else if (collision.gameObject.tag == "Barrier")
            {
                PlayerListCtroll.Instance.OnBarrier();
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
        if (other.gameObject.tag == "Prop" && isHead == true)
        {
            Destroy(other.gameObject);
            PlayerListCtroll.Instance.OnProp();
        }
        else if (other.gameObject.tag == "Entrance")
        {
            OnEntrance();
        }
    }
}
