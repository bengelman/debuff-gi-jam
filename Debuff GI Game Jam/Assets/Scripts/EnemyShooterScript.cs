using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterScript : MonoBehaviour {

	GameObject Player;
	public Rigidbody2D projectile;
	public float projectileSpeed = 100.0f;
	public float bulletLifespan = 1.0f;
	public float aggroDistance = 4.0f;
	
	public int shootDelay = 60;
	private int delay = 60;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Character");
	}
		
	// Update is called once per frame
	void Update () {
		// find distance between self and player
		Vector2 playerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);
		Vector2 myPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		float distance = Vector2.Distance(playerPosition, myPosition);
		
		// if player is within certain distance, shoot projectile at player
		if (distance <= aggroDistance) {
			shootPlayer(myPosition, playerPosition);
		}
	}
	
	void shootPlayer (Vector2 start, Vector2 target) {
		// check if certain time has passed before enemy can shoot
		if (delay <= 0) {
			// create bullet
			Rigidbody2D projectileInstance;
			projectileInstance = Instantiate(projectile, gameObject.transform);
			
			var dir = target - start;
			dir = dir.normalized;
			projectileInstance.GetComponent<Rigidbody2D>().AddForce(dir * projectileSpeed);
			
			// destroy bullet after some seconds
			Destroy(projectileInstance.gameObject, bulletLifespan);
			
			// reset delay
			delay = shootDelay;
		} else {
			delay = delay - 1;
		}
	}
}
