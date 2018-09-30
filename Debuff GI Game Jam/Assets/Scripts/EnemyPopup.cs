using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopup : MonoBehaviour {
	
	public float upTime = 6.0f; // time spent up
	public Sprite upSprite;
	public float downTime = 6.0f; // time spent down
	public Sprite downSprite;
	
	public bool isUp = true;
	private float upTimeDelay;
	private float downTimeDelay;

	// Use this for initialization
	void Start () {
		upTimeDelay = upTime;
		downTimeDelay = downTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (isUp == false) {
			if (downTimeDelay <= 0) {
				// change to active
				downTimeDelay = downTime;
				isUp = true;
				
				//enable collider
				gameObject.GetComponent<PolygonCollider2D>().enabled = true;
				
				// change sprite
				GetComponent<SpriteAnim>().PlayAnimation(0);
				GetComponent<SpriteAnim> ().PlayTemp (3, 1);
				//gameObject.GetComponent<SpriteRenderer>().sprite = upSprite;
			} else {
				downTimeDelay = downTimeDelay - Time.deltaTime;
			}
		} else {
			if (upTimeDelay <= 0) {
				// change to inactive
				upTimeDelay = upTime;
				isUp = false;
				
				// disable collider
				gameObject.GetComponent<PolygonCollider2D>().enabled = false;
				
				// change sprite
				GetComponent<SpriteAnim>().PlayAnimation(2);
				GetComponent<SpriteAnim> ().PlayTemp (1, 1);
				//gameObject.GetComponent<SpriteRenderer>().sprite = downSprite;
			} else {
				upTimeDelay = upTimeDelay - Time.deltaTime;
			}
		}
	}
	
	// collision with player
	void OnCollisionStay2D(Collision2D col)
	{	
		// do nothing if inactive
		if (isUp){			
			// damage player
			if (col.gameObject.GetComponent<LivingEntity> ()) {
				if (col.gameObject.GetComponent<PlayerScript> ()) { // if it has a player script
					col.gameObject.GetComponent<LivingEntity> ().Hurt ();
					//knocks back the target
					// Debug.Log(col.gameObject.transform.position);
					// Debug.Log(this.transform.position);
					Vector2 knockback = -(this.transform.position-col.gameObject.transform.position);
					knockback.Normalize ();
					knockback *= 3;
					col.gameObject.GetComponent<Rigidbody2D>().velocity = (knockback);
					col.gameObject.GetComponent<PlayerScript> ().Stun (0.5F);
				}
			}
		}
	}	
}