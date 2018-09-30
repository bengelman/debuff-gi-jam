using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour {
	public int maxHealth;
	public int currentHealth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Death(){
		if (!GetComponent<PlayerScript> ()) {
			GetComponent<SpriteAnim> ().PlayTemp (GetComponent<SpriteAnim>().AnimationSets.Length - 1, 1);
		}
			
	}
	public void Hurt(){
		if (GetComponent<EnemyPopup> ()) {
			if (GetComponent<EnemyPopup> ().isUp) {
				currentHealth--;
				if (currentHealth <= 0) {
					Death ();
				}
			}
		}
		else if (GetComponent<PlayerScript> ()) {
			GetComponent<PlayerScript> ().Hurt ();
		} else {
			currentHealth--;
			if (currentHealth <= 0) {
				Death ();
			}
		}
	}
}
