using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrderingScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void LateUpdate()
	{
		GetComponent<SpriteRenderer>().sortingOrder = 10000000-(int)(transform.position.y * 100);
	}
}
