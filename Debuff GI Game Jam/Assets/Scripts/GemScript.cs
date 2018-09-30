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
			col.gameObject.gameObject.GetComponent<SpriteAnim> ().PlayTemp (3, 1);
			Destroy(gameObject);
		}
	}
}
