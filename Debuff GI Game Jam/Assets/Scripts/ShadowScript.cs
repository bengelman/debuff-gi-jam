using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnCollisionStay2D (Collision2D coll) {
		if (Input.GetButtonDown("Jump")) {
			if (coll.gameObject.tag == "Enemy") {
				Destroy(coll.gameObject);
			}
		}
	}
}
