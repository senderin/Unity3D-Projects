using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManagerController : MonoBehaviour { 
	protected static int highScore;
	string highScoreKey="High Score";
	Canvas MCanvas;
	Canvas HSCanvas;
	Text txt;
	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	void Start()
	{
		MCanvas = GameObject.Find ("MainCanvas").GetComponent<Canvas> ();
		HSCanvas=GameObject.Find ("HighScoreCanvas").GetComponent<Canvas> ();
		HSCanvas.enabled = false;
		txt=GameObject.Find ("HSText").GetComponent<UnityEngine.UI.Text> ();
		highScore = PlayerPrefs.GetInt ("highScoreKey", 0);
		txt.text = highScore.ToString ();
	}

	public void NewGame()
	{
		SceneManager.LoadScene ("Main");
	}

	public void HighScore()
	{
		MCanvas.enabled = false;
		HSCanvas.enabled = true;
		StartCoroutine (Show ());
	}

	public void Exit()
	{
		Application.Quit ();
	}

	public IEnumerator Show()
	{
		for (int i = 3; i >= 1; i--) {
			yield return new WaitForSeconds ((float)1.5f);
		}
		MCanvas.enabled = true;
		HSCanvas.enabled = false;
	}

	public void compare(int score)
	{
		if (score > highScore) {
			PlayerPrefs.SetInt ("highScoreKey", score);
			PlayerPrefs.Save ();
			txt.text =highScore.ToString ();
			return;
		}
	}
}

