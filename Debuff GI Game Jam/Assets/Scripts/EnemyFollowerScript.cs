using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowerScript : MonoBehaviour {
	
	GameObject Player;
	public float speed = 2.0f;
	Vector2 playerPosition = new Vector2(0,0);

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
		
		// find player position
		playerPosition = new Vector2(
			Player.transform.position.x
			, Player.transform.position.y);
		
		// move towards player
		transform.position = Vector2.MoveTowards(
			new Vector2(transform.position.x, transform.position.y)
			, playerPosition
			, speed * Time.deltaTime);
	}
	
	/*
	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") { 
			Destroy (coll.gameObject);
		}
	}*/
}