using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMove : MonoBehaviour
{
    public float rotateSpeed;
    // Use this for initialization
    void Start()
    {
        rotateSpeed = Random.Range(0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed);
    }
}
