using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public PolygonCollider2D collisionDetection;
	public GameObject shadow;
	public float baseSpeed = 1.0F;
	float speedMod = 1;
	ArrayList trail = new ArrayList();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		FixedUpdate ();
		Vector2 mouse = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
		float mag = mouse.magnitude;
		if (mouse.magnitude > 1.0f) {
			mouse.Normalize ();
			mouse *= 1 + (mag / 100);
		}
		mouse *= ((Time.deltaTime * baseSpeed) * speedMod);
		//mouse.SqrMagnitude();

		//Vector2.MoveTowards (transform.position, mouse, 1.0F * Time.deltaTime);
		Vector2 newVec = mouse + (Vector2)transform.position;
		transform.position = newVec;

		if (Input.anyKeyDown) {
			Debug.Log ("Rewinding\n");
			transform.position = (Vector3)trail[trail.Count - 1];
			trail.Clear ();
		}
		if (trail.Count > 0) {
			shadow.SetActive (true);
			Vector3 position = (Vector3)trail [trail.Count - 1];
			shadow.transform.position = position;

		} else {
			shadow.SetActive (false);
		}
	}
	int updates = 0;
	void FixedUpdate () {
		updates++;
		trail.Insert (0, transform.position);
		if (trail.Count > 100) {
			trail.RemoveRange (100, trail.Count - 100);
		}
	}
}
