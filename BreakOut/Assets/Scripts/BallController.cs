using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;
	private Vector3 velocity;
	public bool isStart = false;
	private static int life=3;
	Text lifeText;
	Rigidbody player;
	public bool canCollide = true;
	FeedBackController fbc;
	void Start ()
	{
		fbc=GameObject.Find("FeedBackCanvas").GetComponent<FeedBackController>();
		rb=GetComponent<Rigidbody> ();
		lifeText = GameObject.Find ("LifeText").GetComponent<UnityEngine.UI.Text> ();
		lifeText.text =""+ life.ToString ();
	}

	void Update()
	{		
		if (!fbc.Visible ()) {
			player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody>();

			if (!isStart) {	
				rb.isKinematic = false;
				player.isKinematic=false;
				rb.position = player.transform.position + new Vector3 (0, 0, 1.0f);
			}

			if (Input.GetMouseButtonDown(0)) {
				rb.AddForce (0, 0, 300f);
				isStart = true;
			}

			if (rb.position.z < 0) {
				fbc.setVisible (true);
				tmp ();
			}
		}	
	}

	public int getLife()
	{
		return life;
	}

	public void tmp()
	{
			rb.isKinematic = true;
			isStart = false;
			player.isKinematic = true;
			fbc.setState (--life);
			if (life != 0)
				StartCoroutine (fbc.GameStart ());
			lifeText.text = "" + life.ToString ();
	}
		
	void OnTriggerEnter(Collider other)
	{
		
	 if(other.name =="Player" && canCollide){
			float offsetX = other.attachedRigidbody.position.x - transform.position.x;
			float offsetZ = other.attachedRigidbody.position.z - transform.position.z;
			rb.velocity = (transform.forward) * speed;
			rb.AddForce(new Vector3 (-100.0f*offsetX, 0.0f,  -100.0f*offsetZ));
		}
	}
}
