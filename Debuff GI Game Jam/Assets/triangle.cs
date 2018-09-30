using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class triangle : MonoBehaviour {
	public triangle2 sibling;
	System.Random rnd ;
	public triangle2 other_triangle;
	int wait=5;
	public Vector2 min;
	public Vector2 max;
	// Use this for initialization
	void Start () {
		rnd =  new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
		if(this.sibling.gameObject.activeSelf== false){ 
			wait--;
			if(wait <= 0){// create a new one
			//Vector3 val =  this.transform.position + new Vector3(rnd.Next(-6, 6),rnd.Next(-6, 6),0);
				Vector3 val = new Vector2(rnd.Next((int)min.x, (int)max.x), rnd.Next((int)min.y, (int)max.y));
			sibling = Instantiate(other_triangle, val, new Quaternion(0,0,0,0)).gameObject.GetComponent<triangle2>();
			sibling.sibling = this;
			}
		} else{
			wait=5;
		}
	}
}
