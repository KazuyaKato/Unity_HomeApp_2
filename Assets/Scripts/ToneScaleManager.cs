using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToneScaleManager : MonoBehaviour {

	Transform CorrectPrefab;

	// 問題パターンは
	// １メジャーコード
	// ２マイナーコード
	// ３セブンコード
	// ４ヘ音コード
	// の４つ

	// コードは７種類

	Sprite C01_C;
	Sprite C02_Cm;
	Sprite C03_C7;
	Sprite C04_C_Bass;
	Sprite C05_C_Dummy1;
	Sprite C06_C_Dummy2;
	Sprite C07_C7_Dummy1;
	Sprite C08_C7_Dummy2;
	Sprite C09_C7_Dummy3;
	Sprite C10_C_Bass_Dummy1;
	Sprite C11_C_Bass_Dummy2;
	Sprite C12_C_Bass_Dummy3;
	Sprite C13_C;
	Sprite C14_Cm;
	Sprite C15_C7;

	Sprite D01_D;
	Sprite D02_Dm;
	Sprite D03_D7;
	Sprite D04_D_Bass;
	Sprite D05_D_Dummy1;
	Sprite D06_D_Dummy2;
	Sprite D07_D7_Dummy1;
	Sprite D08_D7_Dummy2;
	Sprite D09_D7_Dummy3;
	Sprite D10_D_Bass_Dummy1;
	Sprite D11_D_Bass_Dummy2;
	Sprite D12_D_Bass_Dummy3;
	Sprite D13_D;
	Sprite D14_Dm;
	Sprite D15_D7;

	Sprite E01_E;
	Sprite E02_Em;
	Sprite E03_E7;
	Sprite E04_E_Bass;
	Sprite E05_E_Dummy1;
	Sprite E06_E_Dummy2;
	Sprite E07_E7_Dummy1;
	Sprite E08_E7_Dummy2;
	Sprite E09_E7_Dummy3;
	Sprite E10_E_Bass_Dummy1;
	Sprite E11_E_Bass_Dummy2;
	Sprite E12_E_Bass_Dummy3;
	Sprite E13_E;
	Sprite E14_Em;
	Sprite E15_E7;

	Sprite F01_F;
	Sprite F02_Fm;
	Sprite F03_F7;
	Sprite F04_F_Bass;
	Sprite F05_F_Dummy1;
	Sprite F06_F_Dummy2;
	Sprite F07_F7_Dummy1;
	Sprite F08_F7_Dummy2;
	Sprite F09_F7_Dummy3;
	Sprite F10_F_Bass_Dummy1;
	Sprite F11_F_Bass_Dummy2;
	Sprite F12_F_Bass_Dummy3;
	Sprite F13_F;
	Sprite F14_Fm;
	Sprite F15_F7;

	Sprite G01_G;
	Sprite G02_Gm;
	Sprite G03_G7;
	Sprite G04_G_Bass;
	Sprite G05_G_Dummy1;
	Sprite G06_G_Dummy2;
	Sprite G07_G7_Dummy1;
	Sprite G08_G7_Dummy2;
	Sprite G09_G7_Dummy3;
	Sprite G10_G_Bass_Dummy1;
	Sprite G11_G_Bass_Dummy2;
	Sprite G12_G_Bass_Dummy3;
	Sprite G13_G;
	Sprite G14_Gm;
	Sprite G15_G7;

	Sprite A01_A;
	Sprite A02_Am;
	Sprite A03_A7;
	Sprite A04_A_Bass;
	Sprite A05_A_Dummy1;
	Sprite A06_A_Dummy2;
	Sprite A07_A7_Dummy1;
	Sprite A08_A7_Dummy2;
	Sprite A09_A7_Dummy3;
	Sprite A10_A_Bass_Dummy1;
	Sprite A11_A_Bass_Dummy2;
	Sprite A12_A_Bass_Dummy3;
	Sprite A13_A;
	Sprite A14_Am;
	Sprite A15_A7;

	Sprite B01_B;
	Sprite B02_Bm;
	Sprite B03_B7;
	Sprite B04_B_Bass;
	Sprite B05_B_Dummy1;
	Sprite B06_B_Dummy2;
	Sprite B07_B7_Dummy1;
	Sprite B08_B7_Dummy2;
	Sprite B09_B7_Dummy3;
	Sprite B10_B_Bass_Dummy1;
	Sprite B11_B_Bass_Dummy2;
	Sprite B12_B_Bass_Dummy3;
	Sprite B13_B;
	Sprite B14_Bm;
	Sprite B15_B7;

	Sprite CDEAB;
	Sprite FG;

	GameObject ChooseButton;

	string InstCls = "";

	AudioClip audioC01M;
	AudioClip audioC02m;
	AudioClip audioC037;
	AudioClip audioC04b;

	AudioClip audioD01M;
	AudioClip audioD02m;
	AudioClip audioD037;
	AudioClip audioD04b;

	AudioClip audioE01M;
	AudioClip audioE02m;
	AudioClip audioE037;
	AudioClip audioE04b;

	AudioClip audioF01M;
	AudioClip audioF02m;
	AudioClip audioF037;
	AudioClip audioF04b;

	AudioClip audioG01M;
	AudioClip audioG02m;
	AudioClip audioG037;
	AudioClip audioG04b;

	AudioClip audioA01M;
	AudioClip audioA02m;
	AudioClip audioA037;
	AudioClip audioA04b;

	AudioClip audioB01M;
	AudioClip audioB02m;
	AudioClip audioB037;
	AudioClip audioB04b;

	private AudioSource audioSource;
	GameMainScript GMS;

	// 正解記録用
	int icode;	// コード
	int ivar;	// M,m,7,he

	// Use this for initialization
	void Start () {

		CorrectPrefab = Resources.Load<Transform> ("Image");

		C01_C = Resources.Load<Sprite> ("Code/C/Image/C01_C");
		C02_Cm = Resources.Load<Sprite> ("Code/C/Image/C02_Cm");
		C03_C7 = Resources.Load<Sprite> ("Code/C/Image/C03_C7");
		C04_C_Bass = Resources.Load<Sprite> ("Code/C/Image/C04_C_Bass");
		C05_C_Dummy1 = Resources.Load<Sprite> ("Code/C/Image/C05_C_Dummy1");
		C06_C_Dummy2 = Resources.Load<Sprite> ("Code/C/Image/C06_C_Dummy2");
		C07_C7_Dummy1 = Resources.Load<Sprite> ("Code/C/Image/C07_C7_Dummy1");
		C08_C7_Dummy2 = Resources.Load<Sprite> ("Code/C/Image/C08_C7_Dummy2");
		C09_C7_Dummy3 = Resources.Load<Sprite> ("Code/C/Image/C09_C7_Dummy3");
		C10_C_Bass_Dummy1 = Resources.Load<Sprite> ("Code/C/Image/C10_C_Bass_Dummy1");
		C11_C_Bass_Dummy2 = Resources.Load<Sprite> ("Code/C/Image/C11_C_Bass_Dummy2");
		C12_C_Bass_Dummy3 = Resources.Load<Sprite> ("Code/C/Image/C12_C_Bass_Dummy3");
		C13_C = Resources.Load<Sprite> ("Code/C/Image/C13_C");
		C14_Cm = Resources.Load<Sprite> ("Code/C/Image/C14_Cm");
		C15_C7 = Resources.Load<Sprite> ("Code/C/Image/C15_C7");

		D01_D = Resources.Load<Sprite> ("Code/D/Image/D01_D");
		D02_Dm = Resources.Load<Sprite> ("Code/D/Image/D02_Dm");
		D03_D7 = Resources.Load<Sprite> ("Code/D/Image/D03_D7");
		D04_D_Bass = Resources.Load<Sprite> ("Code/D/Image/D04_D_Bass");
		D05_D_Dummy1 = Resources.Load<Sprite> ("Code/D/Image/D05_D_Dummy1");
		D06_D_Dummy2 = Resources.Load<Sprite> ("Code/D/Image/D06_D_Dummy2");
		D07_D7_Dummy1 = Resources.Load<Sprite> ("Code/D/Image/D07_D7_Dummy1");
		D08_D7_Dummy2 = Resources.Load<Sprite> ("Code/D/Image/D08_D7_Dummy2");
		D09_D7_Dummy3 = Resources.Load<Sprite> ("Code/D/Image/D09_D7_Dummy3");
		D10_D_Bass_Dummy1 = Resources.Load<Sprite> ("Code/D/Image/D10_D_Bass_Dummy1");
		D11_D_Bass_Dummy2 = Resources.Load<Sprite> ("Code/D/Image/D11_D_Bass_Dummy2");
		D12_D_Bass_Dummy3 = Resources.Load<Sprite> ("Code/D/Image/D12_D_Bass_Dummy3");
		D13_D = Resources.Load<Sprite> ("Code/D/Image/D13_D");
		D14_Dm = Resources.Load<Sprite> ("Code/D/Image/D14_Dm");
		D15_D7 = Resources.Load<Sprite> ("Code/D/Image/D15_D7");

		E01_E = Resources.Load<Sprite> ("Code/E/Image/E01_E");
		E02_Em = Resources.Load<Sprite> ("Code/E/Image/E02_Em");
		E03_E7 = Resources.Load<Sprite> ("Code/E/Image/E03_E7");
		E04_E_Bass = Resources.Load<Sprite> ("Code/E/Image/E04_E_Bass");
		E05_E_Dummy1 = Resources.Load<Sprite> ("Code/E/Image/E05_E_Dummy1");
		E06_E_Dummy2 = Resources.Load<Sprite> ("Code/E/Image/E06_E_Dummy2");
		E07_E7_Dummy1 = Resources.Load<Sprite> ("Code/E/Image/E07_E7_Dummy1");
		E08_E7_Dummy2 = Resources.Load<Sprite> ("Code/E/Image/E08_E7_Dummy2");
		E09_E7_Dummy3 = Resources.Load<Sprite> ("Code/E/Image/E09_E7_Dummy3");
		E10_E_Bass_Dummy1 = Resources.Load<Sprite> ("Code/E/Image/E10_E_Bass_Dummy1");
		E11_E_Bass_Dummy2 = Resources.Load<Sprite> ("Code/E/Image/E11_E_Bass_Dummy2");
		E12_E_Bass_Dummy3 = Resources.Load<Sprite> ("Code/E/Image/E12_E_Bass_Dummy3");
		E13_E = Resources.Load<Sprite> ("Code/E/Image/E13_E");
		E14_Em = Resources.Load<Sprite> ("Code/E/Image/E14_Em");
		E15_E7 = Resources.Load<Sprite> ("Code/E/Image/E15_E7");

		F01_F = Resources.Load<Sprite> ("Code/F/Image/F01_F");
		F02_Fm = Resources.Load<Sprite> ("Code/F/Image/F02_Fm");
		F03_F7 = Resources.Load<Sprite> ("Code/F/Image/F03_F7");
		F04_F_Bass = Resources.Load<Sprite> ("Code/F/Image/F04_F_Bass");
		F05_F_Dummy1 = Resources.Load<Sprite> ("Code/F/Image/F05_F_Dummy1");
		F06_F_Dummy2 = Resources.Load<Sprite> ("Code/F/Image/F06_F_Dummy2");
		F07_F7_Dummy1 = Resources.Load<Sprite> ("Code/F/Image/F07_F7_Dummy1");
		F08_F7_Dummy2 = Resources.Load<Sprite> ("Code/F/Image/F08_F7_Dummy2");
		F09_F7_Dummy3 = Resources.Load<Sprite> ("Code/F/Image/F09_F7_Dummy3");
		F10_F_Bass_Dummy1 = Resources.Load<Sprite> ("Code/F/Image/F10_F_Bass_Dummy1");
		F11_F_Bass_Dummy2 = Resources.Load<Sprite> ("Code/F/Image/F11_F_Bass_Dummy2");
		F12_F_Bass_Dummy3 = Resources.Load<Sprite> ("Code/F/Image/F12_F_Bass_Dummy3");
		F13_F = Resources.Load<Sprite> ("Code/F/Image/F13_F");
		F14_Fm = Resources.Load<Sprite> ("Code/F/Image/F14_Fm");
		F15_F7 = Resources.Load<Sprite> ("Code/F/Image/F15_F7");

		G01_G = Resources.Load<Sprite> ("Code/G/Image/G01_G");
		G02_Gm = Resources.Load<Sprite> ("Code/G/Image/G02_Gm");
		G03_G7 = Resources.Load<Sprite> ("Code/G/Image/G03_G7");
		G04_G_Bass = Resources.Load<Sprite> ("Code/G/Image/G04_G_Bass");
		G05_G_Dummy1 = Resources.Load<Sprite> ("Code/G/Image/G05_G_Dummy1");
		G06_G_Dummy2 = Resources.Load<Sprite> ("Code/G/Image/G06_G_Dummy2");
		G07_G7_Dummy1 = Resources.Load<Sprite> ("Code/G/Image/G07_G7_Dummy1");
		G08_G7_Dummy2 = Resources.Load<Sprite> ("Code/G/Image/G08_G7_Dummy2");
		G09_G7_Dummy3 = Resources.Load<Sprite> ("Code/G/Image/G09_G7_Dummy3");
		G10_G_Bass_Dummy1 = Resources.Load<Sprite> ("Code/G/Image/G10_G_Bass_Dummy1");
		G11_G_Bass_Dummy2 = Resources.Load<Sprite> ("Code/G/Image/G11_G_Bass_Dummy2");
		G12_G_Bass_Dummy3 = Resources.Load<Sprite> ("Code/G/Image/G12_G_Bass_Dummy3");
		G13_G = Resources.Load<Sprite> ("Code/G/Image/G13_G");
		G14_Gm = Resources.Load<Sprite> ("Code/G/Image/G14_Gm");
		G15_G7 = Resources.Load<Sprite> ("Code/G/Image/G15_G7");

		A01_A = Resources.Load<Sprite> ("Code/A/Image/A01_A");
		A02_Am = Resources.Load<Sprite> ("Code/A/Image/A02_Am");
		A03_A7 = Resources.Load<Sprite> ("Code/A/Image/A03_A7");
		A04_A_Bass = Resources.Load<Sprite> ("Code/A/Image/A04_Bass");
		A05_A_Dummy1 = Resources.Load<Sprite> ("Code/A/Image/A05_A_Dummy1");
		A06_A_Dummy2 = Resources.Load<Sprite> ("Code/A/Image/A06_A_Dummy2");
		A07_A7_Dummy1 = Resources.Load<Sprite> ("Code/A/Image/A07_A7_Dummy1");
		A08_A7_Dummy2 = Resources.Load<Sprite> ("Code/A/Image/A08_A7_Dummy2");
		A09_A7_Dummy3 = Resources.Load<Sprite> ("Code/A/Image/A09_A7_Dummy3");
		A10_A_Bass_Dummy1 = Resources.Load<Sprite> ("Code/A/Image/A10_Bass_Dummy1");
		A11_A_Bass_Dummy2 = Resources.Load<Sprite> ("Code/A/Image/A11_Bass_Dummy2");
		A12_A_Bass_Dummy3 = Resources.Load<Sprite> ("Code/A/Image/A12_Bass_Dummy3");
		A13_A = Resources.Load<Sprite> ("Code/A/Image/A13_A");
		A14_Am = Resources.Load<Sprite> ("Code/A/Image/A14_Am");
		A15_A7 = Resources.Load<Sprite> ("Code/A/Image/A15_A7");

		B01_B = Resources.Load<Sprite> ("Code/B/Image/B01_B");
		B02_Bm = Resources.Load<Sprite> ("Code/B/Image/B02_Bm");
		B03_B7 = Resources.Load<Sprite> ("Code/B/Image/B03_B7");
		B04_B_Bass = Resources.Load<Sprite> ("Code/B/Image/B04_B_Bass");
		B05_B_Dummy1 = Resources.Load<Sprite> ("Code/B/Image/B05_B_Dummy1");
		B06_B_Dummy2 = Resources.Load<Sprite> ("Code/B/Image/B06_B_Dummy2");
		B07_B7_Dummy1 = Resources.Load<Sprite> ("Code/B/Image/B07_B7_Dummy1");
		B08_B7_Dummy2 = Resources.Load<Sprite> ("Code/B/Image/B08_B7_Dummy2");
		B09_B7_Dummy3 = Resources.Load<Sprite> ("Code/B/Image/B09_B7_Dummy3");
		B10_B_Bass_Dummy1 = Resources.Load<Sprite> ("Code/B/Image/B10_B_Bass_Dummy1");
		B11_B_Bass_Dummy2 = Resources.Load<Sprite> ("Code/B/Image/B11_B_Bass_Dummy2");
		B12_B_Bass_Dummy3 = Resources.Load<Sprite> ("Code/B/Image/B12_B_Bass_Dummy3");
		B13_B = Resources.Load<Sprite> ("Code/B/Image/B13_B");
		B14_Bm = Resources.Load<Sprite> ("Code/B/Image/B14_Bm");
		B15_B7 = Resources.Load<Sprite> ("Code/B/Image/B15_B7");

		CDEAB = Resources.Load<Sprite> ("Code/C/Image/CDEAB");
		FG = Resources.Load<Sprite> ("Code/F/Image/FG");

		ChooseButton = GameObject.Find ("TChooseButton");

		audioC01M = Resources.Load<AudioClip>("Code/C/Sound/C");
		audioC02m = Resources.Load<AudioClip>("Code/C/Sound/Cm");
		audioC037 = Resources.Load<AudioClip>("Code/C/Sound/C7");
		audioC04b = Resources.Load<AudioClip>("Code/C/Sound/Cbass");

		audioD01M = Resources.Load<AudioClip>("Code/D/Sound/D");
		audioD02m = Resources.Load<AudioClip>("Code/D/Sound/Dm");
		audioD037 = Resources.Load<AudioClip>("Code/D/Sound/D7");
		audioD04b = Resources.Load<AudioClip>("Code/D/Sound/Dbass");

		audioE01M = Resources.Load<AudioClip>("Code/E/Sound/E");
		audioE02m = Resources.Load<AudioClip>("Code/E/Sound/Em");
		audioE037 = Resources.Load<AudioClip>("Code/E/Sound/E7");
		audioE04b = Resources.Load<AudioClip>("Code/E/Sound/Ebass");

		audioF01M = Resources.Load<AudioClip>("Code/F/Sound/F");
		audioF02m = Resources.Load<AudioClip>("Code/F/Sound/Fm");
		audioF037 = Resources.Load<AudioClip>("Code/F/Sound/F7");
		audioF04b = Resources.Load<AudioClip>("Code/F/Sound/Fbass");

		audioG01M = Resources.Load<AudioClip>("Code/G/Sound/G");
		audioG02m = Resources.Load<AudioClip>("Code/G/Sound/Gm");
		audioG037 = Resources.Load<AudioClip>("Code/G/Sound/G7");
		audioG04b = Resources.Load<AudioClip>("Code/G/Sound/Gbass");

		audioA01M = Resources.Load<AudioClip>("Code/A/Sound/A");
		audioA02m = Resources.Load<AudioClip>("Code/A/Sound/Am");
		audioA037 = Resources.Load<AudioClip>("Code/A/Sound/A7");
		audioA04b = Resources.Load<AudioClip>("Code/A/Sound/Abass");

		audioB01M = Resources.Load<AudioClip>("Code/B/Sound/B");
		audioB02m = Resources.Load<AudioClip>("Code/B/Sound/Bm");
		audioB037 = Resources.Load<AudioClip>("Code/B/Sound/B7");
		audioB04b = Resources.Load<AudioClip>("Code/B/Sound/Bbass");

		audioSource = gameObject.GetComponent<AudioSource>();	// audioInit

		GMS = GetComponent<GameMainScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//-------------------------------------
	// ToneQuesDisplay
	// 問題を表示
	//-------------------------------------
	public void ToneQuesDisplay (){ //04和音とコードネーム

		// ボタン色リセット
		foreach (Transform child in ChooseButton.transform) {
			child.GetComponent<Image> ().color = Color.white;
		}

		GameObject obj = GameObject.Find ("ToneScaleCanvas");
		obj.GetComponent<Canvas> ().enabled = true;	// 和音とコードネーム用canvasを表示
		obj = GameObject.Find ("ScrollCanvas");
		obj.GetComponent<Canvas>().enabled = false;

		// 出題を決定
		icode = (int)(Random.value * 99999)% 7; // C D E F G A B
		ivar = (int)(Random.value * 99999) % 4; // M m 7 He

		// Debug
		/*
		Debug.Log("Debug");
		icode = 6;
		ivar = 2;
*/

		string[] Array = new string[4];

		for(int i = 0;i < 4;i++){
			Array [i] = (i + 1).ToString ();
		}

		// 選択肢をランダムに並び替え
		System.Random rng = new System.Random();
		int n = Array.Length;
		while (n > 1) {
			n--;
			int k = rng.Next (n + 1);
			string tmp = Array [k];
			Array [k] = Array [n];
			Array [n] = tmp;
		}

		// 選択肢を選択肢内に格納
		n = 0;
		int ptA = 0;
		int ptw1 = 0;
		int ptw2 = 0;
		int ptw3 = 0;

		string work = "";

		work = Common.ChordRet (icode);	// codeよりテキストを取得

		switch (ivar) {
		case 0:
			ptA = 1;
			ptw1 = 2;
			ptw2 = 5;
			ptw3 = 6;
			break;
		case 1:
			work += "m";
			ptA = 2;
			ptw1 = 1;
			ptw2 = 5;
			ptw3 = 6;
			break;
		case 2:
			work += "7";
			ptA = 3;
			ptw1 = 7;
			ptw2 = 8;
			ptw3 = 9;
			break;
		case 3:
			ptA = 4;
			ptw1 = 10;
			ptw2 = 11;
			ptw3 = 12;
			break;
		default:
			break;
		}

		obj = GameObject.Find("ToneText");
		Text TextComponent = obj.GetComponent<Text> ();
		TextComponent.text = work + "は？";

		PianoImageQ (icode);	// 問題用ピアノイラストを描画

		DispImage (int.Parse(Array[0]), (icode * 100) + ptA); // RanQues,DispNo
		DispImage (int.Parse(Array[1]), (icode * 100) + ptw1);
		DispImage (int.Parse(Array[2]), (icode * 100) + ptw2);
		DispImage (int.Parse(Array[3]), (icode * 100) + ptw3);

		// 答え
		switch (int.Parse (Array [0])) {
		case 1:
			InstCls = "ButtonTA";
			break;
		case 2:
			InstCls = "ButtonTB";
			break;
		case 3:
			InstCls = "ButtonTC";
			break;
		case 4:
			InstCls = "ButtonTD";
			break;
		default:
			break;
		}
	}
		
	/*
	void SetAlteText(Button btn,string ary){
		Text textComponent = btn.gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();
		textComponent.text = ary;

	}
*/

	//-------------------------------------
	// ButtonClick
	// ボタンクリック時の挙動
	//-------------------------------------
	public void ButtonClick(GameObject name){
		Text textcomponent = name.GetComponent<Text> ();
		if (textcomponent.text == InstCls)
			ButtonAct (true);
		else
			ButtonAct (false);

		GameObject obj = GameObject.Find(InstCls);
		obj.GetComponent<Image> ().color = new Color(0.0f,0.75f,1.0f);
		PianoImageA (icode,ivar);
	}

	//-------------------------------------
	// ButtonAct
	// 正解不正解判定の結果処理
	//-------------------------------------
	void ButtonAct(bool Judge){	// true 正解  false 不正解
        /*
		GameObject obj;

		// for Review
		string iques = "3," + icode + "," + ivar;

		if (Judge) { // 正解
//			obj = GameObject.Find ("SoundMaster");
//			if (obj != null) {
//				SoundMaster script = obj.GetComponent<SoundMaster> ();
//				script.PlaySECrctSnd ();
//			}
			SoundPlay();

			GMS.Con_Count(true); // 連続カウント処理
				
			var objcor = Instantiate (CorrectPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			objcor.name = "CorrectSign";
			ButtonStatus (false);	// ボタン操作無効

			// for Review
			if(!GameMainScript.alcorrect.Contains(iques))
				GameMainScript.alcorrect.Add (iques);

//			NoCA += 1;
//			DispNoCAupd ();
			Invoke ("CorrectNext", 1.5f);

		} else { // 不正解
			obj = GameObject.Find ("SoundMaster");
			if (obj != null) {
				SoundMaster script = obj.GetComponent<SoundMaster> ();
				script.PlaySEWrng ();
			}

			GMS.Con_Count(false); // 連続カウント処理

			ButtonStatus (false);	// ボタン操作無効

			// NEXT ボタンを表示
			obj = GameObject.Find("ToneNextCanvas");
			obj.GetComponent<Canvas> ().enabled = true;


//			string OwnCom = GetOwnComment (mode,QuesNo);
//			SentenceDisp (InstCls, OwnCom, InstCom);

			// 解説を見せる
//			obj = GameObject.Find ("MissCanvas");
//			obj.GetComponent<Canvas> ().enabled = true;
			// Nextボタン表示


			// for Review
			if(!GameMainScript.almiss.Contains(iques))
				GameMainScript.almiss.Add (iques);

//			NoCA = 0;
//			DispNoCAupd ();
		}
        */
	}

	//-------------------------------------
	// ButtonStatus
	// ボタンステータス
	//-------------------------------------
	void ButtonStatus(bool _flg){
		foreach (Transform child in ChooseButton.transform) {	// 選択肢のボタン操作を変更する
			child.GetComponent<Button>().interactable = _flg;
		}

	}

	//-------------------------------------
	// CorrectNext
	// 次の問題へ移行
	//-------------------------------------
	void CorrectNext(){ // 次の問題へ移行 1.5f

		// NextChk
		if(GMS.NextChk().Equals(false))
			return;

		GameObject obj = GameObject.Find ("CorrectSign");
		Destroy (obj);
		NextQuesTo ();
	}

	//-------------------------------------
	// NextQues
	// 次の問題へ移行・サウンド・移行処理・画面取捨等(間違った時)
	//-------------------------------------
	public void NextQues(){
		btnsnd ();
		NextQuesTo ();
		// ボタン非表示
		GameObject obj = GameObject.Find("ToneNextCanvas");
		obj.GetComponent<Canvas> ().enabled = false;
	}

	//-------------------------------------
	// btnsnd
	// ボタン音再生処理
	//-------------------------------------
	void btnsnd(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
	}

	//-------------------------------------
	// NextQuesTo
	// 次の問題移行処理
	//-------------------------------------
	void NextQuesTo(){
		ButtonStatus (true);
		GameObject obj = GameObject.Find ("MissCanvas");
		obj.GetComponent<Canvas> ().enabled = false;
		ToneQuesDisplay(); // 次の問題へ移行
	}

	//-------------------------------------
	// DispImage
	// 引数を元にボタンイラストを描画
	//-------------------------------------
	void DispImage(int btnNo, int codNo){
		GameObject btn = null;
		switch(btnNo){
		case 1:
			btn = GameObject.Find ("ButtonTA");
			break;
		case 2:
			btn = GameObject.Find ("ButtonTB");
			break;
		case 3:
			btn = GameObject.Find ("ButtonTC");
			break;
		case 4:
			btn = GameObject.Find ("ButtonTD");
			break;
		default:
			break;
		}



		Image btnImage = btn.GetComponent<Image> ();

		switch (codNo) {
		case 1:
			btnImage.sprite = C01_C;
			break;
		case 2:
			btnImage.sprite = C02_Cm;
			break;
		case 3:
			btnImage.sprite = C03_C7;
			break;
		case 4:
			btnImage.sprite = C04_C_Bass;
			break;
		case 5:
			btnImage.sprite = C05_C_Dummy1;
			break;
		case 6:
			btnImage.sprite = C06_C_Dummy2;
			break;
		case 7:
			btnImage.sprite = C07_C7_Dummy1;
			break;
		case 8:
			btnImage.sprite = C08_C7_Dummy2;
			break;
		case 9:
			btnImage.sprite = C09_C7_Dummy3;
			break;
		case 10:
			btnImage.sprite = C10_C_Bass_Dummy1;
			break;
		case 11:
			btnImage.sprite = C11_C_Bass_Dummy2;
			break;
		case 12:
			btnImage.sprite = C12_C_Bass_Dummy3;
			break;
		case 101:
			btnImage.sprite = D01_D;
			break;
		case 102:
			btnImage.sprite = D02_Dm;
			break;
		case 103:
			btnImage.sprite = D03_D7;
			break;
		case 104:
			btnImage.sprite = D04_D_Bass;
			break;
		case 105:
			btnImage.sprite = D05_D_Dummy1;
			break;
		case 106:
			btnImage.sprite = D06_D_Dummy2;
			break;
		case 107:
			btnImage.sprite = D07_D7_Dummy1;
			break;
		case 108:
			btnImage.sprite = D08_D7_Dummy2;
			break;
		case 109:
			btnImage.sprite = D09_D7_Dummy3;
			break;
		case 110:
			btnImage.sprite = D10_D_Bass_Dummy1;
			break;
		case 111:
			btnImage.sprite = D11_D_Bass_Dummy2;
			break;
		case 112:
			btnImage.sprite = D12_D_Bass_Dummy3;
			break;
		case 201:
			btnImage.sprite = E01_E;
			break;
		case 202:
			btnImage.sprite = E02_Em;
			break;
		case 203:
			btnImage.sprite = E03_E7;
			break;
		case 204:
			btnImage.sprite = E04_E_Bass;
			break;
		case 205:
			btnImage.sprite = E05_E_Dummy1;
			break;
		case 206:
			btnImage.sprite = E06_E_Dummy2;
			break;
		case 207:
			btnImage.sprite = E07_E7_Dummy1;
			break;
		case 208:
			btnImage.sprite = E08_E7_Dummy2;
			break;
		case 209:
			btnImage.sprite = E09_E7_Dummy3;
			break;
		case 210:
			btnImage.sprite = E10_E_Bass_Dummy1;
			break;
		case 211:
			btnImage.sprite = E11_E_Bass_Dummy2;
			break;
		case 212:
			btnImage.sprite = E12_E_Bass_Dummy3;
			break;
		case 301:
			btnImage.sprite = F01_F;
			break;
		case 302:
			btnImage.sprite = F02_Fm;
			break;
		case 303:
			btnImage.sprite = F03_F7;
			break;
		case 304:
			btnImage.sprite = F04_F_Bass;
			break;
		case 305:
			btnImage.sprite = F05_F_Dummy1;
			break;
		case 306:
			btnImage.sprite = F06_F_Dummy2;
			break;
		case 307:
			btnImage.sprite = F07_F7_Dummy1;
			break;
		case 308:
			btnImage.sprite = F08_F7_Dummy2;
			break;
		case 309:
			btnImage.sprite = F09_F7_Dummy3;
			break;
		case 310:
			btnImage.sprite = F10_F_Bass_Dummy1;
			break;
		case 311:
			btnImage.sprite = F11_F_Bass_Dummy2;
			break;
		case 312:
			btnImage.sprite = F12_F_Bass_Dummy3;
			break;
		case 401:
			btnImage.sprite = G01_G;
			break;
		case 402:
			btnImage.sprite = G02_Gm;
			break;
		case 403:
			btnImage.sprite = G03_G7;
			break;
		case 404:
			btnImage.sprite = G04_G_Bass;
			break;
		case 405:
			btnImage.sprite = G05_G_Dummy1;
			break;
		case 406:
			btnImage.sprite = G06_G_Dummy2;
			break;
		case 407:
			btnImage.sprite = G07_G7_Dummy1;
			break;
		case 408:
			btnImage.sprite = G08_G7_Dummy2;
			break;
		case 409:
			btnImage.sprite = G09_G7_Dummy3;
			break;
		case 410:
			btnImage.sprite = G10_G_Bass_Dummy1;
			break;
		case 411:
			btnImage.sprite = G11_G_Bass_Dummy2;
			break;
		case 412:
			btnImage.sprite = G12_G_Bass_Dummy3;
			break;
		case 501:
			btnImage.sprite = A01_A;
			break;
		case 502:
			btnImage.sprite = A02_Am;
			break;
		case 503:
			btnImage.sprite = A03_A7;
			break;
		case 504:
			btnImage.sprite = A04_A_Bass;
			break;
		case 505:
			btnImage.sprite = A05_A_Dummy1;
			break;
		case 506:
			btnImage.sprite = A06_A_Dummy2;
			break;
		case 507:
			btnImage.sprite = A07_A7_Dummy1;
			break;
		case 508:
			btnImage.sprite = A08_A7_Dummy2;
			break;
		case 509:
			btnImage.sprite = A09_A7_Dummy3;
			break;
		case 510:
			btnImage.sprite = A10_A_Bass_Dummy1;
			break;
		case 511:
			btnImage.sprite = A11_A_Bass_Dummy2;
			break;
		case 512:
			btnImage.sprite = A12_A_Bass_Dummy3;
			break;
		case 601:
			btnImage.sprite = B01_B;
			break;
		case 602:
			btnImage.sprite = B02_Bm;
			break;
		case 603:
			btnImage.sprite = B03_B7;
			break;
		case 604:
			btnImage.sprite = B04_B_Bass;
			break;
		case 605:
			btnImage.sprite = B05_B_Dummy1;
			break;
		case 606:
			btnImage.sprite = B06_B_Dummy2;
			break;
		case 607:
			btnImage.sprite = B07_B7_Dummy1;
			break;
		case 608:
			btnImage.sprite = B08_B7_Dummy2;
			break;
		case 609:
			btnImage.sprite = B09_B7_Dummy3;
			break;
		case 610:
			btnImage.sprite = B10_B_Bass_Dummy1;
			break;
		case 611:
			btnImage.sprite = B11_B_Bass_Dummy2;
			break;
		case 612:
			btnImage.sprite = B12_B_Bass_Dummy3;
			break;
		default:
			break;
		}
	}

	//-------------------------------------
	// PianoImageQ
	// 引数を元にピアノイラストを描画(出題時)
	//-------------------------------------
	void PianoImageQ(int _code){
		Image img = GameObject.Find ("ToneScalePianoImage").GetComponent<Image> ();
		if ((_code.Equals (3)) ||	// F
		   (_code.Equals (4)))		// G
			img.sprite = FG;
		else
			img.sprite = CDEAB;		// CDEAB
	}

	//-------------------------------------
	// PianoImageA
	// 引数を元にピアノイラストを描画(解答時)
	//-------------------------------------
	void PianoImageA(int _code,int _var){
		Image img = GameObject.Find ("ToneScalePianoImage").GetComponent<Image> ();
		switch (_code) {
		case 0:	// C
			if (_var.Equals (0) ||
			   _var.Equals (3))
				img.sprite = C13_C;
			if (_var.Equals (1))
				img.sprite = C14_Cm;
			if (_var.Equals (2))
				img.sprite = C15_C7;
			break;
		case 1:	// D
			if (_var.Equals (0) ||
			   _var.Equals (3))
				img.sprite = D13_D;
			if (_var.Equals (1))
				img.sprite = D14_Dm;
			if (_var.Equals (2))
				img.sprite = D15_D7;
			break;
		case 2:	// E
			if (_var.Equals (0) ||
				_var.Equals (3))
				img.sprite = E13_E;
			if (_var.Equals (1))
				img.sprite = E14_Em;
			if (_var.Equals (2))
				img.sprite = E15_E7;
			break;
		case 3:	// F
			if (_var.Equals (0) ||
				_var.Equals (3))
				img.sprite = F13_F;
			if (_var.Equals (1))
				img.sprite = F14_Fm;
			if (_var.Equals (2))
				img.sprite = F15_F7;
			break;
		case 4:	// G
			if (_var.Equals (0) ||
				_var.Equals (3))
				img.sprite = G13_G;
			if (_var.Equals (1))
				img.sprite = G14_Gm;
			if (_var.Equals (2))
				img.sprite = G15_G7;
			break;
		case 5:	// A
			if (_var.Equals (0) ||
				_var.Equals (3))
				img.sprite = A13_A;
			if (_var.Equals (1))
				img.sprite = A14_Am;
			if (_var.Equals (2))
				img.sprite = A15_A7;
			break;
		case 6:	// B
			if (_var.Equals (0) ||
				_var.Equals (3))
				img.sprite = B13_B;
			if (_var.Equals (1))
				img.sprite = B14_Bm;
			if (_var.Equals (2))
				img.sprite = B15_B7;
			break;

		default:
			break;
		}
	}

	//-------------------------------------
	// SoundPlay
	// コード音再生
	//-------------------------------------
	void SoundPlay(){
		if (icode.Equals (0)) {	// C
			if (ivar.Equals (0))
				audioSource.clip = audioC01M;
			if (ivar.Equals (1))
				audioSource.clip = audioC02m;
			if (ivar.Equals (2))
				audioSource.clip = audioC037;
			if (ivar.Equals (3))
				audioSource.clip = audioC04b;
		}
		else if (icode.Equals (1)) {	// D
			if (ivar.Equals (0))
				audioSource.clip = audioD01M;
			if (ivar.Equals (1))
				audioSource.clip = audioD02m;
			if (ivar.Equals (2))
				audioSource.clip = audioD037;
			if (ivar.Equals (3))
				audioSource.clip = audioD04b;
		}
		else if (icode.Equals (2)) {	// E
			if (ivar.Equals (0))
				audioSource.clip = audioE01M;
			if (ivar.Equals (1))
				audioSource.clip = audioE02m;
			if (ivar.Equals (2))
				audioSource.clip = audioE037;
			if (ivar.Equals (3))
				audioSource.clip = audioE04b;
		}
		else if (icode.Equals (3)) {	// F
			if (ivar.Equals (0))
				audioSource.clip = audioF01M;
			if (ivar.Equals (1))
				audioSource.clip = audioF02m;
			if (ivar.Equals (2))
				audioSource.clip = audioF037;
			if (ivar.Equals (3))
				audioSource.clip = audioF04b;
		}
		else if (icode.Equals (4)) {	// G
			if (ivar.Equals (0))
				audioSource.clip = audioG01M;
			if (ivar.Equals (1))
				audioSource.clip = audioG02m;
			if (ivar.Equals (2))
				audioSource.clip = audioG037;
			if (ivar.Equals (3))
				audioSource.clip = audioG04b;
		}
		else if (icode.Equals (5)) {	// A
			if (ivar.Equals (0))
				audioSource.clip = audioA01M;
			if (ivar.Equals (1))
				audioSource.clip = audioA02m;
			if (ivar.Equals (2))
				audioSource.clip = audioA037;
			if (ivar.Equals (3))
				audioSource.clip = audioA04b;
		}
		else if (icode.Equals (6)) {	// B
			if (ivar.Equals (0))
				audioSource.clip = audioB01M;
			if (ivar.Equals (1))
				audioSource.clip = audioB02m;
			if (ivar.Equals (2))
				audioSource.clip = audioB037;
			if (ivar.Equals (3))
				audioSource.clip = audioB04b;
		}
		audioSource.Play ();

	}

}
