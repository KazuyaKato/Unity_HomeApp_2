using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCConfirmMaster : MonoBehaviour {

	public GameObject EducateText;
	public GameObject EducateNum;

	// Use this for initialization
	void Start () {
        /*
		SettingDB.SetDB settingdb = SaveData.GetClass<SettingDB.SetDB>("Setting", new SettingDB.SetDB());

		Queue<string> queue = new Queue<string> ();
		if (settingdb.Gaku.Equals (true))
			queue.Enqueue("学習指導要領音楽");
		if (settingdb.Tone.Equals (true))
			queue.Enqueue("調・音階");
		if (settingdb.Song.Equals(true))
			queue.Enqueue("曲名と作曲者");
		if (settingdb.Chord.Equals(true))
			queue.Enqueue("和音とコードネーム");
		

		int queuework = queue.Count;

		UnityEngine.UI.Text textComponent;
		foreach (Transform child in EducateText.transform) {
			textComponent = child.GetComponent<UnityEngine.UI.Text> ();
			if (queue.Count < 1)
				textComponent.text = "";
			else
				textComponent.text = (string)queue.Dequeue();
		}

		foreach (Transform child in EducateNum.transform) {
			textComponent = child.GetComponent<UnityEngine.UI.Text> ();
			if (queuework < 1)
				textComponent.text = "";
			else {
//				textComponent.text = SettingDB.Q_num.ToString ();
				textComponent.text = settingdb.Q_num.ToString ();

				queuework--;
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BackOnClick(){
		btnsnd ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCMain");
	}

	public void StartOnClick(){
        /*
		btnsnd ();
		GameMainScript.stage = 1;
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("GameMain");
        */
	}

	void btnsnd(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
	}
}
