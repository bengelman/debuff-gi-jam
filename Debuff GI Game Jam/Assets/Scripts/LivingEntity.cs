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
		if (currentHealth <= 0) {
			Death ();
		}
	}
	void Death(){
		if (!GetComponent<PlayerScript>())
			gameObject.SetActive (false);
	}
	public void Hurt(){
		if (GetComponent<EnemyPopup> ()) {
			if (GetComponent<EnemyPopup> ().isUp) {
				currentHealth--;
			}
		}
		else if (GetComponent<PlayerScript> ()) {
			GetComponent<PlayerScript> ().Hurt ();
		} else {
			currentHealth--;
		}
	}
}
