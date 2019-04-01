using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CModeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(DateTime.Now.ToString("yyyy/MM/dd"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(){
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameMain");
        /*
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
		string name = this.gameObject.name;
		if (name.Equals ("LngButton"))
			GameMainScript.mode = 0;
		if (name.Equals ("MathButton"))
			GameMainScript.mode = 1;
		if (name.Equals ("MscButton"))
			GameMainScript.mode = 2;
		if (name.Equals ("ArtButton"))
			GameMainScript.mode = 3;
		if (name.Equals ("ListenMusicButton")) {
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("MusicPlay");
			return;
		}
		GameMainScript.stage = 0;
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("GameMain");
        */
    }

}
