using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster: MonoBehaviour 
{
    private NavMeshAgent yapayZeka;
	private Rigidbody rb;
	GameObject player;
	FeedBackController fbc;

    void Start()
    {
		fbc=GameObject.Find("FeedBackCanvas").GetComponent<FeedBackController>();
        yapayZeka = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody>();
		isVisible (false);
    }
     
    void Update ()
    {
		if (!fbc.Visible ()) {
			BrickController bc=new BrickController();
			int y = GameObject.Find ("Ball").GetComponent<BallController> ().getLife ();
			int z = GameObject.Find ("TimeText").GetComponent<Timer> ().getTime ();
			if (bc.getScore () == 50 || (y == 2 && bc.getScore () == 200) || z == 10) {
				yapayZeka.transform.position = new Vector3 (0.0f, 0.0f, 22.0f);
				isVisible (true);
			}
			if (yapayZeka.transform.position.z <= 3.0f) {
					Vector3 x = new Vector3 (yapayZeka.transform.position.x, yapayZeka.transform.position.y, -3.0f);
					yapayZeka.destination = x;
			} else
					yapayZeka.destination = player.transform.position;
		}
		if (fbc.Visible ())
			isVisible (false);
    }

	void isVisible(bool x)
	{
		rb.GetComponent<MeshRenderer> ().enabled = x;
		rb.GetComponent<SphereCollider> ().enabled = x;
		rb.isKinematic = x;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			other.attachedRigidbody.GetComponent<MeshRenderer>().enabled=false;
			int x = GameObject.Find ("Ball").GetComponent<BallController> ().getLife ();
			GameObject.Find ("Ball").GetComponent<BallController> ().canCollide = false;
			fbc.setState (--x);
		}
	}
}
