using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class MainManager : MonoBehaviour {

	public Text Display;

	// Use this for initialization
	void Start () {
		// スタンプ用保存データ取得
		string DataWork = PlayerPrefs.GetString("stmpdata","");

		StringReader reader = new StringReader (DataWork);

//		List<strData> ls = new List<strData>();

//		if (ls.Count.Equals (0)) {
		int meridiem;
		DateTime dt = System.DateTime.Now;

		if (int.Parse (dt.ToString ("HH")) < 12)
			meridiem = 1;
		else
			meridiem = 2;


		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			string[] values = line.Split (',');


			if (DateTime.Today.Year.Equals (int.Parse (values [0])) &&
			    DateTime.Today.Month.Equals (int.Parse (values [1])) &&
			    DateTime.Today.Day.Equals (int.Parse (values [2])) &&
			    meridiem.Equals (int.Parse (values [3]))) {
				Display.text = "今日は学習済だよ";
				break;
			}
			else
				Display.text = "今日はまだ学習してないよ";
//			if (values [0] != "")
//				ls.Add (new strData (int.Parse (values [0]), int.Parse (values [1]), int.Parse (values [2]), int.Parse (values [3])));
		}
//		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
