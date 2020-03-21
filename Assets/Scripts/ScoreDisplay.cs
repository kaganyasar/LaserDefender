using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text score = GetComponent<Text>();
        score.text = ScoreKeeper.score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
