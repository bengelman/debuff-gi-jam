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
		gameObject.SetActive (false);
	}
	public void Hurt(){
		currentHealth--;
		if (GetComponent<PlayerScript> ()) {
			
		}
	}
}
