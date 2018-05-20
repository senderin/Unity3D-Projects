using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour {

	Text countdownText;
	FeedBackController fbc;
	private float countdownTo=60;
	void Start()
	{
		fbc=GameObject.Find("FeedBackCanvas").GetComponent<FeedBackController>();
		countdownText = GameObject.Find("TimeText").GetComponent<Text>();
		countdownText.text = "" + (int)countdownTo;
	}


	void Update()
	{		
		if (!fbc.Visible ()) {
			countdownTo -= Time.deltaTime;
			if (countdownTo > 0) {
				countdownText.text = "" + (int)countdownTo;
			} else {
				fbc.setVisible (true);
				fbc.setState (-2);
			}
		}
	}

	public int getTime()
	{
		return (int)countdownTo;
	}
}
