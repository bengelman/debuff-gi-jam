using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class lizard : MonoBehaviour {
	int delay = 0;
	public GameObject face;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * -1);
		delay++;
/*		if(delay%50==0){
			Instantiate(face, this.transform.position, this.transform.rotation);
		}*/
		if(delay%100 == 0){ // laser
			float t = this.transform.eulerAngles[2];
			Vector3 forwards = new Vector3(-(float)Math.Sin(t * Math.PI/180),  (float)Math.Cos(t * Math.PI/180), 0f);
			for(int i=0; i<100; i+=7){
				//Debug.Log(forwards*i);
				Instantiate(Resources.Load<GameObject>("Prefabs/laser"),  this.transform.position - forwards*i, this.transform.rotation);
			}
		}
	}
}
