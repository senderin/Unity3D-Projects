using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Boundary
{
	public float xMin, xMax;
}

public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	public float speed;
	public Boundary boundary;
	FeedBackController fbc;
	void Start ()
	{
		fbc=GameObject.Find("FeedBackCanvas").GetComponent<FeedBackController>();
		rb = GetComponent<Rigidbody>();
	}
	float horizontalSpeed=0.5f;
	void Update()
	{
		if (!fbc.Visible()) {
			float h = horizontalSpeed * Input.GetAxis ("Mouse X");
			transform.Translate (0, -h, 0);
			rb.position = new Vector3 (
				Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
				0.0f,
				0.0f
			);
		}
	}
	void FixedUpdate () {
		if (!fbc.Visible ()) {
			if (!GameObject.Find ("Ball").GetComponent<BallController> ().isStart) {
				GameObject.Find ("Player").GetComponent<MeshRenderer>().enabled=true;
				GameObject.Find ("Ball").GetComponent<BallController> ().canCollide = true;
			}
		}
	}



}
