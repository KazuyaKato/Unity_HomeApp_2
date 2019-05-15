using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DCSettingMaster : MonoBehaviour {
	[SerializeField]
	public Text NumbDisp;

	public static int numbCnt = 6;
	public Toggle Gaku;	// 01学習指導要領
	public Toggle Tone;	// 02調・音階
	public Toggle Song;// 03曲名と作曲者
	public Toggle Chord;	// 04和音とコードネーム
	public Toggle time1;
	public Toggle time2;
	public SettingDB.SetDB settingdb;

	public GameObject ScrollViewHourA;
	public GameObject ScrollViewMinA;
	public GameObject ScrollViewHourB;
	public GameObject ScrollViewMinB;

	private void Awake()
	{
		#if UNITY_IOS
			GameObject obj = GameObject.FindGameObjectWithTag("iOS");
			obj.GetComponent<Canvas>().enabled = true;
		#endif
	}

	// Use this for initialization
	void Start () {

		// セーブデータロード
		settingdb = SaveData.GetClass<SettingDB.SetDB>("Setting", new SettingDB.SetDB());

		numbCnt = settingdb.Q_num;
		NumbDisp.text = numbCnt.ToString();
		Gaku.isOn = settingdb.Gaku;		
		Tone.isOn = settingdb.Tone;
		Song.isOn = settingdb.Song;
		Chord.isOn = settingdb.Chord;
		time1.isOn = settingdb.PushNoticeA;
		time2.isOn = settingdb.PushNoticeB;

		MyScroll mscript = ScrollViewHourA.GetComponent<MyScroll> ();
		mscript.thisTime = settingdb.HourA;
		setScrollHour (mscript.cnt - 1, settingdb.HourA, ScrollViewHourA);

		mscript = ScrollViewMinA.GetComponent<MyScroll> ();
		mscript.thisTime = settingdb.MinA;
		setScrollMin (mscript.cnt - 1, settingdb.MinA, ScrollViewMinA);

		mscript = ScrollViewHourB.GetComponent<MyScroll> ();
		mscript.thisTime = settingdb.HourB;
		setScrollHour (mscript.cnt - 1, settingdb.HourB, ScrollViewHourB);

		mscript = ScrollViewMinB.GetComponent<MyScroll> ();
		mscript.thisTime = settingdb.MinB;
		setScrollMin (mscript.cnt - 1, settingdb.MinB, ScrollViewMinB);

	}

	void setScrollHour(int max,int hour,GameObject obj){
		if (hour.Equals (0))
			hour = 24;	// 0の場合は24に修正
		hour--;
		float pos = 1f - (1f / 23f * (float)hour);
		float fmax = (float)max;
		obj.GetComponent<ScrollRect> ().verticalNormalizedPosition = Mathf.RoundToInt (fmax * pos) / fmax;
	}
	
	void setScrollMin(int max,int min,GameObject obj){
		float pos = 0f;
		switch (min) {
		case 0:
			pos = 1f;
			break;
		case 15:
			pos = 0.6f;
			break;
		case 30:
			pos = 0.3f;
			break;
		case 45:
			pos = 0f;
			break;
		default:
			break;
		}
		float fmax = (float)max;
		obj.GetComponent<ScrollRect> ().verticalNormalizedPosition = Mathf.RoundToInt (fmax * pos) / fmax;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void BackButtonOnClick(){
		btnsnd ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCMain");
	}

	public void LeftOnclick(){
		btnsnd ();
		if (numbCnt > 6) {
			numbCnt--;
			NumbDisp.text = numbCnt.ToString ();
		}
	}

	public void RightOnClick(){
		btnsnd ();
		if (numbCnt >= 99)
			return;
		numbCnt++;
		NumbDisp.text = numbCnt.ToString();
	}

	public void AdoptOnClick(){ // 適用ボタン押下
		btnsnd ();
		// 適用処理
		if((Gaku.isOn.Equals(false)) &&
			(Tone.isOn.Equals(false)) &&
			(Song.isOn.Equals(false)) &&
			(Chord.isOn.Equals(false)))
			return;

		settingdb.Q_num = numbCnt;
		settingdb.Gaku = Gaku.isOn;
		settingdb.Tone = Tone.isOn;
		settingdb.Song = Song.isOn;
		settingdb.Chord = Chord.isOn;
		settingdb.PushNoticeA = time1.isOn;
		settingdb.PushNoticeB = time2.isOn;


		GameObject obj;
		MyScroll script;
		LocalNoticeScript LNScript = GetComponent<LocalNoticeScript>();
		LNScript.ClearLocalNotification ();
		if (time1.isOn.Equals (true)) {
			obj = GameObject.Find ("ScrollViewHourA");
			script = obj.GetComponent<MyScroll> ();
			settingdb.HourA = script.thisTime;
			obj = GameObject.Find ("ScrollViewMinA");
			script = obj.GetComponent<MyScroll> ();
			settingdb.MinA = script.thisTime;
			LNScript.setLocalNotification (settingdb.HourA, settingdb.MinA, "noticeA"); // 通知設定
		}
		if (time2.isOn.Equals (true)) {
			obj = GameObject.Find ("ScrollViewHourB");
			script = obj.GetComponent<MyScroll> ();
			settingdb.HourB = script.thisTime;
			obj = GameObject.Find ("ScrollViewMinB");
			script = obj.GetComponent<MyScroll> ();
			settingdb.MinB = script.thisTime;
			LNScript.setLocalNotification (settingdb.HourB, settingdb.MinB, "noticeB"); // 通知設定
		}

		// 設定データを保存
		SaveData.SetClass<SettingDB.SetDB>("Setting", settingdb);
		SaveData.Save();

		obj = GameObject.Find ("AdoptedCanvas");
		obj.GetComponent<Canvas> ().enabled = true;
		StartCoroutine (AdoptEnabled (1.0f, obj));
	}

	IEnumerator AdoptEnabled(float delay,GameObject obj){ // 適用しました表示
		yield return new WaitForSeconds (delay);
		obj.GetComponent<Canvas> ().enabled = false;
	}

	void btnsnd(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
	}
}
