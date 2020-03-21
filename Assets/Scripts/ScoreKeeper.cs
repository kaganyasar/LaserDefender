using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {
    public static int score = 0;
    private Text myScoreText;

    void Start()
    {
        myScoreText = GetComponent<Text>();
        reset();
    }

	public void makeScore(int points)
    {
        score += points;
        myScoreText.text = score.ToString();
    }
    public static void reset()
    {
        score = 0;
    }
}
