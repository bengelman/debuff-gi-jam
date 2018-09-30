
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser_script : MonoBehaviour {
	int delay;
	public Sprite alterante_sprite;
	// Use this for initialization
	void Start () {
		delay = 100;
	}
	
	// Update is called once per frame
	void Update () {
		delay--;
		if(delay == 0){ // if time runs out change the sprite
			SpriteRenderer spr = GetComponent<SpriteRenderer>();
			spr.sprite = alterante_sprite;
		}
		if(delay == -600){
			Destroy(gameObject);
		}
		Debug.Log(delay);
	}
    void OnTriggerStay2D(Collider2D coll)
    {
		if (coll.gameObject.GetComponent<PlayerScript> () && delay <= 0) {  // if it's a player
			coll.GetComponent<LivingEntity> ().Hurt ();
	  	}
		
    }
}