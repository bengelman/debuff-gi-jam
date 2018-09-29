using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour {
	public Button start;
	// Use this for initialization
	void Start () {
		start.onClick.AddListener (TaskOnClick);
	}
	void TaskOnClick(){
		SceneManager.LoadScene ("Overworld");
	}
	// Update is called once per frame
	void Update () {
		
	}
}
