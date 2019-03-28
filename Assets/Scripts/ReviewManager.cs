using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class ReviewManager : MonoBehaviour {

	public Text CategoryText;
	public Text ClassText;
	public Text SentenceText;

	System.Collections.ArrayList alex = new System.Collections.ArrayList (); // データ格納用配列
	int alcnt = 0; // 配列の現在表示管理
	string workQ;// 現在の問題を保存(Review用)
	string workA;// 現在の回答を保存(Review用)

//	CSVReaderScript CSVr;

	struct strOwnCom{
		public int ClassMode;
		public int No;	// No or Chord
		public string ComText;

		public strOwnCom(int ClassMode,int No,string ComText){
			this.ClassMode = ClassMode;
			this.No = No;
			this.ComText = ComText;
		}
	}

	List<strOwnCom> ls = new List<strOwnCom>();

	// 190202 Add
	int Q_Now = 0;
	int Q_All = 0; // 全体Review数表示用
	//

	// Use this for initialization
	void Start () {

		//Debug 初期化
//		PlayerPrefs.DeleteKey ("MscComdata"); // データ削除
		//

		// データ作成(Debug)
//		string DataWork = "3,0,1" + Environment.NewLine + "3,1,0"; // 教科、No
//		string RevData = "0,0," + Environment.NewLine + "3,1,0";
//		PlayerPrefs.SetString ("MscRevdata", RevData); // データセーブ

		string DataWork = PlayerPrefs.GetString ("MscRevdata", "");
		//

//		string DataWork = CalendarManager.RevQues; // (Debug時はコメント

//		alex = new System.Collections.ArrayList ();	// 既存データ用配列

		StringReader reader = new StringReader(DataWork);
		while (reader.Peek () > -1) {
			string line = reader.ReadLine ();
			alex.Add (line);	// 1データずつ格納 (教科,No)
		}

		//CSVデータ取得
//		CSVReaderScript CSVr = GetComponent<CSVReaderScript>();

		OwnComInit ();	// 自己解説データ読み込み

		// 190202 Add
		int i;
		alcnt = 999;
		for (i = 0; i < alex.Count; i++) {
			string work = alex [i].ToString ();
			string[] values = work.Split (',');
			int ClassMode = int.Parse (values [0]);
			if (Common.SetSubjectCheck (ClassMode)) {
				Q_All++;
				if (alcnt == 999)
					alcnt = i;
			}
		}
		// Add

		QuesDisp (alcnt);
	}

	// --------------------------------------------------------------------------------
	// QuesDisp
	// Review Display
	// --------------------------------------------------------------------------------
	void QuesDisp(int alcnt){

		// 190202 Add
		// alcnt件目を表示
		Q_Now++;
		//

		// alcnt件目を表示
		DispRevNum();

		string work = alex[alcnt].ToString();
		string[] values = work.Split (',');
		int ClassMode = int.Parse (values [0]);
		CategoryText.text = Common.SetSubject (ClassMode);

		GameObject obj;

		if (ClassMode.Equals (3)) {	// 和音とコードネームであればChordCanvasをOn
			GameObject.Find("NormalCanvas").GetComponent<Canvas>().enabled = false; // 通常キャンバスを非表示
			GameObject.Find ("ChordCanvas").GetComponent<Canvas> ().enabled = true;
			int iCode = int.Parse (values [1]);
			int ivar = int.Parse (values [2]);
			workQ = Common.ChordRet(iCode);

			switch (ivar) {
			case 0:
				break;
			case 1:
				workQ += "m";
				break;
			case 2:
				workQ += "7";
				break;
			case 3:
				break;
			default:
				break;
			}
			work = "";	// 解説を取得

			// 画像を表示
			obj = GameObject.Find("ChordImage");
			Image C_Img = obj.GetComponent<Image> ();
			C_Img.sprite = Common.ChordImg(iCode,ivar);
			ClassText.text = "";

			PianoDisp (iCode, ivar);


		}else{	
			// 03以外の処理
			//CSVデータ取得
			CSVReaderScript CSVr = GetComponent<CSVReaderScript>();

			int Q_num = int.Parse (values [1]);	// 問題番号
			CSVr.GetCsv(ClassMode); // CSVデータ取得

			workA = CSVr.arr[Q_num].A;
			/*
			string strCom = GetOwnComment(ClassMode,Q_num);

			if (strCom != "")
				work += Environment.NewLine + Environment.NewLine + "〜自己解説〜" + Environment.NewLine + strCom;
*/
//			ClassText.text = workA;	// 解答（学年）を表示(画面下部枠)

			// OwnCommentを表示
			String strCom = GetOwnComment(ClassMode,Q_num);

			// 解説登録にも表示（変更用）
			obj = GameObject.Find ("InputField");
			InputField textComponent = obj.GetComponent<InputField> ();
			textComponent.text = strCom;

			CommentDisp (workA, strCom);

			workQ = CSVr.arr [Q_num].Q;
		}
		SentenceDisp(workQ,"");	// 問題と解答、解説を表示
	}

	// --------------------------------------------------------------------------------
	// PianoDisp
	// ピアノ鍵盤を表示
	// --------------------------------------------------------------------------------
	void PianoDisp(int _icode,int _ivar){
		Sprite SpPiano = null;
		if (_icode.Equals (0)) {
			if((_ivar.Equals(0)) || (_ivar.Equals(3)))
				SpPiano = Resources.Load<Sprite> ("Code/C/Image/C13_C");
			if(_ivar.Equals(1))
				SpPiano = Resources.Load<Sprite> ("Code/C/Image/C14_Cm");
			if(_ivar.Equals(2))
				SpPiano = Resources.Load<Sprite> ("Code/C/Image/C15_C7");
		}
		if (_icode.Equals (1)) {
			if((_ivar.Equals(0)) || (_ivar.Equals(3)))
				SpPiano = Resources.Load<Sprite> ("Code/D/Image/D13_D");
			if(_ivar.Equals(1))
				SpPiano = Resources.Load<Sprite> ("Code/D/Image/D14_Dm");
			if(_ivar.Equals(2))
				SpPiano = Resources.Load<Sprite> ("Code/D/Image/D15_D7");
		}
		if (_icode.Equals (2)) {
			if((_ivar.Equals(0)) || (_ivar.Equals(3)))
				SpPiano = Resources.Load<Sprite> ("Code/E/Image/E13_E");
			if(_ivar.Equals(1))
				SpPiano = Resources.Load<Sprite> ("Code/E/Image/E14_Em");
			if(_ivar.Equals(2))
				SpPiano = Resources.Load<Sprite> ("Code/E/Image/E15_E7");
		}
		if (_icode.Equals (3)) {
			if((_ivar.Equals(0)) || (_ivar.Equals(3)))
				SpPiano = Resources.Load<Sprite> ("Code/F/Image/F13_F");
			if(_ivar.Equals(1))
				SpPiano = Resources.Load<Sprite> ("Code/F/Image/F14_Fm");
			if(_ivar.Equals(2))
				SpPiano = Resources.Load<Sprite> ("Code/F/Image/F15_F7");
		}
		if (_icode.Equals (4)) {
			if((_ivar.Equals(0)) || (_ivar.Equals(3)))
				SpPiano = Resources.Load<Sprite> ("Code/G/Image/G13_G");
			if(_ivar.Equals(1))
				SpPiano = Resources.Load<Sprite> ("Code/G/Image/G14_Gm");
			if(_ivar.Equals(2))
				SpPiano = Resources.Load<Sprite> ("Code/G/Image/G15_G7");
		}
		if (_icode.Equals (5)) {
			if((_ivar.Equals(0)) || (_ivar.Equals(3)))
				SpPiano = Resources.Load<Sprite> ("Code/A/Image/A13_A");
			if(_ivar.Equals(1))
				SpPiano = Resources.Load<Sprite> ("Code/A/Image/A14_Am");
			if(_ivar.Equals(2))
				SpPiano = Resources.Load<Sprite> ("Code/A/Image/A15_A7");
		}
		if (_icode.Equals (6)) {
			if((_ivar.Equals(0)) || (_ivar.Equals(3)))
				SpPiano = Resources.Load<Sprite> ("Code/B/Image/B13_B");
			if(_ivar.Equals(1))
				SpPiano = Resources.Load<Sprite> ("Code/B/Image/B14_Bm");
			if(_ivar.Equals(2))
				SpPiano = Resources.Load<Sprite> ("Code/B/Image/B15_B7");
		}
		Image PianoImg = GameObject.Find ("PianoImage").GetComponent<Image> ();
		PianoImg.sprite = SpPiano;
	}

	// --------------------------------------------------------------------------------
	// OwnComInit
	// 自己解説初期化
	// --------------------------------------------------------------------------------
	void OwnComInit(){
		// my own commentデータ取得
		string work = PlayerPrefs.GetString("MscComdata","");
		//string work = "0,85,テスト解説";
		if (work.Equals(""))
			return;

		StringReader reader = new StringReader (work);
		while (reader.Peek () > -1) { // 無限ループ
			string line = reader.ReadLine ();
			string[] values = line.Split (',');
//			Debug.Log ("values[0] = " + values [0]);
//			Debug.Log ("values[1] = " + values [1]);
//			Debug.Log ("values[2] = " + values [2]);
			ls.Add (new strOwnCom (int.Parse (values [0]), int.Parse (values [1]), values [2]));
		}
	}

	// --------------------------------------------------------------------------------
	// CommentDisp
	// 解答と解説を表示
	// --------------------------------------------------------------------------------
	void CommentDisp(string _Qes,string _Com){
		string work = _Qes;
		if (_Com != "")
			work += Environment.NewLine + Environment.NewLine + "～解説～" + Environment.NewLine + _Com;
		ClassText.text = work;
	}



	// --------------------------------------------------------------------------------
	// SentenceDisp
	// 解答と解説を表示
	// --------------------------------------------------------------------------------
	void SentenceDisp(string _Qes,string _Com){
		string work = _Qes;
		if (_Com != "")
			work += Environment.NewLine + Environment.NewLine + "～解説～" + Environment.NewLine + _Com;
		SentenceText.text = work;
	}

	// --------------------------------------------------------------------------------
	// GetOwnComment
	// 自己解説取得(03和音とコードネーム以外)
	// 自己解説が登録されていれば解説を返す
	// 自己解説が登録されていなければ""を返す
	// --------------------------------------------------------------------------------
	string GetOwnComment(int _ClassMode,int _Qnum){
		// コメントゲット
//		_Qnum = _Qnum + 1;
		for (int i = 0; i < ls.Count; i++) {
			if ((ls [i].ClassMode == _ClassMode) &&
			   (ls [i].No == _Qnum)) {
				return ls [i].ComText;
			}
		}

		return "";

	}

	// --------------------------------------------------------------------------------
	// OnClickNext
	// Nextボタン押下処理
	// --------------------------------------------------------------------------------
	public void OnClickNext(){
		Common.btnsnd ();
		alcnt++;

		// 190202 Add
		int i;
		for (i = alcnt; i < alex.Count; i++) {
			string work = alex [i].ToString ();
			string[] values = work.Split (',');
			int ClassMode = int.Parse (values [0]);
			if (Common.SetSubjectCheck (ClassMode))
				break;
		}
		alcnt = i;
		//

		if (alex.Count <= alcnt) {	// 全てを表示したらDairy Caliculum 確認画面へ
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCConfirm");
			return;
		}

		// 次の問題を表示
		QuesDisp(alcnt);
	}

	// --------------------------------------------------------------------------------
	// OnClickBack
	// 戻るボタン押下処理
	// --------------------------------------------------------------------------------
	public void OnClickBack(){
		Common.btnsnd ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCMain");
	}

	// --------------------------------------------------------------------------------
	// OnClickReg
	// 自己解説登録ボタン押下処理
	// --------------------------------------------------------------------------------
	public void OnClickReg(){	// 解説登録
		Common.btnsnd ();
		GameObject obj = GameObject.Find ("ButtonNext");
		obj.GetComponent<Button> ().interactable = false;	// NextButton無効
		obj = GameObject.Find ("CanvasAddCom");
		obj.GetComponent<Canvas> ().enabled = true;			// 登録用キャンバス表示

	}

	// --------------------------------------------------------------------------------
	// OnClickRegReg
	// 自己解説登録の登録ボタン押下処理
	// 自己解説登録は１つずつ(ここでは２つ以上のデータを登録することはない)
	// --------------------------------------------------------------------------------
	public void OnClickRegReg(){
		Common.btnsnd ();
		string work = alex[alcnt].ToString();
		string[] values = work.Split (',');
		int ClassMode = int.Parse (values [0]);

		int Q_num = int.Parse (values [1]);	// 問題番号

		ArrayList alRR = new ArrayList ();

		GameObject obj = GameObject.Find ("InputField");
		InputField textComponent = obj.GetComponent<InputField> ();
		work = textComponent.text;

		OwnComInit ();// 自己解説初期化

		bool flg = false;
		// 解説として登録(教科とNoがキー)
		// 03(和音とコードネーム)かそうでないかで処理が分岐)

		for (int i = 0; i < ls.Count; i++) {
			int _classmode = ls [i].ClassMode;
			int _no = ls [i].No;
			if ((_classmode == ClassMode) &&
			   (_no == Q_num)) {
				// 新データ登録
				alRR.Add (_classmode.ToString () + "," + _no.ToString ("D3") + "," + work);
				flg = true;
			} else {// 既存データ登録
				alRR.Add (_classmode.ToString () + "," + _no.ToString ("D3") + "," + ls [i].ComText);
			}
		}
		if (!flg)// 最後の場合の新規データ登録
			alRR.Add (ClassMode.ToString () + "," + Q_num.ToString ("D3") + "," + work);
		alRR.Sort();

		string svwork = "";
		for (int i = 0; i < alRR.Count; i++) {
			svwork += alRR [i];
			if (i < alRR.Count - 1)
				svwork += Environment.NewLine;
		}

		// セーブ
//		Debug.Log("Saved " + svwork);
		PlayerPrefs.SetString("MscComdata",svwork);

		// 登録画面クローズ
		OnClickRegCan();
		// 解説更新
//		SentenceDisp(workQ,work);
		CommentDisp(workA,work);
	}

	public void OnClickRegCan(){
		Common.btnsnd ();
		GameObject obj = GameObject.Find ("CanvasAddCom");
		obj.GetComponent<Canvas> ().enabled = false;
		obj = GameObject.Find ("ButtonNext");
		obj.GetComponent<Button> ().interactable = true;
	}

	void DispRevNum(){
		GameObject obj = GameObject.Find("TextRevNum");
		Text textcomponent = obj.GetComponent<Text> ();

		// 190202 Add
		//textcomponent.text = alcnt + 1 + " / " + alex.Count;
		textcomponent.text = Q_Now + " / " + Q_All;
	}
}
