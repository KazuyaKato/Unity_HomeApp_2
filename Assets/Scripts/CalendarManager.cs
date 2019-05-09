using System;

using System.Linq;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using UniRx;
using System.IO;

#if UNITY_IOS
	using NotificationServices = UnityEngine.iOS.NotificationServices;
#endif


public class CalendarManager : MonoBehaviour {

	///<summaryボタンの親オブジェクト</summary>

	public GameObject calenderParent;

	/// <summary>来月へ</summary>

	public Button nextButton;

	/// <summary>先月へ</summary>

	public Button prevButton;

	/// <summary>カレンダーの日時</summary>

	public DateTime current;

	/// <summary>Buttonオブジェクト</summary>

	GameObject[] objDays = new GameObject[42];

	/// <summary>カレンダーの日付マス</summary>

	CalendarButton[] Days = new CalendarButton[42];

	public Text MonthText;

	public GameObject stmpPrefabAM;
	public GameObject stmpPrefabPM;

	public GameObject stmpCanvas;

	public List<strData> ls = new List<strData>();

	public static string RevQues = "";


	// Use this for initialization

	void Start () {
		#if UNITY_IOS
			NotificationServices.ClearLocalNotifications ();	// 受信したローカル通知を削除
		#endif

		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			if (script.BGMSource.isPlaying.Equals (false)) {
				script.PlayBGM ();
			}
		}


		ls = new List<strData>();

		current = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);



		InitCalendarComponent();

		SetCalendar();



		if(nextButton != null)

		{

			//押されたら起動

			nextButton.onClick.AsObservable()

				.Subscribe(_ =>

					{

						//一つ月を進める

						current = current.AddMonths(1);

						SetCalendar();

					});

		}

		if(prevButton != null)

		{

			prevButton.onClick.AsObservable()

				.Subscribe(_ =>

					{

						current = current.AddMonths(-1);

						SetCalendar();

					});

		}

	}



	/// <summary>コンポーネントの取得、設定</summary>

	void InitCalendarComponent()

	{

		//行

		for (int i = 0; i < calenderParent.transform.childCount; i++) {

			//子オブジェクトを保存

			objDays[i] = calenderParent.transform.GetChild(i).gameObject;

			//コンポーネントを設定、取得

			Days[i] = objDays[i].AddComponent<CalendarButton>();

			Days[i].index = i + 1;

		}

	}



	/// <summary>カレンダーに日付をセット</summary>

	void SetCalendar()

	{

		// ここでスタンプ削除
		Transform children = stmpCanvas.transform.GetComponentInChildren<Transform>();
		foreach (Transform child in children) {
			GameObject.Destroy (child.gameObject);
		}

		int day = 1;

		//今月の1日目

		var first = new DateTime(current.Year, current.Month, day);

		MonthText.text = current.Month.ToString() + "月";

		//来月

		var nextMonth = current.AddMonths(1);

		int nextMonthDay = 1;

		//先月

		var prevMonth = current.AddMonths(-1);

		//先月の場合は後ろから数える。

		int prevMonthDay = 

			DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month) - (int)first.DayOfWeek + 1;

		// スタンプ用保存データ取得
		string DataWork = PlayerPrefs.GetString("stmpdata","");

		StringReader reader = new StringReader (DataWork);

		if (ls.Count.Equals (0)) {
			while (reader.Peek () > -1) {
				string line = reader.ReadLine ();
				string[] values = line.Split (',');
				if (values [0] != "")
					ls.Add (new strData (int.Parse (values [0]), int.Parse (values [1]), int.Parse (values [2]), int.Parse (values [3])));
			}
		}

		int cnt = 0;

		foreach (var cDay in Days)
		{

			//今月の1日より前のマスには先月の日にちを入れる。
			if(cDay.index <= (int)first.DayOfWeek)
			{

				cDay.dateValue = new DateTime(prevMonth.Year, prevMonth.Month, prevMonthDay);

				// 最低値をとり、データと比較

				for (cnt = 0; cnt < ls.Count; ++cnt) {
					if (!(DateAdjust (cDay.dateValue, ls [cnt])))
						break;
				}


				while (true) {
					if (ls.Count > cnt) {
						if (stmpCheck (prevMonth.Year, prevMonth.Month, prevMonthDay, ls [cnt]).Equals (true)) {
							StmpDisp (cDay.transform.position.x, cDay.transform.position.y, ls [cnt].mode);
							cnt = cnt + 1;
						} else {
							break;
						}
					} else {
						break;
					}
				}

				prevMonthDay++;

			}

			//今月の最終日より後ろのマスには来月の日にちを入れる。

			else if(day > DateTime.DaysInMonth(current.Year, current.Month))

			{

				cDay.dateValue = new DateTime(nextMonth.Year, nextMonth.Month, nextMonthDay);

				while (true) {
					if (ls.Count > cnt) {
						if (stmpCheck (nextMonth.Year, nextMonth.Month, nextMonthDay, ls [cnt]).Equals (true)) {
							StmpDisp (cDay.transform.position.x, cDay.transform.position.y, ls [cnt].mode);
							cnt = cnt + 1;
						} else {
							break;
						}
					} else {
						break;
					}
				}

				nextMonthDay++;

			}

			//今月の日にちをマスに入れる。

			else

			{

				cDay.dateValue = new DateTime(current.Year, current.Month, day);

				// 最低値をとり、データと比較

				for (cnt = 0; cnt < ls.Count; ++cnt) {
					if (!(DateAdjust (cDay.dateValue, ls [cnt])))
						break;
				}

				while (true) {
					if (ls.Count > cnt) {
						if (stmpCheck (current.Year, current.Month, day, ls [cnt]).Equals (true)) {
							StmpDisp (cDay.transform.position.x, cDay.transform.position.y, ls [cnt].mode);
							cnt = cnt + 1;
						} else {
							break;
						}
					} else {
						break;
					}
				}

				day++;

			}

		}

	}

	// --------------------------------------------------------------------------------
	// DateAdjsut
	// 日付調整
	// --------------------------------------------------------------------------------
	bool DateAdjust(DateTime Prev,strData _ls){
		DateTime datadt = new DateTime (_ls.year, _ls.month, _ls.day, 0, 0, 0);
		if (datadt < Prev)
			return true;
		return false;
	}

	// --------------------------------------------------------------------------------
	// stmpCheck
	// スタンプチェック
	// --------------------------------------------------------------------------------
	bool stmpCheck(int _year,int _month,int _day,strData _ls){
		if ((_year != _ls.year) ||
			(_month != _ls.month) ||
			(_day != _ls.day))
			return false;
		return true;
	}

	// --------------------------------------------------------------------------------
	// StmpDisp
	// スタンプ表示
	// --------------------------------------------------------------------------------
	void StmpDisp(float _x,float _y,int _meridirem){
		Vector3 pos = new Vector3 (_x, _y, 0);
		GameObject obj = null;
		if(_meridirem.Equals(1))
			obj = Instantiate (stmpPrefabAM, pos, Quaternion.identity);
		else
			obj = Instantiate (stmpPrefabPM, pos, Quaternion.identity);
		obj.transform.SetParent(stmpCanvas.transform);
	}

	// --------------------------------------------------------------------------------
	// StartOnClick
	// スタートボタン押下
	// --------------------------------------------------------------------------------
	public void StartOnClick(){
		btnsnd ();

		// ReviewSchene追加
		RevQues = PlayerPrefs.GetString ("MscRevdata", "");

		// 0202 Add
		SettingDB.SetDB settingdb;
		// Save Data Load
		settingdb = SaveData.GetClass<SettingDB.SetDB> ("Setting", new SettingDB.SetDB ());
		System.Collections.ArrayList alex = new System.Collections.ArrayList (); // データ格納用配列
		StringReader reader = new StringReader (RevQues);
		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			alex.Add (line);	// 1データずつ格納(教科,No)
		}
		bool flg = false;
		int i;
		for (i = 0; i < alex.Count; i++) {
			string work = alex [i].ToString ();
			string[] values = work.Split (',');
			int ClassMode = int.Parse (values [0]);

			if (Common.SetSubjectCheck (ClassMode)) {
				flg = true;
				break;
			}
		}

		if(flg)
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Review");
		else
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCConfirm");

/*
		if (RevQues.Equals ("")) {
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCConfirm");
			return;
		}
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Review");
		// 追加End
*/
	}

	// --------------------------------------------------------------------------------
	// SettingOnClick
	// 設定ボタン押下
	// --------------------------------------------------------------------------------
	public void SettingOnClick(){
		btnsnd ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCSetting");
	}
	public void BackOnclick(){
		btnsnd ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Title");
	}

	public static string getDatawork(){
		return RevQues;
	}

	void btnsnd(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
	}

}