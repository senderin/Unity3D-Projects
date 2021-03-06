using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}

public class PlayerController: NetworkBehaviour {

	public float speed;

	bool isStart=false;

	public float increase;

	public Boundary boundary;

	public GameObject food;

	public List<Material> colors = new List<Material> ();

	GameObject[] players;

	Text score1Text;

	[SyncVar] 
	int score1=0;

	float orthographicSize = 5;
	float aspect = 1.77777f;

	float instantiationTimer=1f;

	int numOfFood=0;

	void Start () {

		Camera.main.projectionMatrix = Matrix4x4.Ortho(
			-orthographicSize * aspect, orthographicSize * aspect,
			-orthographicSize, orthographicSize,
			Camera.main.nearClipPlane, Camera.main.farClipPlane);

		score1 = 0;
		score1Text = GameObject.Find ("Score").GetComponent<Text>();
		score1Text.text = "SCORE: " + score1;
	}

	public override void OnStartClient()
	{
		GetComponent<Renderer> ().material = colors[0];
	}

	public override void OnStartLocalPlayer()
	{
		GetComponent<Renderer> ().material = colors[1];
	}

	void Update()
	{
		if (!isLocalPlayer) {
			return;
		}

		else {

			//dokunmatik ekran icin
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {

				Vector3 target = Camera.main.ScreenToWorldPoint (new Vector3(Input.GetTouch (0).position.x, Input.GetTouch(0).position.y, 0));
				target.z = 0;

				// Get movement of the finger since last frame
				Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;

				// Move object across XY plane
				transform.Translate (touchDeltaPosition.x * speed, touchDeltaPosition.y* speed, 0);
		
			}

			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x, boundary.xMin, boundary.xMax),
				Mathf.Clamp (transform.position.y, boundary.yMin, boundary.yMax),
				0.0f
			);

			CmdGenerate ();
		}

		if (GameObject.FindGameObjectWithTag ("Food")==null && isStart==true) {

			players = GameObject.FindGameObjectsWithTag ("Player");
			int x = players [0].GetComponent<PlayerController> ().score1;
			Debug.Log ("x:" + x);
			Debug.Log ("y:" + (200-x));
			Compare (score1, (200)-score1);
		} 

	}

	[Command]
	void CmdGenerate()
	{
		instantiationTimer -= Time.deltaTime;
		if(numOfFood<10 && instantiationTimer <= 0) {

			int x = Random.Range (0, Camera.main.pixelWidth);
			int y = Random.Range (0, Camera.main.pixelHeight);

			Vector3 target = Camera.main.ScreenToWorldPoint (new Vector3 (x, y, 0));
			target.z = 0;

			var build = Instantiate (food, target, Quaternion.identity) as GameObject;

			NetworkServer.Spawn (build);
			numOfFood++;
			Debug.Log (numOfFood);
			instantiationTimer = 1f;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Destroy (other.gameObject);
		SetScore ();
		isStart = true;
	}

	public void SetScore()
	{
		if (!isServer)
			return;

		RpcScore ();
	}

	[ClientRpc]
	void RpcScore()
	{
		if (isLocalPlayer)
		{
			score1 += 10;
			score1Text.text = "SCORE: " + score1;
		}
	}

	public void Compare(int score1, int score2)
	{
		Text t1, t2, t3;
		t1 = GameObject.Find ("Text1").GetComponent<Text>();
		t2 = GameObject.Find ("Text2").GetComponent<Text>();
		t3 = GameObject.Find ("Text3").GetComponent<Text>();
		if (score1 > score2) {
			t1.text = "YOU WON!";
			t2.text = "Your Score: " + score1;
			t3.text = "Match's Score: " + score2;
		} else {
			t1.text = "YOU LOST!";
			t2.text = "Your Score: " + score1;
			t3.text = "Match's Score: " + score2;
		}
	}
}



