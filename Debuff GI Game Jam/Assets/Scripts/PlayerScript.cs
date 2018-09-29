using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public PolygonCollider2D collider;
	public float baseSpeed = 1.0F;
	float speedMod = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
