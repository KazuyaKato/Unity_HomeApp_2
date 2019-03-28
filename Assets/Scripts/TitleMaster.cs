using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMaster : MonoBehaviour {

	[SerializeField]
	// Use this for initialization
	void Start () {

//		PlayerPrefs.DeleteAll ();
//		PlayerPrefs.Save ();
//		Debug.Log ("Reset PlauerPrefs");


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SolveProblemsOnClick(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("ChooseMode");
	}

	public void GoDailyCarriculmOnClick(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCMain");
	}

}
