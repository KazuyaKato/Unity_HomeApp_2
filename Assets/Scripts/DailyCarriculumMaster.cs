using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyCarriculumMaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BackButtonOnClick(){
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Title");
	}

	public void GoSettingOnClick(){
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCSetting");
	}

}
