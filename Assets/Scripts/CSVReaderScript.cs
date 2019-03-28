using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVReaderScript : MonoBehaviour {

	public struct Questions{
		public int No;
		public string Q;
		public string A;
		public string W1;
		public string W2;
		public string W3;
		public string D; // description 解説

		public Questions(int _No,string _Q,string _A,string _W1,string _W2,string _W3,string _D){
			No = _No;
			Q = _Q;
			A = _A;
			W1 = _W1;
			W2 = _W2;
			W3 = _W3;
			D = _D;
		}
	}

	public struct Wrong01{	// 02調と音階
		public string Chose;

		public Wrong01(string _Chose){
			Chose = _Chose;
		}
	}

	public List<Questions> arr = new List<Questions> ();
	public List<Wrong01> wrg01A = new List<Wrong01> ();
	public List<Wrong01> wrg01B = new List<Wrong01> ();

	public int GetCsv(int mode){

		arr = new List<Questions> (); // 配列の初期化

		TextAsset csv = null;
		StringReader reader;
		if(mode.Equals(0))
			csv = Resources.Load ("CSV/00MusicLead")as TextAsset;
		if(mode.Equals(1))
			csv = Resources.Load ("CSV/01CodeQues")as TextAsset;
		if(mode.Equals(2))
			csv = Resources.Load ("CSV/04MusicAuthor")as TextAsset;
		if (mode > 2)
			Debug.Log ("Error mode = " + mode);
//		if(mode.Equals(5))
//			csv = Resources.Load ("CSV/EduLeadArt")as TextAsset;
//		if(mode.Equals(6))
//			csv = Resources.Load ("CSV/EduLeadAct")as TextAsset;

		reader = new StringReader (csv.text);

		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			string[] words = line.Split (',');

			if (words [0].Equals ("")) {
				break;
			}

			string stQues1 = words [1].Replace ("/n", Environment.NewLine);
			string stAns = "";
			string stWrg1 = "";
			string stWrg2 = "";
			string stWrg3 = "";
			string stDet = "";

			if (mode.Equals (0)) {
				stAns = words [2];
				stWrg1 = words [3];
				stWrg2 = words [4];
				stWrg3 = words [5];
			} else if (mode.Equals (1)) {
				stQues1 = stQues1 + words [2];
				stAns = words [3];
				stWrg1 = words [4];	// wrong1をtype格納に使用
				stDet = words [5];
			} else if (mode.Equals (2)) {	// 曲名と作曲者
				stQues1 = words [1];	// 曲名
				stAns = words [2];		// 作曲者
				stWrg1 = words[3];		// wrong1を洋楽邦楽判定に使用
				stDet = words [4].Replace ("/n", Environment.NewLine);		// 解説
			}
			else{
				stDet = words[3] + words[4];
				for (int i = 5; i < words.Length; i++) {
					if (!((words [i] == null) ||
						(words[i] == ""))) {
						stDet += words [i] + Environment.NewLine;
					} else {
						break;
					}
				}
			}
			arr.Add(new Questions(int.Parse(words[0]),stQues1,stAns,stWrg1,stWrg2,stWrg3,stDet));
		}
		if (mode.Equals (1)) {	// 調・音階時は間違い問も取得
			// 間違いは配列の格納だけ行う。
			wrg01A = new List<Wrong01> (); // 配列の初期化
			wrg01B = new List<Wrong01>();
			GetWngAns ();
			//

		}

		return arr.Count;
	}

	void GetWngAns(){
//		TextAsset csv = null;
//		csv = Resources.Load ("CSV/02CodeAnsA")as TextAsset;
		GetWngAnsMake (0,Resources.Load ("CSV/02CodeAnsA")as TextAsset);
		GetWngAnsMake (1,Resources.Load ("CSV/03CodeAnsB")as TextAsset);
//			csv = Resources.Load ("CSV/03CodeAnsB")as TextAsset;
//		StringReader reader = new StringReader (csv.text);
//		while (reader.Peek () > -1) {
//			string line = reader.ReadLine ();
//			wrg01A.Add (new Wrong01(line));
//		}
	}

	void GetWngAnsMake(int _type,TextAsset _csv){
		StringReader reader = new StringReader (_csv.text);
		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			if (_type.Equals (0)) {
				wrg01A.Add (new Wrong01 (line));
			} else {
				wrg01B.Add (new Wrong01 (line));
			}
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
