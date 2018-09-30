using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour {
	
	public float explosionDelay = 5.0F;
	public float explosionDuration = 1.0F;
	public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (explosionDelay <= 0) {
			// spawn explosion
			GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
			// destroy explosion after time
			Destroy(explosion, explosionDuration);
			
			// destroy self
			Destroy(gameObject);
		} else {
			// countdown until explosion
			explosionDelay -= Time.deltaTime;
		}
	}
}