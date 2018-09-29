using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnim : MonoBehaviour
{
	public GameObject AnimatedGameObject;
	public AnimSpriteSet[] AnimationSets;
	private int Cur_SpriteID;
	private float SecsPerFrame = 0.25f;

	void Awake ()
	{
		Cur_SpriteID = 0;
		if(!AnimatedGameObject){
			AnimatedGameObject = this.gameObject;
		}
		PlayAnimation (0);
	}

	public void PlayAnimation (int ID)
	{
		SecsPerFrame = AnimationSets[ID].speed;
		StopCoroutine ("AnimateSprite");
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
		switch (ID) {
		default:
			yield return new WaitForSeconds (SecsPerFrame);
			AnimatedGameObject.GetComponent<SpriteRenderer> ().sprite
			= AnimationSets[ID].Anim_Sprites[Cur_SpriteID];
			Cur_SpriteID++;
			if (Cur_SpriteID >= AnimationSets[ID].Anim_Sprites.Length) {
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
