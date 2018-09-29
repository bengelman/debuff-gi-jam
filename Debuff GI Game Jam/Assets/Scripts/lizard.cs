using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lizard : MonoBehaviour {
	int delay = 0;
	public GameObject face;
	// Use this for initialization
	void Start () {
		delay = 50;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * -1);
		delay --;
		if(delay == 0){
			Instantiate(face, this.transform.position, this.transform.rotation);
			delay = 50;
			
		}
	}
}
