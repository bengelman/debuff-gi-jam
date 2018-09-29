using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class face : MonoBehaviour {
	float t;
	// Use this for initialization
	void Start () {
		this.transform.Rotate(Vector3.forward * -90);
	}
	
	// Update is called once per frame
	void Update () {
		t = this.transform.eulerAngles[2];
		transform.position += new Vector3(-(float)Math.Sin(t * Math.PI/180),  (float)Math.Cos(t * Math.PI/180), 0f)*0.7f;
	//	Debug.Log(this.transform.position);
	}
    void OnTriggerEnter2D(Collider2D coll)
    {
		Debug.Log(coll);
		if (coll.gameObject.GetComponent<PlayerScript> ()) { 
			coll.GetComponent<LivingEntity> ().Hurt ();
			Destroy(gameObject);
	  	}
		
    }
}
