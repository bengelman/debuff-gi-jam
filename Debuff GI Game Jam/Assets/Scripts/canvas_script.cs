﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class canvas_script : MonoBehaviour {
	System.Random rnd = new System.Random((int)System.DateTime.Now.TimeOfDay.TotalMilliseconds);
	int delay;
	public GameObject jellyfish;
	public GameObject lizard;
	public GameObject triangle;
	public GameObject triangle2;
	
	// Use this for initialization
	void Start () {
		delay = 100;

		
		Debug.Log("A");
	}
	
	// Update is called once per frame
	void Update () {
		if(delay <= 0){
			//Instantiate(jellyfish,new Vector3(0, 0, 0),new Quaternion(0,0,0,0));
			delay = 100;
		} else{
			delay-=1;
		}
	}
}