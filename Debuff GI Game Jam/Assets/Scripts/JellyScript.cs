using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyScript : MonoBehaviour {
	public Sprite[] anim_sprites;
	public GameObject AnimatedGameObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void LateUpdate()
	{
		GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100);
	}
}
