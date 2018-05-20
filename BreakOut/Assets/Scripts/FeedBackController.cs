using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FeedBackController : MonoBehaviour {

	Text txt;
	Canvas canvas;
	private static bool isVisible;
	Text state;
	Text end;
	UIManagerController uı;
	BrickController bc;
	void Start () {
		canvas = GameObject.Find ("FeedBackCanvas").GetComponent<Canvas> ();
		isVisible = true;
		StartCoroutine(GameStart ());
		 
	}

	public void setState(int x)
	{
		state = GameObject.Find ("AnyText").GetComponent<Text> ();
		end = GameObject.Find ("EndText").GetComponent<Text> ();
		BrickController bc = new BrickController ();
		end.text = "";
		state.text = "";
		if (x >= 1) {			
			state.text = "LIFE: " + (x).ToString ();
		} else if (x == 0) {
			end.fontSize = 8;
			end.text = "GAME OVER!";
			state.text="Score:" + bc.getScore ().ToString ();
			StartCoroutine(End (bc.getScore ()));
		} else if (x == -1) {
			end.fontSize = 8;
			end.text = "YOU WON!!!";
			StartCoroutine(End (bc.getScore ()));
		} else if (x == -2) {
			end.fontSize = 8;
			end.text = "TIME IS UP!";
			state.text="Score:" + bc.getScore ().ToString ();
			StartCoroutine(End (bc.getScore ()));
		} 
	}

	public IEnumerator End(int x)
	{
		for(int i=3; i>=1; i--) {
			yield return new WaitForSeconds ((float)1.5f);
		}
		uı=GameObject.Find ("UIManager").GetComponent<UIManagerController> ();
		uı.compare (x);
		SceneManager.LoadScene ("Menu");
	}

	public IEnumerator GameStart()
	{
		txt=GameObject.Find ("StartText").GetComponent<Text> ();
		for(int i=3; i>=1; i--) {
			txt.text = i.ToString ();
			yield return new WaitForSeconds ((float)1.5f);
		}
		txt.text = "START";
		yield return new WaitForSeconds ((float)1.5f);
		txt.text = "";
		isVisible = false;
	}

	public bool Visible()
	{
		return isVisible;
	}
		
	public void setVisible(bool x)
	{
		isVisible = x;
	}

	void Update () {
		if (!isVisible)
			canvas.enabled = false;
		else
			canvas.enabled=true;
	}
}
