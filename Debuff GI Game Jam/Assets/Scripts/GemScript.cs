using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.tag.Equals ("Player")) {
			if (Input.GetButton("Interact")) {
				col.gameObject.GetComponent<SpriteAnim> ().PlayTemp (3, 1);
				GetComponent<SpriteAnim> ().PlayTemp (1, 1);
			}
		}
	}
}
