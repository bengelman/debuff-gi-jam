using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class canvas_script : MonoBehaviour {
	System.Random rnd = new System.Random((int)System.DateTime.Now.TimeOfDay.TotalMilliseconds);
	int delay;
	public GameObject jellyfish;
	// Use this for initialization
	void Start () {
		delay = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if(delay <= 0){
			Instantiate(jellyfish,new Vector3(0, 0, 0),new Quaternion(0,0,0,0));
			delay = 100;
			//Debug.Log("new fish");
		} else{
			delay-=1;
		}
	}
}
