using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickController : MonoBehaviour {

	Text scoreText;
	private static int score;
	FeedBackController fbc;
	void Start()
	{
		fbc=GameObject.Find("FeedBackCanvas").GetComponent<FeedBackController>();
		score = 0;
		scoreText = GameObject.Find ("ScoreText").GetComponent<UnityEngine.UI.Text> ();
		SetScoreText ();
	}
	void Update () {

	}
		
	void OnCollisionEnter(Collision other)
	{
		if(other.collider.gameObject.name=="Ball") {
			Destroy (this.gameObject);
			score =score+ 10;
			SetScoreText ();
		}
	}

	void SetScoreText()
	{
		scoreText.text = score.ToString ();
		if (score ==350) {
			fbc.setVisible (true);
			fbc.setState (-1);	
		}
	}
	public int getScore()
	{
		return score;
	}

}
