using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionStay2D (Collision2D col) {
		if (col.gameObject.GetComponent<LivingEntity> ()) {
			if (col.gameObject.GetComponent<PlayerScript> ()) { // if it has a player script
				col.gameObject.GetComponent<LivingEntity> ().Hurt ();
				
				//knocks back the target
				Vector2 knockback = -(this.transform.position-col.gameObject.transform.position);
				knockback.Normalize ();
				knockback *= 3;
				col.gameObject.GetComponent<Rigidbody2D>().velocity = (knockback);
				col.gameObject.GetComponent<PlayerScript> ().Stun (0.5F);
				
				// destroy self
				Destroy(gameObject);
			}
		}
	}
}
