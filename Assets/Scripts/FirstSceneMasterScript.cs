using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneMasterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// セーブデータロード
		SettingDB.SetDB settingdb = SaveData.GetClass<SettingDB.SetDB>("Setting", new SettingDB.SetDB());

		// 通知更新
		LocalNoticeScript script = GetComponent<LocalNoticeScript>();
		script.ClearLocalNotification ();		// 既存通知を削除
		script.CancelAllLocalNotification();	// セットされた通知を削除

		if (settingdb.PushNoticeA.Equals(true)) {
			script.setLocalNotification (settingdb.HourA, settingdb.MinA, "noticeA");
		}
		if (settingdb.PushNoticeB.Equals(true)) {
			script.setLocalNotification (settingdb.HourB, settingdb.MinB, "noticeB");
		}

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Title");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
