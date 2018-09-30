using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionStay2D(Collision2D col)
	{
		if (col.gameObject.GetComponent<LivingEntity> ()) {
			if (col.gameObject.GetComponent<PlayerScript> ()) { // if it has a player script
				col.gameObject.GetComponent<LivingEntity> ().Hurt ();
				//knocks back the target
				// Debug.Log(col.gameObject.transform.position);
				// Debug.Log(this.transform.position);
				col.gameObject.transform.position -= (this.transform.position-col.gameObject.transform.position);
				
				// delete self
				Destroy(gameObject);
			}
		}
	}	
}
