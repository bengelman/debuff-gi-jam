using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyScript : MonoBehaviour {
	
	GameObject Character;
	public float speed = 2.0f;
	Vector2 targetPosition = new Vector2(0,0);
	float left_offset = 0f;
	float right_offset = 0f;
	// Use this for initialization
	void Start () {

		Character = GameObject.Find("Character");
		System.Random rnd = new System.Random(GetInstanceID()  +(int)System.DateTime.Now.TimeOfDay.TotalMilliseconds);
		left_offset = (float)rnd.NextDouble()*6-3;
		right_offset = (float)rnd.NextDouble()*6-3;
		Debug.Log(left_offset);
		Debug.Log(right_offset);
	}
	
	// Update is called once per frame
	void Update () {
		// find target
			targetPosition = new Vector2(
			Character.transform.position.x-this.left_offset,
			Character.transform.position.y+this.right_offset);
		
		// move towards Character
		transform.position = Vector2.MoveTowards(
			new Vector2(transform.position.x, transform.position.y)
			, targetPosition
			, speed * Time.deltaTime);
	}
	void LateUpdate()
	{
		GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100);
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.GetComponent<LivingEntity> ()) {
			if (col.gameObject.GetComponent<PlayerScript> ()) {
				col.gameObject.GetComponent<LivingEntity> ().Hurt ();
			}
		}
	}	
	/*
	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Character") { 
			Destroy (coll.gameObject);
		}
	}*/
}