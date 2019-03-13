using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresView : View
{
    public Transform score;
    public Transform highScore;
    private void Start()
    {
        if (score == null || highScore == null)
        {
            score = transform.Find("Score");
            highScore = transform.Find("HighScore");
        }
    }
    public override IList<string> GetAttentionEvents()
    {
        return new string[] { MyEvents.Added_Score, MyEvents.Show_CanvasView, MyEvents.Loaded_Data };
    }

    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case MyEvents.Loaded_Data:
                {
                    int score = MVC.instance.GetModel<LevelData>().score;
                    int highScore = MVC.instance.GetModel<LevelData>().highestScore;
                    SetNodeText(score, highScore);
                }
                break;
            case MyEvents.Added_Score:
                {
                    int score = (data as int[])[0];
                    int highScore = (data as int[])[1];
                    SetNodeText(score, highScore);
                }
                break;
            case MyEvents.Show_CanvasView:
                {
                    GameCourse course = (GameCourse)data;
                    bool active = false;
                    if (course == GameCourse.failure)
                    {
                        active = true;
                    }
                    SetHighScoreActive(active);
                }
                break;
        }
    }
    void SetNodeText(int score, int highScore)
    {
        this.score.GetComponent<Text>().text = score.ToString();
        this.highScore.Find("Score").GetComponent<Text>().text = highScore.ToString();
    }
    public void SetHighScoreActive(bool boo)
    {
        highScore.gameObject.SetActive(boo);
        highScore.GetChild(0).GetComponent<Text>().text = MVC.instance.GetModel<LevelData>().highestScore.ToString();
    }
}
