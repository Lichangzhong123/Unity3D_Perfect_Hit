using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float speed = 10f;
    public float angle = 45;
    public float Numb = 0.5f;
    public Vector3 endPos;
    public Vector3 startPos;
    public float startTime;
    private float distanceToTargrt;//两者的距离
    public bool isMove = false;

    // Use this for initialization
    void Awake()
    {
        startTime = Time.time;
        startPos = transform.position;
        endPos = GameObject.Find("Pipe_Root0").transform.position;
        distanceToTargrt = Vector3.Distance(startPos, endPos);
        // Invoke("UseAngle", 3f);
    }
    // Update is called once per frame
    void Update()
    {
        UseSlerp();
        // StartCoroutine(UseAngle());

    }
    void UseSlerp()
    {
        if (isMove)
        {
            Vector3 center = (startPos + endPos) * 0.5f;
            //Vector3 centorProject = Vector3.Project(center, startPos - endPos); // 中心点在两点之间的投影
            //center = Vector3.MoveTowards(center, centorProject, 1f); // 沿着投影方向移动移动距离（距离越大弧度越小）
            center -= new Vector3(0, 8, 0);

            Vector3 startCenter = startPos - center;
            Vector3 endCenter = endPos - center;
            transform.position = Vector3.Slerp(startCenter, endCenter, (Time.time - startTime) / 1.2f);
            transform.position += center;
            if ((endPos - transform.position).magnitude < 0.1f)
            {
                isMove = false;
                transform.gameObject.AddComponent<Rigidbody>();
            }
        }
        else
        {
            startTime = Time.time;
        }
    }
    IEnumerator UseAngle()
    {
        while (isMove)
        {
            Vector3 targetPos = endPos;

            transform.LookAt(targetPos);

            float angles = Mathf.Min(1, Vector3.Distance(
                transform.position, targetPos) / distanceToTargrt) * 45;
            transform.rotation = transform.rotation * Quaternion.Euler(
                Mathf.Clamp(-angles, 42, 42), 0, 0);
            float currentDist = Vector3.Distance(transform.position, endPos);
            if (currentDist < 0.5f)
            {
                isMove = false;
                transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
                yield return null;
            }
        }
    }
}
