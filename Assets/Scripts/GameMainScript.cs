using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class GameMainScript : MonoBehaviour {


	struct Komoku_lst{
		public int No;
		public string lsCode;	// No or Chord
		public string lsSentence;
        public int lsLevel; // レベル用 Shoのみ

		public Komoku_lst(int No, string lsCode, string lsSentence,int lsLevel){
			this.No = No;
			this.lsCode = lsCode;
			this.lsSentence = lsSentence;
            this.lsLevel = lsLevel;
		}
	}

	List<Komoku_lst> ls_Daimon = new List<Komoku_lst>();
    List<Komoku_lst> ls_Shomon = new List<Komoku_lst>();

    // Debug
    public Toggle DebugFlg; // デバッグチェックフラグ
    public int GameLevel;
    //	public int dbgnum = 83;


    // 02曲再生用
    AudioClip JukeClip;
	AudioSource audiosource;

    // Text
    public Text Daikomoku_txt;
    public Text Text_SC;    // 詳細欄
    string[] Card_All_txt = new string[5]; // 0=A 1=B 2=C 3=D 4=E

    // 管理データ
    string Daikomoku_str;
    string[] Card_All_str = new string[5];
    int[] Card_All_int = new int[5]; // 0=A 1=B 2=C 3=D 4=E

    GameObject Deck; // カード５枚の親
    GameObject CardField; // カードフィールド(場)
    public GameObject CorrectPanel; // 正解の花丸イメージ
    public GameObject PanelClear;   // クリアパネル
    private GameObject tForm;
    private GameObject DeckMaster;
    private Canvas DeckMasterCanvas;    // デッキマスターキャンバス
    private Canvas NextButtonCanvas;    // NextCardボタン

    Queue<int> Card_queue = new Queue<int>(); // カードデッキキュー

    bool flg_CardBring; // カード保持フラグ
    bool flg_Put; // カード置きフラグ
    bool flg_JnC; // ボタン表示フラグ
    bool flg_operate;   // カード操作フラグ
    bool flg_DeckMasterDisabled;    // デッキマスター非表示フラグ

    // 設定　デッキの数
    bool DeckLimit = false; // デッキリミット false時は無制限
    int Decknum = 6; // デッキ数 DeckLimit = true時のみ参照



    // Use this for initialization
    void Start () {
        tForm = GameObject.Find("JudgeChargeCanvas");
        ColPos_Judge();
        flg_JnC = false; // ボタン表示フラグをオフ
        Deck = GameObject.Find("Deck");
        CardField = GameObject.Find("CardField");
        DeckMaster = GameObject.Find("DeckMaster"); // デッキマスターをセット
        flg_DeckMasterDisabled = false; // デッキマスター非表示フラグ
        DeckMasterCanvas = DeckMaster.GetComponent<Canvas>();
        NextButtonCanvas = GameObject.Find("NextButtonCanvas").GetComponent<Canvas>();

        flg_CardBring = false;  // カード保持フラグ初期化
        flg_Put = false;        // カード置きフラグ初期化
        flg_operate = true;     // カード操作フラグ初期化
        if (GameLevel.Equals(0))
            GameLevel = 1;

        Daikomoku_str = "";
        for(int i = 0;i < Card_All_str.Length; i++)
        {
            Card_All_str[i] = "";   // カードデータ初期化
        }

        // 大項目、小項目をcsvより読み込み、それぞれリストに格納する。
        readCSV(2);

        // 問題文を表示
        StuckEmptyPlace();

    }

    // --------------------------------------------------------------------------------
    // readCSV
    // CSVを読み込む
    // --------------------------------------------------------------------------------
    void readCSV(int _komoku)
    {
        TextAsset csv = null;
        StringReader reader;
        switch (_komoku)
        {
            case 2: // 2 = 小項目
                csv = Resources.Load("CSV/JH_MDcsv") as TextAsset;
                break;
            default:
                Debug.Log("Error_01");
                return;
        }
        reader = new StringReader(csv.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] words = line.Split(',');

            if (words[0].Equals(""))
            {
                break;
            }

            switch (_komoku)
            {
                case 1: // 1 = 大項目
                    ls_Daimon.Add(new Komoku_lst(int.Parse(words[0]), words[1], words[2],0));
                    break;
                case 2: // 2 = 小項目
                    ls_Shomon.Add(new Komoku_lst(int.Parse(words[0]), words[1], words[3],int.Parse(words[2])));
                    break;
                default:
                    Debug.Log("Error_02");
                    return;
            }
        }
    }

    // --------------------------------------------------------------------------------
    // StuckEmptyPlace
    // Startからのみくる初期処理
    // 空の箇所に問題を格納
    // --------------------------------------------------------------------------------
    void StuckEmptyPlace()
    {
        // ここでキューに値を格納する
        // 番号のみでよい。基本番号で管理
        // csvデータをランダムにシャッフル
        //        ls_Shomon.Shuffle(); // ls_Shomonをシャッフルしている　大元だしシャッフルしたくない
        int[] Array = new int[ls_Shomon.Count]; // シャッフル用の一時配列
        for(int i = 0;i < ls_Shomon.Count; i++) // シャッフル用配列に値を格納
        {
            Array[i] = ls_Shomon[i].No;
        }

        Array = Array.Shuffle();

        int MaxLength;
        if (DeckLimit == false)
            MaxLength = Array.Length;
        else
            MaxLength = Decknum;

        for (int i = 0;i < MaxLength;i++)
        {
            Card_queue.Enqueue(Array[i]); // シャッフルしたカードデータをデッキに格納
        }

        for (int i = 0; i < Card_All_str.Length; i++)   // Card_All_strは画面のカード数。常に5
        {
            if (Card_All_str[i] == "") {
                SetCSVData_Sho();  // 小項目に空があった場合
            }
        }
    }

    // --------------------------------------------------------------------------------
    // SetCSVData_Sho
    // CSVをセットするデータ
    // 小項目において
    // 空である項目をサーチし
    // 空項目を埋める。
    // 小項目の空が複数存在していれば
    // 可能な限り全てに格納するものとする。
    // --------------------------------------------------------------------------------
    void SetCSVData_Sho()
    {

     // 小項目の処理
     // 本当に埋める項目があるかの確認
    // ついでに入っている値の確認
        string[] tmp = new string[5];
        for (int i = 0; i < Card_All_str.Length; i++)   // Card_All_strは画面のカード数。基本5
        {
            if ((Card_All_str[i] == ""))
            {
                if (Card_queue.Count > 0)
                {
                    int tmpi = CardDequeue();
                        Card_All_str[i] = NoTolsCode(tmpi);
                    Card_All_int[i] = tmpi; // 番号を保存
                    Card_All_txt[i] = NoToSentence(tmpi);
                }
            }
        }
            DrawScreen();
    }
    // --------------------------------------------------------------------------------
    // NoTolsCode(int _no)
    // Noより問題文を取得する
    // --------------------------------------------------------------------------------
    public string NoTolsCode(int _no)
    {
        string str = "";
        for (int i = 0; i < ls_Shomon.Count; i++)
        {
            if (_no == ls_Shomon[i].No)
            {
                str = ls_Shomon[i].lsCode;
                break;
            }

        }
        return str;
    }

    // --------------------------------------------------------------------------------
    // NoToSentence(int _no)
    // Noより問題文を取得する
    // --------------------------------------------------------------------------------
    public string NoToSentence(int _no)
    {
        string str = "";
        for (int i = 0; i < ls_Shomon.Count; i++) // シャッフル用配列に値を格納
        {
            if (_no == ls_Shomon[i].No)
            {
                str = ls_Shomon[i].lsSentence;
                break;
            }

        }
        return str;
    }

    // --------------------------------------------------------------------------------
    // DebugChanged
    // DebugToggleに変化があったらここを改修する
    // --------------------------------------------------------------------------------
    public void DebugChanged()
    {
        DrawScreen();
    }

    // --------------------------------------------------------------------------------
    // SetCSVData_Dai
    // CSVをセットするデータ
    // 大項目において
    // 空である項目をサーチし
    // 空項目を埋める。
    // --------------------------------------------------------------------------------

    void SetCSVData_Dai()
    {
        // 大項目の処理
        // 空か否かの確認
        if (Daikomoku_str == "")
        {
            // 小項目より一つ選択
            int idx = (int)(UnityEngine.Random.value * 99999) % Card_All_str.Length;
            string CorrectSign = Card_All_str[idx];
            while (CorrectSign == "")
            {
                idx = (int)(UnityEngine.Random.value * 99999) % Card_All_str.Length;
                CorrectSign = Card_All_str[idx];
            }

            // 大項目より答えがそれとなる問題を取得
            for (int i = 0; i < ls_Daimon.Count; i++)
            {
                if (CorrectSign == ls_Daimon[i].lsCode)
                {
                    // 大項目に問題を格納
                    Daikomoku_str = ls_Daimon[i].lsCode;
                    if (DebugFlg.isOn == true)
                        Daikomoku_txt.text = Daikomoku_str + " : " + ls_Daimon[i].lsSentence;
                    else
                        Daikomoku_txt.text = ls_Daimon[i].lsSentence;
                    break;
                }
            }
        }
    }
    // --------------------------------------------------------------------------------
    // DrawScreen
    // 画面への描画処理
    // --------------------------------------------------------------------------------
    void DrawScreen()
    {
        // 小項目の描画(大項目は直接描画しているため必要なし)
        int cnt = 0;
        foreach (Transform child in Deck.transform)
        {
            Text textComponent = child.GetComponentInChildren<Text>();
            if (DebugFlg.isOn == true)
                textComponent.text = Card_All_str[cnt] + " : " + Card_All_txt[cnt];
            else
                textComponent.text = Card_All_txt[cnt];
            cnt++;
        }

        // 詳細の箇所は空欄にする。
        Text_SC.text = "";
    }

    // --------------------------------------------------------------------------------
    // JudgeQuestion
    // 問題の正解不正解判定
    // CardオブジェクトよりCardDrag.csを介して
    // 本処理に飛んでくる。
    // --------------------------------------------------------------------------------
    public void JudgeQuestion(string _name)
    {
        string LastWord = Right(_name, 1);
        // 該当カードに割り振られた問題を表示
        int iname = 9;
        if (LastWord == "A")
            iname = 0;
        else if (LastWord == "B")
            iname = 1;
        else if (LastWord == "C")
            iname = 2;
        else if (LastWord == "D")
            iname = 3;
        else if (LastWord == "E")
            iname = 4;

        if (Card_All_str[iname] == Daikomoku_str)
        {
            CorrectPanel.SetActive(true);
            Debug.Log("正解");
            // 正解時の処理
            Invoke("CorrectNext", 1.5f);
            Card_All_str[iname] = "";
            Daikomoku_str = "";
        }
        else
            Debug.Log("不正解");

        Debug.Log(LastWord);
    }


    // --------------------------------------------------------------------------------
    // Right
    // VBAでいうRight関数
    // --------------------------------------------------------------------------------
    public static string Right(string str, int len)
    {
        if (len < 0)
        {
            throw new ArgumentException("引数'len'は0以上でなければなりません。");
        }
        if (str == null)
        {
            return "";
        }
        if (str.Length <= len)
        {
            return str;
        }
        return str.Substring(str.Length - len, len);
    }

    // --------------------------------------------------------------------------------
    // CorrectNext
    // 正解処理
    // --------------------------------------------------------------------------------
    // 次の問題へ移行 1.5f
    void CorrectNext()
    {
        // 抽出されたものの選択肢を消す

        // 正解表示を消す
        CorrectPanel.SetActive(false);

        // 正解したカードを消す
        for(int i = 0; i < Card_All_str.Length; i++)
        {
            if (Card_All_str[i] == "")
            {
                int cnt = 0;
                foreach (Transform child in Deck.transform)
                {
                    if (i == cnt)
                    {
                        child.gameObject.SetActive(false);
                        break;
                    }
                    cnt++;
                }
            }
        }

        // 但しカードがなくなったら補充する
        int chk = 0;
        for(chk = 0; chk < Card_All_str.Length; chk++)
        {
            if (Card_All_str[chk] != "")
                break;
        }
        if (chk >= Card_All_str.Length)
        {
            foreach (Transform child in Deck.transform)
            {
                child.gameObject.SetActive(true);
            }
            SetCSVData_Sho();   // 初期化
        }


        // 全てのカードに対し、初期位置へ戻れ命令。
        foreach (Transform child in Deck.transform)
        {
            CardDrag script = child.GetComponent<CardDrag>();
            script.InitPos();
        }


        SetCSVData_Dai();   // 新たな問題描画処理
    }

	// ボタンへの問題描画
	void SetAlteText(Button btn,string ary){
		Text textComponent = btn.gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();
		textComponent.text = ary;
	}


	// Update is called once per frame
	void Update () {
        // 縮小test
        //

        // カードを持っていない
        if ((flg_CardBring == false) && // カード保持していない checked
            (flg_Put == true) &&        // カード置いてある
            (flg_JnC == false))         // ボタン非表示
        {
            tForm.GetComponent<Canvas>().enabled = true;
            flg_JnC = true;
        }
    }

	// 自己解説読み込み
	void GetOwnCom(){
	}

	// 連続カウント処理
	public void Con_Count(bool _res){
        /*
		if(_res.Equals(true))
			NoCA += 1;
		else
			NoCA = 0;
		DispNoCAupd ();
        */
	}

	// 解説表示
	void SentenceDisp(string _Qes,string _Com,string _instcom){
		string work = "正解は" + _Qes + Environment.NewLine;
		if (_Com != "")
			work += "～解説～" + Environment.NewLine + _Com + Environment.NewLine + Environment.NewLine;
		work += _instcom;
		GameObject obj = GameObject.Find ("InstText");
		UnityEngine.UI.Text textComponent = obj.GetComponent<UnityEngine.UI.Text> ();
		textComponent.text = work;
	}

	// 次の問題チェック
	public bool NextChk(){
        /*
		if (stage.Equals (1)) {	// デイリーカリキュラム調整
			DCcnt--;
			if (DCcnt < 1) {
				if(DCInit ().Equals(false))
					return false;
				SetSubject ();
			}
		}
        */
		return true;
	}

	// NoCount更新
	void DispNoCAupd(){
        /*
		GameObject obj = GameObject.Find ("ContinuityText");
		UnityEngine.UI.Text textComponent = obj.GetComponent<UnityEngine.UI.Text> ();
		if (NoCA.Equals (0)) {
			textComponent.text = "";
			DCcnt = settingdb.Q_num;
		}
		else {
			textComponent.text = "連続" + NoCA + "問正解中";

		}
        */
	}

	// 戻るボタン
	public void backButtonOnClick(){
//		Common.backButtonOnClick (stage);
	}

	// 解説登録
	public void OnClickReg(){	// 解説登録
		Common.btnsnd ();
		GameObject obj = GameObject.Find ("ButtonNext");
		obj.GetComponent<Button> ().interactable = false;	// NextButton無効
		obj = GameObject.Find ("CanvasAddCom");
		obj.GetComponent<Canvas> ().enabled = true;			// 登録用キャンバス表示
	}

	// --------------------------------------------------------------------------------
	// OnClickRegReg
	// 解説登録の登録ボタン
	// --------------------------------------------------------------------------------
	public void OnClickRegReg(){
        /*
		Common.btnsnd ();
//		string work = alex[alcnt].ToString();
//		string[] values = work.Split (',');
		int ClassMode = mode;	// 教科

		int Q_num = QuesNo;		// 問題番号

		ArrayList alRR = new ArrayList ();

		GameObject obj = GameObject.Find ("InputField");
		InputField textComponent = obj.GetComponent<InputField> ();
		string work = textComponent.text;

		OwnComInit(PlayerPrefs.GetString("MscComdata","")); // 自己解説初期化

		bool flg = false;
		// 解説として登録(教科とNoがキー)
		for (int i = 0; i < ls.Count; i++) {
			int _classmode = ls [i].ClassMode;
			int _no = ls [i].No;
			if ((_classmode == ClassMode) &&
				(_no == Q_num)) {
				// 新データ登録
				alRR.Add (_classmode.ToString () + "," + _no.ToString ("D3") + "," + work);
				flg = true;
			} else {
				alRR.Add (_classmode.ToString () + "," + _no.ToString ("D3") + "," + ls [i].ComText);
			}
		}
		if(!flg)
			alRR.Add (ClassMode.ToString () + "," + Q_num.ToString ("D3") + "," + work);

		alRR.Sort();

		string svwork = "";
		for (int i = 0; i < alRR.Count; i++) {
			svwork += alRR [i];
			if (i < alRR.Count - 1)
				svwork += Environment.NewLine;
		}

		// セーブ
		PlayerPrefs.SetString("MscComdata",svwork);
		OwnComInit (svwork);

		// 登録画面クローズ
		OnClickRegCan();
		// 解説更新
		SentenceDisp (InstCls, work, InstCom);
        */
	}

	// 登録キャンセル
	public void OnClickRegCan(){
		Common.btnsnd ();
		GameObject obj = GameObject.Find ("CanvasAddCom");
		obj.GetComponent<Canvas> ().enabled = false;
		obj = GameObject.Find ("ButtonNext");
		obj.GetComponent<Button> ().interactable = true;
	}

    // --------------------------------------------------------------------------------
    // 新解答ボタン押下処理
    // --------------------------------------------------------------------------------
    public void ButtonClick_S(string _str)
    {
//        Debug.Log(_str);
    }

    // --------------------------------------------------------------------------------
    // 解答ボタン押下処理
    // --------------------------------------------------------------------------------
    public void ButtonClick(GameObject name){
        /*
		Text textcomponent = name.GetComponent<Text> ();
		if (textcomponent.text == InstCls)
			ButtonAct (true);
		else
			ButtonAct (false);
            */
	}

	// --------------------------------------------------------------------------------
	// DCInit
	// デイリーカリキュラム時の初期設定
	// --------------------------------------------------------------------------------
	bool DCInit(){
        /*
		DCcnt = settingdb.Q_num;
		if (mode.Equals (9)) {
			mode = 0;
			if (settingdb.Gaku.Equals (true))
				return true;
		}
		if ((mode.Equals(0)) &&
			(settingdb.Tone.Equals (true))) {
			mode = 1;
			return true;
		}
		if ((mode < 2) &&
			(settingdb.Song.Equals (true))) {
			mode = 2;
			return true;
		}
		if ((mode < 3) &&
		   (settingdb.Chord.Equals (true))) {
			mode = 3;
			return true;
		}
//		if ((mode < 6) &&
//		    (settingdb.Gym.Equals (true))) {
//			mode = 6;
//			return true;
//		}
		GameEnd ();
        */
		return false;
	}

	// --------------------------------------------------------------------------------
	// SetSubject
	// ・画面に教科を表示
	// ・教科より問題数を取得
	// --------------------------------------------------------------------------------
	void SetSubject(){
        /*
		string work  = Common.SetSubject (mode);	// 共通より教科を取得
		GameObject obj = GameObject.Find ("GenreText");
		UnityEngine.UI.Text textComponent = obj.GetComponent<UnityEngine.UI.Text> ();
		textComponent.text = work; // 教科を表示(学習指導要領など)

		if (mode.Equals (3)) {
			ToneScaleManager TCM = GetComponent<ToneScaleManager> ();
			TCM.ToneQuesDisplay ();
			return;
		}

		CSVReaderScript csvr = GetComponent<CSVReaderScript> ();
		QuestCnt = csvr.GetCsv (mode);	// 教科より問題数を表示
		InstCls = "";
		InstCom = "";

		// 問題を描画
		QuestDisplay();
        */
	}

	void btnsnd(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
	}

    // --------------------------------------------------------------------------------
    // RetryButton()
    // --------------------------------------------------------------------------------
    public void RetryButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameMain");
    }

    // --------------------------------------------------------------------------------
    // CardDequeue()
    // --------------------------------------------------------------------------------
    int CardDequeue()
    {
        int i = Card_queue.Dequeue();
        if (Card_queue.Count == 0)
        {   // デッキが0になったら
            DeckMaster.SetActive(false);    // デッキマスター非表示
            flg_DeckMasterDisabled = true;  // デッキマスター非表示フラグ
        }
        return i;
    }

    // --------------------------------------------------------------------------------
    // GetOwnComment
    // 自己解説取得(03和音とコードネーム以外)
    // 自己解説が登録されていれば解説を返す
    // 自己解説が登録されていなければ""を返す
    // --------------------------------------------------------------------------------
    string GetOwnComment(int _ClassMode,int _Qnum){
		// コメントゲット
/*		for (int i = 0; i < ls.Count; i++) {
			if ((ls [i].ClassMode == _ClassMode) &&
				(ls [i].No == _Qnum)) {
				return ls [i].ComText;
			}
		}
        */
		return "";
	}

    // --------------------------------------------------------------------------------
    // Changeflg_CardBring
    // カード保持フラグ切り替え
    // --------------------------------------------------------------------------------
    public void Changeflg_CardBring(bool _flg)
    {
        if ((flg_CardBring == false) &&  // カード持っていない状態から
        _flg == true) {                   // カードを持っている状態になる時
            tForm.GetComponent<Canvas>().enabled = false;
            flg_JnC = false;
                }
    flg_CardBring = _flg;
    }

    // --------------------------------------------------------------------------------
    // Changeflg_Put
    // カード置きフラグ切り替え
    // --------------------------------------------------------------------------------
    public void Changeflg_Put(bool _flg)
    {
        if (_flg == true)
            flg_Put = true;
        else if(ColPos_Judge() == false)
            flg_Put = false;
    }

    // --------------------------------------------------------------------------------
    // ColPos_Judge
    // それぞれの床に聞きに行く
    // --------------------------------------------------------------------------------
    static bool ColPos_Judge()  // それぞれの床に聞きに行く
    {
        GameObject CardField = GameObject.Find("CardField"); // ないとエラーが出る。原因不明
        foreach (Transform child in CardField.transform)
        {
            foreach(Transform son in child.transform)
            {
                if(Common.Left(son.name,7) == "ColPos_")
                {
                    FieldCube script = son.GetComponent<FieldCube>();
                    if (script.flg_Put == true)
                        return true;

                }
            }
        }

        return false;
    }

    // --------------------------------------------------------------------------------
    // CheckFieldCube()
    // FieldCubeよりUpdateを受けてCheck処理が走る
    // --------------------------------------------------------------------------------
    public void CheckFieldCube()
    {
        if (ColPos_Judge() == false)
            flg_Put = false;    // カード置かれていませんよフラグ
    }

    // --------------------------------------------------------------------------------
    // ButtonClick_JudgenChange()
    // Judge n Change ボタンクリック処理
    // --------------------------------------------------------------------------------
    public void ButtonClick_JudgenChange()
    {
        flg_operate = false;    // ボタン操作フラグオフ
        tForm.GetComponent<Canvas>().enabled = false;   // ボタン非表示
        flg_JnC = false;    // ボタン表示フラグオフ
        // 載っているカードの枚数を取得するとして
        // どうやればそれができるのか。
        // それぞれのCardFieldに聞きにいくしかあるまい。
        int cnt = 0;

        // 正解・不正解判定
        string[] strArray;  // 置かれたオブジェクト名を保持するArray
        cnt = 0; // カウント用
        foreach(Transform child in Deck.transform)
        {
            cnt++;
        }
        strArray = new string[cnt];

        for(int i = 0; i < cnt; i++)
        {
            strArray[i] = "";   // 配列の初期化
        }

        // どのカードがどのlsCodeかはこのScriptが持っている。
        // 問題はどれがどのフィールドに置かれているかということ
        // これを取得すればよい。

        foreach(Transform CFChild in CardField.transform)
        {
            foreach(Transform son in CFChild.transform)
            {
                if(son.name == "ColPos_JH") // 小学生
                {
                    FieldCube script = son.GetComponent<FieldCube>();
                    for ( int i = 0; i< cnt; i++)
                    {
                        if(script.Card_Put(i) == true)
                        {
                            switch (Card_All_str[i])
                            {
                                case "":
                                    strArray[i] = "-";
                                    break;
                                case "JH":
                                    strArray[i] = "正解";
                                    break;
                                default:
                                    strArray[i] = "不正解";
                                    break;
                            }
                        }
                    }
                }
                if (son.name == "ColPos_M") // 小学生
                {
                    FieldCube script = son.GetComponent<FieldCube>();
                    for (int i = 0; i < cnt; i++)
                    {
                        if (script.Card_Put(i) == true)
                        {
                            switch (Card_All_str[i])
                            {
                                case "":
                                    strArray[i] = "-";
                                    break;
                                case "MD":
                                    strArray[i] = "正解";
                                break;
                                default:
                                    strArray[i] = "不正解";
                                break;
                            }
                        }
                    }
                }

            }
        }
        for (int i = 0; i < cnt; i++)
        {
            if(strArray[i] == "正解" || strArray[i] == "不正解")
            {
                foreach (Transform child in Deck.transform)
                {
                    if(child.name == "card_" + i)
                    {
                        foreach(Transform son in child.transform)
                        {
                            if (strArray[i] == "正解" && son.name == "Hanamaru") // 花丸の表示

                            {
                                son.GetComponent<Image>().enabled = true;
                                CardDrag script = child.GetComponent<CardDrag>();
                                // ここで縮小命令をかける
//debug                                script.flg_Reduction = true;
                                break;
                            }
                            else if(strArray[i] == "不正解" && son.name == "Batu") // バツの表示
                            {
                                son.GetComponent<Image>().enabled = true;
                                Card_queue.Enqueue(Card_All_int[i]);
                                CardDrag script = child.GetComponent<CardDrag>();
                                // デッキに戻る処理
//debug                                script.DeckBackFunc();
                                break;
                            }
                        }
                    }
                }
            }
        }
        NextButtonCanvas.enabled = true;    // NextCardボタン表示
        flg_Put = false;    // カード置きフラグオフ
    }
    // --------------------------------------------------------------------------------
    // UpdateCard_All()
    // カード情報更新処理
    // --------------------------------------------------------------------------------
    public void UpdateCard_All(int _i)
    {
        string str = "";
        int i = 9;
        string txt = "";
        if (Card_queue.Count > 0)
        { // デッキにカードがまだあれば
            i = CardDequeue();
            str = NoTolsCode(i);
            txt = NoToSentence(i);
        }
        Card_All_str[_i] = str;
        Card_All_int[_i] = i; // 番号を保存
        Card_All_txt[_i] = txt;

        // カード終了チェック処理
        int j;
        for (j = 0; j < Card_All_str.Length;j++)
        {
            if (Card_All_str[j] != "")
                break;
        }

        if(j < Card_All_str.Length)
            DrawScreen();
        else
        {   // パネルを表示
            PanelClear.SetActive(true);
        }

    }

    // --------------------------------------------------------------------------------
    // DeckMasterOverrideSorting()
    // デッキプライオリティ管理
    // --------------------------------------------------------------------------------
    public void DeckMasterOverrideSorting(int _i)
    {
        DeckMasterCanvas.sortingOrder = _i;
    }

    // --------------------------------------------------------------------------------
    // DeckCountCheck()
    // デッキの残枚数確認
    // --------------------------------------------------------------------------------
    public int DeckCountCheck()
    {
        int i = Card_queue.Count;
        return i;
    }

    // --------------------------------------------------------------------------------
    // FieldCountCheck()
    // フィールドのカードの残枚数確認
    // --------------------------------------------------------------------------------
    public void FieldCountCheck(int _i)
    {
        Card_All_str[_i] = "";  // 該当を空にする
        for(int i = 0;i < Card_All_str.Length; i++)
        {
            if (Card_All_str[i] != "")
                return;
        }
        PanelClear.SetActive(true);
    }

}