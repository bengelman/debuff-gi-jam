using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMineScript : MonoBehaviour {
	
	public int numMines = 5;
	public float explosionDelay = 5.0F; // time in seconds until mine detonates
	public float explosionDuration = 1.0F; // duration of explosion
	
	// the boundaries where mines can spawn
	public float minX = -5.0f;
	public float maxX = 5.0F;
	public float minY = -5.0F;
	public float maxY = 5.0F;
	
	public GameObject minePrefab;
	
	// the time in seconds before boss spawns mines again 
	public float mineSpawnDelay = 6.0F;
	
	private float mineDelay;
	
	// Use this for initialization
	void Start () {
		mineDelay = mineSpawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		
		// check if it's time to spawn mines
		if (mineDelay <= 0) {
			Vector3 mineLocation;
			
			// spawn mines in area around boss
			for (int i=0; i < numMines; i++) {
				
				// create new random location in boundaries
				mineLocation = new Vector3(Random.Range(minX, maxX)
										   , Random.Range(minY, maxY)
										   , 0);
				
				// spawn mine
				GameObject mine = Instantiate(minePrefab, mineLocation, Quaternion.identity);
				MineScript m = mine.GetComponent<MineScript>();
				m.explosionDelay = explosionDelay;
				m.explosionDuration = explosionDuration;
			}
			
			// reset mine delay
			mineDelay = mineSpawnDelay;
		} else {
			// countdown until spawn mines
			mineDelay -= Time.deltaTime;
		}
	}
}