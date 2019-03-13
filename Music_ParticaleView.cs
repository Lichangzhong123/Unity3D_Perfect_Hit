using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_ParticaleView : View
{
    public bool isPlaye = false;
    public int particalType = 0;
    public int particalNum;
    public float forceNumb = 1f;
    public GameObject sphere;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public Transform playerList;
    public Transform particalPerent;

    private void Update()
    {
        if (isPlaye)
            SpawnPartical(particalType);
    }
    public void SpawnPartical(int type)
    {
        if (type == 0)
        {
            SpawnMeteor();
        }
        else if (type == 1)
        {
            Spawnloca();
        }
    }
    public void SpawnMeteor()
    {//生成溅射
        isPlaye = false;
        for (int i = 0; i < particalNum; i++)
        {
            GameObject newsp = Instantiate(sphere, particalPerent);
            newsp.transform.position = playerList.position;//碰撞触发点

            Vector3 force = Vector3.forward;
            force.x = Random.Range(-0.5f, 0.5f);
            force.y = Random.Range(1, 3);
            newsp.GetComponent<TrailRenderer>().enabled = true;
            newsp.transform.GetComponent<Rigidbody>().AddForce(force.normalized * forceNumb);
        }
        Invoke("RemoveParticals", 1f);
    }
    public void Spawnloca()
    {//生成消融球体
        isPlaye = false;
        for (int i = 0; i < particalNum; i++)
        {
            GameObject newsp = Instantiate(sphere, particalPerent);
            newsp.transform.position = new Vector3(0, 0, 0);//碰撞触发点
            float x = Random.Range(-1, 1);
            float y = Random.Range(-0.5f, 1f);

            float scal = Random.Range(0.6f, 1.3f);
            newsp.transform.position += particalPerent.position + new Vector3(x, y, 0);
            newsp.transform.localScale = new Vector3(scal, scal, scal);
        }
        Invoke("RemoveParticals", 1f);
    }
    void RemoveParticals()
    {
        if (particalPerent.childCount > 0)
        {
            for (int i = 0; i < particalNum - 1; i++)
            {
                Destroy(particalPerent.GetChild(i).gameObject);
            }
        }
    }
    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.Added_Score, MyEvents.Player_OnBarrier };
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case MyEvents.Added_Score:
                {
                    particalPerent.position = playerList.position;
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                    SpawnMeteor();
                }
                break;
            case MyEvents.Player_OnBarrier:
                {
                    particalPerent.position = playerList.position;
                    print("Music:Player_OnBarrier");
                    bool invincible = (bool)(data as object[])[0];
                    if (invincible == false)
                    {
                        audioSource.clip = audioClips[1];
                        audioSource.Play();
                        Spawnloca();
                    }
                    else
                    {
                        audioSource.clip = audioClips[2];
                        audioSource.Play();
                    }
                }
                break;
        }
    }
}
