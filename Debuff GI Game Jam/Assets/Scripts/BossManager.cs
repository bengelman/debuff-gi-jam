using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {
	public int stage = 0;
	public float stageTime = 0;
	public float speed = 2;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		GetComponent<SpriteAnim> ().PlayAnimation (1);
	}
	GameObject player;

	// Update is called once per frame
	float left_offset = 0;
	float right_offset = 0;
	void Update () {
		stageTime += Time.deltaTime;
		if (stageTime > 15) {
			System.Random rnd = new System.Random(GetInstanceID()  +(int)System.DateTime.Now.TimeOfDay.TotalMilliseconds);
			stageTime = 0;
			stage++;
			stage = stage % 2;
			left_offset = (float)rnd.NextDouble()*6-3;
			right_offset = (float)rnd.NextDouble()*6-3;
		}
		GetComponent<SpriteRenderer> ().flipX = transform.position.x < player.transform.position.x;
		switch (stage){
		case 0:
			ChasePlayer ();
			GetComponent<BossMineScript> ().enabled = false;
			break;
		case 1:
			GetComponent<BossMineScript> ().enabled = true;
			break;
		case 2:
			GetComponent<BossMineScript> ().enabled = false;
			break;
		default:
			break;
		}
	}
	void ChasePlayer(){
		

		Vector2 targetPosition = new Vector2(
			player.transform.position.x-left_offset,
			player.transform.position.y+right_offset);

		// move towards player
		transform.position = Vector2.MoveTowards(
			new Vector2(transform.position.x, transform.position.y)
			, targetPosition
			, speed * Time.deltaTime);
	}
	void OnCollisionStay2D(Collision2D col)
	{
		if (col.gameObject.GetComponent<LivingEntity> ()) {
			if (col.gameObject.GetComponent<PlayerScript> ()) { // if it has a player script
				col.gameObject.GetComponent<LivingEntity> ().Hurt ();
				GetComponent<SpriteAnim> ().PlayTemp (2, 1);
				//knocks back the target
				// Debug.Log(col.gameObject.transform.position);
				// Debug.Log(this.transform.position);
				Vector2 knockback = -(this.transform.position-col.gameObject.transform.position);
				knockback.Normalize ();
				knockback *= 3;
				col.gameObject.GetComponent<Rigidbody2D>().velocity = (knockback);
				col.gameObject.GetComponent<PlayerScript> ().Stun (0.5F);
				//col.gameObject.transform.position -= (this.transform.position-col.gameObject.transform.position);

			}
		}
	}	
}
