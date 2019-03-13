using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyNmbView : View
{
    public Text text;
    // Use this for initialization
    void Start()
    {
        if (text == null)
            text = transform.GetComponent<Text>();
    }
    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.PlayerBody_Change };
    }

    public override void HandleEvent(string eventName, object data)
    {
        int bodyNumb = (int)data;
        switch (eventName)
        {
            case MyEvents.PlayerBody_Change:
                {
                    text.text = bodyNumb.ToString();
                }
                break;
        }
    }
}
