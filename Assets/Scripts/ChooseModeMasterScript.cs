using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseModeMasterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			if (script.BGMSource.isPlaying.Equals (false)) {
				script.PlayBGM ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BackButtonOnClick(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Title");
	}
}
