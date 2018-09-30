using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteAnim : MonoBehaviour
{
	public GameObject AnimatedGameObject;
	public AnimSpriteSet[] AnimationSets;
	private int Cur_SpriteID;
	private float SecsPerFrame = 0.25f;
	public int loops = 0;
	public int prevID = 0;



	void Awake ()
	{
		Cur_SpriteID = 0;
		if(!AnimatedGameObject){
			AnimatedGameObject = this.gameObject;
		}
		PlayAnimation (0);
	}
	public void PlayTemp(int ID, int loops){
		if (gameObject.tag.Equals ("Shadow")) {
			GameObject.Find ("Character").GetComponent<SpriteRenderer> ().enabled = false;
		}
		StopCoroutine ("AnimateSprite");
		this.loops = loops;
		SecsPerFrame = AnimationSets[ID].speed;
		Cur_SpriteID = 0;

		StartCoroutine ("AnimateSprite", ID);

	}
	public void StopAnimation(){
		StopCoroutine ("AnimateSprite");
	}
	public void PlayAnimation (int ID)
	{
		
		SecsPerFrame = AnimationSets[ID].speed;
		StopCoroutine ("AnimateSprite");
		prevID = ID;
		//Add as much ID as necessary. Each is a different animation.
		switch (ID) {
		default:
			Cur_SpriteID = 0;
			StartCoroutine ("AnimateSprite", ID);
			break;
		}
	}

	IEnumerator AnimateSprite (int ID)
	{
		//if (!gameObject.activeSelf)
			//yield break;
		switch (ID) {
		default:
			yield return new WaitForSeconds (SecsPerFrame);
			AnimatedGameObject.GetComponent<SpriteRenderer> ().sprite
			= AnimationSets[ID].Anim_Sprites[Cur_SpriteID];
			Cur_SpriteID++;
			if (Cur_SpriteID >= AnimationSets[ID].Anim_Sprites.Length) {
				if (loops > 0) {
					loops--;
					if (loops == 0) {
						if (gameObject.GetComponent<LivingEntity> ()) {
							if (gameObject.GetComponent<LivingEntity> ().currentHealth <= 0 && AnimationSets[ID].AnimationName.Equals("Death")) {
								gameObject.SetActive (false);
							}
						}
						if (gameObject.tag.Equals ("Player") && ID == 2) {
							if (gameObject.GetComponent<LivingEntity> ().currentHealth <= 0) {
								SceneManager.LoadScene ("MainMenu");
							}
						}
						ID = prevID;
						/*
						if (gameObject.tag.Equals ("Hourglass")) {
							if (GameObject.Find ("Character").GetComponent<PlayerScript> ().breakHourglass) {
								gameObject.SetActive (false);
							}
						}*/
						if (gameObject.tag.Equals ("Shadow")) {
							GameObject.Find ("Character").GetComponent<SpriteRenderer> ().enabled = true;
							GameObject.Find ("Character").GetComponent<PlayerScript> ().lockOnShadow = false;
							GameObject.Find ("Character").GetComponent<PlayerScript> ().noTrail = true;
							ID = 0;
						}

						if (gameObject.tag.Equals ("Waypoint")) {
							gameObject.SetActive (false);
						}
					}
				}
				Cur_SpriteID = 0;
			}
			StartCoroutine ("AnimateSprite", ID);
			break;
		}
	}
}
[System.Serializable]
public class AnimSpriteSet{
	public string AnimationName;
	public Sprite[] Anim_Sprites;
	public float speed;
}
