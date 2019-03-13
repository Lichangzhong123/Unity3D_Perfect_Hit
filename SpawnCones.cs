using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCones : MonoBehaviour
{
    public GameObject conePrefab;
    public int coneLenth = 18;
    private Transform my_trans;
    private Transform coneList;
    // Use this for initialization
    void Start()
    {
        my_trans = GameObject.Find("PipeRotate").transform;
        coneList = transform.Find("ConeList");
        my_trans.position = transform.position;
        my_trans.eulerAngles = transform.eulerAngles;
        SpawnCone();
    }
    void SpawnCone()
    {
        //TODO:画射线线进行射线碰撞检测后，使碰撞点绕着中心点旋转并生成锥形
        for (int i = 0; i < coneLenth; i++)
        {
            Vector3 dir = (my_trans.right * 10);
            my_trans.Rotate(new Vector3(0, 360 / coneLenth, 0));
            RaycastHit hit;
            Debug.DrawRay(transform.position, dir, Color.red);
            if (Physics.Raycast(transform.position, dir, out hit, 1 << LayerMask.NameToLayer("Wall")))
            {
                GameObject cone = Instantiate(conePrefab);
                cone.transform.position = hit.point;
                cone.transform.SetParent(coneList);
                cone.transform.LookAt(transform.position);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
    }
}
