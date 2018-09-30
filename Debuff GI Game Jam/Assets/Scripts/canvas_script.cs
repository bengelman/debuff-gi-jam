using System.Collections;
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

		//Instantiate(lizard, new Vector3(0,0,0), new Quaternion(0,0,0,0));
		triangle a_triangle = Instantiate(triangle, new Vector3(-10,0,0), new Quaternion(0,0,0,0)).gameObject.GetComponent<triangle>();
		triangle2 a_triangle2 = Instantiate(triangle2, new Vector3(-10,5,0), new Quaternion(0,0,0,0)).gameObject.GetComponent<triangle2>();
		a_triangle.sibling = a_triangle2;
		a_triangle2.sibling = a_triangle;
		

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