﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowerScript : MonoBehaviour {
	
	GameObject Player;
	public float speed = 2.0f;
	Vector2 targetPosition = new Vector2(0,0);
	float left_offset = 0f;
	float right_offset = 0f;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		System.Random rnd = new System.Random(GetInstanceID());
		left_offset = (float)rnd.NextDouble()*6-3;
		right_offset = (float)rnd.NextDouble()*6-3;
		Debug.Log(left_offset);
		Debug.Log(right_offset);
		
	}
	
	// Update is called once per frame
	void Update () {
		// find target
			targetPosition = new Vector2(
			Player.transform.position.x-this.left_offset,
			 Player.transform.position.y+this.right_offset);
		
		// move towards player
		transform.position = Vector2.MoveTowards(
			new Vector2(transform.position.x, transform.position.y)
			, targetPosition
			, speed * Time.deltaTime);
	}
	
	/*
	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") { 
			Destroy (coll.gameObject);
		}
	}*/
}