using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ROProgressView : View
{
    public Transform nowLevelNmb;
    public Transform nextLevelNmb;
    // Use this for initialization
    void Start()
    {
        if (nextLevelNmb == null || nextLevelNmb == null)
        {
            nowLevelNmb = transform.Find("Now/Numb");
            nextLevelNmb = transform.Find("Next/Numb");
        }
    }

    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.Show_ROProgressView };
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case MyEvents.Show_ROProgressView:
                {
                    int[] i = data as int[];
                    float value = (float)i[0] / 10 + (float)i[0] / 100;
                    int numb = i[1];

                    transform.GetComponent<Slider>().value = value;
                    nowLevelNmb.GetComponent<Text>().text = numb.ToString();
                    nextLevelNmb.GetComponent<Text>().text = (numb + 1).ToString();
                }
                break;
        }
    }
}
