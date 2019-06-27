using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class GameMainScript : MonoBehaviour {

    public static int mode = 0; // 出題のタイプ 押下ボタンによる mode13をreviewに当てる

	struct Komoku_lst{
		public int No;
		public string lsCode;	// No or Chord
		public string lsSentence;
        public int lsLevel; // レベル用 Shoのみ
        public string lsHelp;   // ヘルプ用

		public Komoku_lst(int No, string lsCode, string lsSentence,int lsLevel,
        string lsHelp){
			this.No = No;
			this.lsCode = lsCode;
			this.lsSentence = lsSentence;
            this.lsLevel = lsLevel;
            this.lsHelp = lsHelp;
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
    string[] strArray;  // 置かれたオブジェクト名を保持するArray

    // 管理データ
    string Daikomoku_str;
    string[] Card_All_str = new string[5];
    int[] Card_All_int = new int[5]; // 0=A 1=B 2=C 3=D 4=E
    bool[] Card_All_help = new bool[5]; // ヘルプボタン押されたか判定
    string[] Card_All_strHelp = new string[5];  // ヘルプ用テキストデータ

    GameObject Deck; // カード５枚の親
    GameObject CardField; // カードフィールド(場)
    public GameObject CorrectPanel; // 正解の花丸イメージ
    public GameObject PanelClear;   // クリアパネル
    private GameObject tForm;
    private GameObject DeckMaster;
    private Text Text_CardNum;  // デッキ枚数
    private Canvas DeckMasterCanvas;    // デッキマスターキャンバス
    private Canvas NextButtonCanvas;    // NextCardボタン

    private Button Button_help; // ヘルプボタン

    Queue<int> Card_queue = new Queue<int>(); // カードデッキキュー

    bool flg_CardBring; // カード保持フラグ
    bool flg_Put; // カード置きフラグ
    bool flg_JnC; // ボタン表示フラグ

    // ミス表示　ペナルティ
    private Canvas MissCanvas;          // Miss時のキャンバス
    private Text Text_MissDisp; // ペナルティ表示用
    private bool flg_Miss;  // ミスフラグ

    // 設定　デッキの数
    bool DeckLimit = true; // デッキリミット false時は無制限
    int Decknum = 6; // デッキ数 DeckLimit = true時のみ参照
    int cnt_MissCard = 0;   // ミスカード枚数カウント用

    public string strDisplayNow;   // 表示中を格納管理

    int[] WholeCardArray;   // 全カード格納配列
    int WholeCardCnt;   // 全カード格納配列用カウント

    SettingDB.SetDB settingdb;

    // scrollRect
    ScrollRect myScrollRect;

    // SaveDataManager
    SaveDataManager SDM;

    // データ格納用配列　誤答 復習用
    static ArrayList almiss = new ArrayList();

    // Use this for initialization
    void Start () {
        // Setting読み込み
        settingdb = SaveData.GetClass<SettingDB.SetDB>("Setting", new SettingDB.SetDB());
        Decknum = settingdb.Q_num; // ワンゲームのデッキ数

        // BGMtoggleがfalseの場合は音を止める
        if (settingdb.BGMToggle == false)
        {
            GameObject obj = GameObject.Find("SoundMaster");
            if (obj != null)
            {
                SoundMaster script = obj.GetComponent<SoundMaster>();
                script.StopBGM();
            }
        }

        tForm = GameObject.Find("JudgeChargeCanvas");
        ColPos_Judge();
        flg_JnC = false; // ボタン表示フラグをオフ
        Deck = GameObject.Find("Deck");
        CardField = GameObject.Find("CardField");
        DeckMaster = GameObject.Find("DeckMaster"); // デッキマスターをセット
        Text_CardNum = GameObject.Find("Text_CardNum").GetComponent<Text>();

        // キャンバス設定
        DeckMasterCanvas = DeckMaster.GetComponent<Canvas>();
        NextButtonCanvas = 
        GameObject.Find("NextButtonCanvas").GetComponent<Canvas>();

        // ペナルティ表示用
        MissCanvas = GameObject.Find("MissCanvas").GetComponent<Canvas>();  // 不正解用キャンバスセット
        Text_MissDisp = GameObject.Find("Text_Penalty").GetComponent<Text>();   // 不正解用テキストセット

        strDisplayNow = ""; // 表示中間利用初期化
        myScrollRect = GameObject.Find("Sentence_Cube").GetComponent<ScrollRect>();

        Button_help = GameObject.Find("Button_help").GetComponent<Button>();

        SDM = GetComponent<SaveDataManager>();  // SaveDataManager
        // セーブデータ削除
        //PlayerPrefs.SetString(SDM.GetStmpName(), "");

        // 復習データインポート
        string str = PlayerPrefs.GetString("MissData","");
        StringReader reader = new StringReader(str);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            almiss.Add(line);
        }

        flg_CardBring = false;  // カード保持フラグ初期化
        flg_Put = false;        // カード置きフラグ初期化
        if (GameLevel.Equals(0))
            GameLevel = 1;

        Daikomoku_str = "";
        for(int i = 0;i < Card_All_str.Length; i++)
        {
            Card_All_str[i] = "";   // カードデータ初期化
            Card_All_help[i] = false;  // カードヘルプ初期化
            Card_All_strHelp[i] = "";   // カードヘルプテキスト初期化
        }

        // 大項目、小項目をcsvより読み込み、それぞれリストに格納する。
        readCSV(2);

        // 問題文を表示
        StuckEmptyPlace();

    }

    // -------------------------------------------------------------------------
    // readCSV
    // CSVを読み込む
    // -------------------------------------------------------------------------
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
            string lslevel = words[2];
            if (mode == 0)
                mode = 1;   
            if (lslevel != mode.ToString()) // ここでバージョンを決める
                continue;   

            switch (_komoku)
            {
                case 1: // 1 = 大項目
                    ls_Daimon.Add(new Komoku_lst(int.Parse(words[0]), words[1],
                        words[2], 0,""));
                    break;
                case 2: // 2 = 小項目
                    ls_Shomon.Add(new Komoku_lst(int.Parse(words[0]), words[1],
                        words[3], int.Parse(words[2]),words[4]));
                    break;
                default:
                    Debug.Log("Error_02");
                    return;
            }
        }
    }

    // -------------------------------------------------------------------------
    // StuckEmptyPlace
    // Startからのみくる初期処理
    // 空の箇所に問題を格納
    // -------------------------------------------------------------------------
    void StuckEmptyPlace()
    {
        // ここでキューに値を格納する
        // 番号のみでよい。基本番号で管理
        // csvデータをランダムにシャッフル
        WholeCardArray = new int[ls_Shomon.Count]; // シャッフル用の一時配列
        for(int i = 0;i < ls_Shomon.Count; i++) // シャッフル用配列に値を格納
        {
            WholeCardArray[i] = ls_Shomon[i].No;
        }

        WholeCardArray = WholeCardArray.Shuffle();

        int MaxLength;
        if ((DeckLimit == false) || (WholeCardArray.Length < Decknum))
            MaxLength = WholeCardArray.Length;
        else
            MaxLength = Decknum;

        for (int i = 0;i < MaxLength;i++)
        {
            Card_queue.Enqueue(WholeCardArray[i]); // シャッフルしたカードデータをデッキに格納
            WholeCardCnt = i + 1;   // ミス時のカード追加用カウント
        }
        // Card_All_strは画面のカード数。常に5
        for (int i = 0; i < Card_All_str.Length; i++){
            if (Card_All_str[i] == "") {
                SetCSVData_Sho();  // 小項目に空があった場合
            }
        }
    }

    // -------------------------------------------------------------------------
    // StuckEmptyPlace_Review
    // 復習用に問題を格納
    // -------------------------------------------------------------------------
    void StuckEmptyPlace_Review()
    {
        // ここでキューに値を格納する
        // 番号のみでよい。基本番号で管理
        // csvデータをランダムにシャッフル

        WholeCardArray = new int[ls_Shomon.Count]; // シャッフル用の一時配列
        for (int i = 0; i < ls_Shomon.Count; i++) // シャッフル用配列に値を格納
        {
            WholeCardArray[i] = ls_Shomon[i].No;
        }

        WholeCardArray = WholeCardArray.Shuffle();

        int MaxLength;
        if ((DeckLimit == false) || (WholeCardArray.Length < Decknum))
            MaxLength = WholeCardArray.Length;
        else
            MaxLength = Decknum;

        for (int i = 0; i < MaxLength; i++)
        {
            Card_queue.Enqueue(WholeCardArray[i]); // シャッフルしたカードデータをデッキに格納
            WholeCardCnt = i + 1;   // ミス時のカード追加用カウント
        }
        // Card_All_strは画面のカード数。常に5
        for (int i = 0; i < Card_All_str.Length; i++)
        {
            if (Card_All_str[i] == "")
            {
                SetCSVData_Sho();  // 小項目に空があった場合
            }
        }
    }

    // -------------------------------------------------------------------------
    // SetCSVData_Sho
    // CSVをセットするデータ
    // 小項目において
    // 空である項目をサーチし
    // 空項目を埋める。
    // 小項目の空が複数存在していれば
    // 可能な限り全てに格納するものとする。
    // -------------------------------------------------------------------------
    void SetCSVData_Sho()
    {

     // 小項目の処理
     // 本当に埋める項目があるかの確認
    // ついでに入っている値の確認
        string[] tmp = new string[5];
        // Card_All_strは画面のカード数。基本5
        for (int i = 0; i < Card_All_str.Length; i++){
            if ((Card_All_str[i] == ""))
            {
                if (Card_queue.Count > 0)
                {
                    int tmpi = CardDequeue();
         //           Card_All_str[i] = NoTolsCode(tmpi); // 正解コード
           //         Card_All_int[i] = tmpi; // 番号を保存
             //       Card_All_txt[i] = NoToSentence(tmpi);
               //     Card_All_strHelp[i] = NoToHelpSentence(tmpi);
                    CardUpdating(i,tmpi); // カード配列更新処理
                }
            }
        }
            DrawScreen();
    }
    // -------------------------------------------------------------------------
    // NoTolsCode(int _no)
    // Noより問題文を取得する
    // -------------------------------------------------------------------------
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

    // -------------------------------------------------------------------------
    // NoToSentence(int _no)
    // Noより問題文を取得する
    // -------------------------------------------------------------------------
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

    // -------------------------------------------------------------------------
    // NoToHelpSentence(int _no)
    // Noよりヘルプ文を取得する
    // -------------------------------------------------------------------------
    public string NoToHelpSentence(int _no)
    {
        string str = "";
        for (int i = 0; i < ls_Shomon.Count; i++) // シャッフル用配列に値を格納
        {
            if (_no == ls_Shomon[i].No)
            {
                str = ls_Shomon[i].lsHelp;
                break;
            }

        }
        return str;
    }

    // -------------------------------------------------------------------------
    // DebugChanged
    // DebugToggleに変化があったらここを改修する
    // -------------------------------------------------------------------------
    public void DebugChanged()
    {
        DrawScreen();
    }

    // -------------------------------------------------------------------------
    // SetCSVData_Dai
    // CSVをセットするデータ
    // 大項目において
    // 空である項目をサーチし
    // 空項目を埋める。
    // -------------------------------------------------------------------------

    void SetCSVData_Dai()
    {
        // 大項目の処理
        // 空か否かの確認
        if (Daikomoku_str == "")
        {
            // 小項目より一つ選択
            int idx = (int)(UnityEngine.Random.value * 99999) % 
            Card_All_str.Length;
            string CorrectSign = Card_All_str[idx];
            while (CorrectSign == "")
            {
                idx = (int)(UnityEngine.Random.value * 99999) % 
                Card_All_str.Length;
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
                        Daikomoku_txt.text = Daikomoku_str + " : " + 
                        ls_Daimon[i].lsSentence;
                    else
                        Daikomoku_txt.text = ls_Daimon[i].lsSentence;
                    break;
                }
            }
        }
    }
    // -------------------------------------------------------------------------
    // DrawScreen
    // 画面への描画処理
    // -------------------------------------------------------------------------
    void DrawScreen()
    {
        // 小項目の描画(大項目は直接描画しているため必要なし)
        int cnt = 0;
        foreach (Transform child in Deck.transform)
        {
            CardDrag script = child.GetComponent<CardDrag>();
            script.str_Help = Card_All_strHelp[cnt];   
            Text textComponent = child.GetComponentInChildren<Text>();
            if (DebugFlg.isOn == true)
                textComponent.text = 
                Card_All_str[cnt] + " : " + Card_All_txt[cnt];
            else
                textComponent.text = Card_All_txt[cnt];
            cnt++;

            Text_CardNum.text = Card_queue.Count.ToString();

            // 詳細の箇所は空欄にする。
            ChangeText_SC("");
            strDisplayNow = ""; // 表示用も空にする
        }

    }

    // -------------------------------------------------------------------------
    // JudgeQuestion
    // 問題の正解不正解判定
    // CardオブジェクトよりCardDrag.csを介して
    // 本処理に飛んでくる。
    // -------------------------------------------------------------------------
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

    // -------------------------------------------------------------------------
    // Right
    // VBAでいうRight関数
    // -------------------------------------------------------------------------
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

    // -------------------------------------------------------------------------
    // CorrectNext
    // 正解処理
    // -------------------------------------------------------------------------
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
		Text textComponent = 
        btn.gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();
		textComponent.text = ary;
	}


	// Update is called once per frame
	void Update () {
        // カードを持っていない
        if ((flg_CardBring == false) && // カード保持していない checked
            (flg_Put == true) &&        // カード置いてある
            (flg_JnC == false))         // ボタン非表示
            Change_JudgenChangeButton(true);
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
			work += "～解説～" + Environment.NewLine + _Com + Environment.NewLine
             + Environment.NewLine;
		work += _instcom;
		GameObject obj = GameObject.Find ("InstText");
		UnityEngine.UI.Text textComponent = 
        obj.GetComponent<UnityEngine.UI.Text> ();
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

	// 戻るボタン
	public void backButtonOnClick(){
        Common.btnsnd(); // ボタン音再生
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("ChooseMode");
    }

    // 解説登録
    public void OnClickReg(){	// 解説登録
		Common.btnsnd ();
		GameObject obj = GameObject.Find ("ButtonNext");
		obj.GetComponent<Button> ().interactable = false;	// NextButton無効
		obj = GameObject.Find ("CanvasAddCom");
		obj.GetComponent<Canvas> ().enabled = true;			// 登録用キャンバス表示
	}

	// 登録キャンセル
	public void OnClickRegCan(){
		Common.btnsnd ();
		GameObject obj = GameObject.Find ("CanvasAddCom");
		obj.GetComponent<Canvas> ().enabled = false;
		obj = GameObject.Find ("ButtonNext");
		obj.GetComponent<Button> ().interactable = true;
	}

    // -------------------------------------------------------------------------
    // 新解答ボタン押下処理
    // -------------------------------------------------------------------------
    public void ButtonClick_S(string _str)
    {
//        Debug.Log(_str);
    }

    // -------------------------------------------------------------------------
    // 解答ボタン押下処理
    // -------------------------------------------------------------------------
    public void ButtonClick(GameObject name){
        /*
		Text textcomponent = name.GetComponent<Text> ();
		if (textcomponent.text == InstCls)
			ButtonAct (true);
		else
			ButtonAct (false);
            */
	}

    // -------------------------------------------------------------------------
    // GameEnd()
    // ゲームエンド処理
    // -------------------------------------------------------------------------
    string GameEnd(){ // デイリーカリキュラム時のゲームエンド              GameObject obj = GameObject.Find ("SoundMaster");         if (obj != null) {             SoundMaster script = obj.GetComponent<SoundMaster> ();             script.PlaySEClr ();         }          var ls = new List<strData> ();          // 既存構造体作成          // スタンプ用保存データ取得         string DataWork = PlayerPrefs.GetString (SDM.GetStmpName (), "");          StringReader reader = new StringReader (DataWork);          if (ls.Count.Equals (0)) {             while (reader.Peek () > -1) {                 string line = reader.ReadLine ();                 string[] values = line.Split (',');                 if (values [0] != "")                     ls.Add (new strData (int.Parse (values [0]), int.Parse (values [1]), int.Parse (values [2]), int.Parse (values [3])));             }         }          int listyear = System.DateTime.Now.Year;         int listmonth = System.DateTime.Now.Month;         int listday = System.DateTime.Now.Day;         string sNow = listyear.ToString() + "," + listmonth.ToString("00") + "," + listday.ToString("00") + "," + mode.ToString("00");          // 比較          if (ls.Count < 1) {             return sNow; // 既存がない場合は最新のみを返す         }          string swork = "";          bool exFlg = false;         bool nwFlg = false;          string sExi = ""; // 既存データ保存用         sNow = "";        // 新規データ保存用          sExi = ls [0].year.ToString () + "," + ls [0].month.ToString ("00") + "," + ls [0].day.ToString ("00") + ","            + ls [0].mode.ToString ("00");         sNow =     listyear.ToString() + "," +     listmonth.ToString("00") + "," +     listday.ToString("00") + ","            +        mode.ToString("00");         string str_ex = ls [0].year.ToString() + ls [0].month.ToString("00") + ls [0].day.ToString("00") 
           + ls [0].mode.ToString("00");         string str_nw =    listyear.ToString() +    listmonth.ToString("00") +    listday.ToString("00")           +        mode.ToString("00");         int i_ex = int.Parse (str_ex);         int i_nw = int.Parse (str_nw);         int i = 0;          while (true) {             if (exFlg) {    // 既存終了時                 swork += sNow;                 break;             }             if (nwFlg) {    // 新規終了時                 swork += sExi;                 break;             }             if (i_ex < i_nw) {  // 既存が小さい時                 swork += sExi + System.Environment.NewLine;                 if (ls.Count > i + 1) {                     ++i;                    sExi = ls [i].year.ToString () + "," + ls [i].month.ToString ("00") + "," + ls [i].day.ToString ("00") + ","                        + ls [i].mode.ToString ("00");                     str_ex = ls [i].year.ToString() + ls [i].month.ToString("00") + ls [i].day.ToString("00")                       + ls [i].mode.ToString("00");                     i_ex = int.Parse (str_ex);             }  else {                 exFlg = true;             }             continue;         }          if (i_ex == i_nw) {             return "already";         }         if (i_ex > i_nw) {  // 新規が小さい時             swork += sNow + System.Environment.NewLine;             nwFlg = true;             continue;         }           //Debug.Log ("something wrong");         return "already";     }       //Debug.Log ("Save Successfully");     return swork;   }

	void btnsnd(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
	}

    // -------------------------------------------------------------------------
    // ReviewUpd
    // 復習問題の項目更新 レビュー時表示する問題のセーブ
    // -------------------------------------------------------------------------
    public void ReviewUpd()
    {
        string strwork = "";
        for(int i = 0; i < almiss.Count; i++)
        {
            if (strwork != "")
                strwork += Environment.NewLine;
            strwork += almiss[i];
        }
        PlayerPrefs.SetString("MissData", strwork);
    }


    // -------------------------------------------------------------------------
    // RetryButton()
    // -------------------------------------------------------------------------
    public void RetryButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameMain");
    }

    // -------------------------------------------------------------------------
    // ToMenuButton()
    // -------------------------------------------------------------------------
    public void ToMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("ChooseMode");
    }

    // -------------------------------------------------------------------------
    // CardDequeue()
    // -------------------------------------------------------------------------
    int CardDequeue()
    {
        int i = Card_queue.Dequeue();
        if (Card_queue.Count == 0)
        {   // デッキが0になったら
            DeckMaster.SetActive(false);    // デッキマスター非表示
            //flg_DeckMasterDisabled = true;  // デッキマスター非表示フラグ
        }
        return i;
    }

    // -------------------------------------------------------------------------
    // GetOwnComment
    // 自己解説取得(03和音とコードネーム以外)
    // 自己解説が登録されていれば解説を返す
    // 自己解説が登録されていなければ""を返す
    // -------------------------------------------------------------------------
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

    // -------------------------------------------------------------------------
    // Changeflg_CardBring
    // カード保持フラグ切り替え
    // -------------------------------------------------------------------------
    public void Changeflg_CardBring(bool _flg)
    {
        if ((flg_CardBring == false) &&  // カード持っていない状態から
        _flg == true) // カードを持っている状態になる時
            Change_JudgenChangeButton(false);
    flg_CardBring = _flg;
    }

    // -------------------------------------------------------------------------
    // Changeflg_Put
    // カード置きフラグ切り替え
    // -------------------------------------------------------------------------
    public void Changeflg_Put(bool _flg)
    {
        if (_flg == true)
            flg_Put = true;
        else if(ColPos_Judge() == false)
            flg_Put = false;
    }

    // -------------------------------------------------------------------------
    // ColPos_Judge
    // それぞれの床に聞きに行く
    // -------------------------------------------------------------------------
    static bool ColPos_Judge()  // それぞれの床に聞きに行く
    {
        // ないとエラーが出る。原因不明
        GameObject CardField = GameObject.Find("CardField");
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

    // -------------------------------------------------------------------------
    // CheckFieldCube()
    // FieldCubeよりUpdateを受けてCheck処理が走る
    // -------------------------------------------------------------------------
    public void CheckFieldCube()
    {
        if (ColPos_Judge() == false)
            flg_Put = false;    // カード置かれていませんよフラグ
    }

    // -------------------------------------------------------------------------
    // ButtonClick_JudgenChange()
    // Judge n Change ボタンクリック処理
    // -------------------------------------------------------------------------
    public void ButtonClick_JudgenChange()
    {
        //flg_operate = false;    // ボタン操作フラグオフ
        Change_JudgenChangeButton(false);
        // 載っているカードの枚数を取得するとして
        // どうやればそれができるのか。
        // それぞれのCardFieldに聞きにいくしかあるまい。
        int cnt = 0;

        // 正解・不正解判定
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

        flg_Miss = false; // ミスフラグ初期化

        for (int i = 0; i < cnt; i++)
        {
            if(strArray[i] == "正解" || strArray[i] == "不正解")
            {
                foreach (Transform child in Deck.transform)
                {
                    if(child.name == "card_" + i)
                    {
                        // カード操作不能にする
                        CardDrag script = child.GetComponent<CardDrag>();
                        script.flg_EnableMove = false;
                        foreach(Transform son in child.transform)
                        {
                            // 花丸の表示
                            if (strArray[i] == "正解" && son.name == "Hanamaru")
                            {
                                son.GetComponent<Image>().enabled = true;
                                break;
                            }
                            // バツの表示
                            else if 
                            (strArray[i] == "不正解" && son.name == "Batu") 
                            {
                                son.GetComponent<Image>().enabled = true;
                                Card_queue.Enqueue(Card_All_int[i]);
                                WrongToReview(Card_All_int[i]); // 復習に情報追加

                                if (settingdb.CardInclude == true)
                                {  // カード追加用設定がOnなら
                                    flg_Miss = true;    // ミスフラグオン
                                    CardInclude();  // ミス時のカード追加処理
                                }
                                break;
                            }
                        }
                        // helpの表示
                        CardDrag CDScript = child.GetComponent<CardDrag>();
                        CDScript.flg_Help = true; // help表示フラグを更新
                        if(child.name == strDisplayNow) // 現在表示中の問題であったらば
                            CDScript.DisplayText_SC(); // 表示の更新
                    }
                }
            }
        }
        NextButtonCanvas.enabled = true;    // NextCardボタン表示
        flg_Put = false;    // カード置きフラグオフ
    }

    // -------------------------------------------------------------------------
    // WrongToReview()
    // 不正解時のMissData反映
    // -------------------------------------------------------------------------
    void WrongToReview(int CAi)
    {
        // for Review
        int listyear = System.DateTime.Now.Year;
        int listmonth = System.DateTime.Now.Month;
        int listday = System.DateTime.Now.Day;
        string inques = listyear.ToString() + listmonth.ToString("00") + listday.ToString("00") + "," + CAi.ToString("000"); // 今日の日付8桁,問題番号
        if (!almiss.Contains(inques))
            almiss.Add(inques);
    }

    // -------------------------------------------------------------------------
    // CardInclude()
    // ミス時のカード追加処理
    // -------------------------------------------------------------------------
    void CardInclude()
    {
        if(WholeCardCnt + 1 >= WholeCardArray.Length)  // 次に追加するカードがオーバーフローを起こしたら
            WholeCardCnt = 0;   // カード追加用カウントをリセット
        Card_queue.Enqueue(WholeCardArray[WholeCardCnt]);   // カード追加
        cnt_MissCard += 1;  // ミスカードカウント加算
        WholeCardCnt++; // カード追加用カウントを加算する
    }

    // -------------------------------------------------------------------------
    // NextCardButton_OnClick()
    // 次のカードボタン押下処理
    // -------------------------------------------------------------------------
    public void NextCardButton_OnClick()
    {
        if (settingdb.CardInclude == true && flg_Miss == true)
        {
            Text_MissDisp.text = "Penalty + " + cnt_MissCard; 
            MissCanvas.enabled = true;  // 不正解キャンバスを表示
        }

        for (int i = 0; i < strArray.Length; i++)
        {
            if (strArray[i] == "正解" || strArray[i] == "不正解")
            {
                foreach (Transform child in Deck.transform)
                {
                    if (child.name == "card_" + i)
                    {
                        if (strArray[i] == "正解")
                        {
                            CardDrag script = child.GetComponent<CardDrag>();
                            // ここで縮小命令をかける
                            script.Card_Reduct_Order();
                            break;
                         }
                        else if (strArray[i] == "不正解")
                        {
                            CardDrag script = child.GetComponent<CardDrag>();
                            // デッキに戻る処理
                                script.DeckBackFunc();
                            break;
                        }
                    }
                }
            }
        }
        NextButtonCanvas.enabled = false;    // NextCardボタン非表示
    }
    // --------------------------------------------------------------------------------
    // CardUpdating()
    // カード管理配列更新処理
    // --------------------------------------------------------------------------------
    void CardUpdating(int _Arrayi,int _tmpi)
    {
        Card_All_str[_Arrayi] = NoTolsCode(_tmpi); // 正解コード
        Card_All_int[_Arrayi] = _tmpi; // 番号を保存
        Card_All_txt[_Arrayi] = NoToSentence(_tmpi);
        Card_All_strHelp[_Arrayi] = NoToHelpSentence(_tmpi);
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
        Card_All_int[_i] = 0;
        Card_All_str[_i] = "";  // 該当を空にする
        Card_All_txt[_i] = "";
        Card_All_help[_i] = false;
        Card_All_strHelp[_i] = "";
    }
    // --------------------------------------------------------------------------------
    // PaneClearCheck()
    // ゲームクリアチェック
    // --------------------------------------------------------------------------------
    void PaneClearCheck()
    {
        for (int i = 0; i < Card_All_str.Length; i++)
        {
            if (Card_All_str[i] != "")
                return;
        }
        string str = GameEnd();
        if (str != "already")
            PlayerPrefs.SetString(SDM.GetStmpName(), str);
        PanelClear.SetActive(true);
    }

    // -------------------------------------------------------------------------
    // OnClick_Help()
    // ヘルプボタン押下処理
    // strDisplayNowには現在詳細画面に表示されているカード名が入っている
    // そのカードのscriptまで行き、help表示flgを立てる
    // 表示の更新をしたらhelp表示が反映される
    // -------------------------------------------------------------------------
    public void OnClick_Help()
    {
        // フラグ変更
        foreach (Transform child in Deck.transform)
        {
            if(child.gameObject.name == strDisplayNow) // 現在表示中のものを探す
            {
                CardDrag script = child.GetComponent<CardDrag>();
                script.flg_Help = true; // help表示フラグを更新
                script.DisplayText_SC(); // 表示の更新
            }
        }
    }

    // -------------------------------------------------------------------------
    // HelpButtonEnabled()
    // ヘルプボタン表示非表示処理
    // -------------------------------------------------------------------------
    public void HelpButtonEnabled(bool _flg)
    {
        Button_help.interactable = _flg;    // ボタン状態変更
    }

    // -------------------------------------------------------------------------
    // ChangeText_SC()
    // helpボタンのための表示切り替え処理
    // 表示が切り替わったらかならずここを通る
    // もし表示が切り替わったら
    // 現在表示中のものは strDisplayNow
    // -------------------------------------------------------------------------
    public void ChangeText_SC(string _txt)
    {
        Text_SC.text = _txt;
        myScrollRect.verticalNormalizedPosition = 1f;   // スクロールバーをトップへ
    }

    // -------------------------------------------------------------------------
    // Change_JudgenChangeButton()
    // JudgenChangeボタンの表示、非表示
    // true = 表示、false = 非表示
    // -------------------------------------------------------------------------
    void Change_JudgenChangeButton(bool _flg)
    {
        tForm.GetComponent<Canvas>().enabled = _flg;
        flg_JnC = _flg;
    }

    // -------------------------------------------------------------------------
    // DeckMasterDisabledFunc(bool _flg)
    // デッキマスター表示処理
    // -------------------------------------------------------------------------
    public void DeckMasterDisabledFunc(bool _flg)
    {
        DeckMaster.SetActive(_flg);
    }

    // -------------------------------------------------------------------------
    // CardDrag_flgActCheck()
    // CardDragのflg_Actチェック処理
    // -------------------------------------------------------------------------
    public void CardDrag_flgActCheck()
    {
        // 全てのカードのフラグ状態をチェック
        foreach (Transform child in Deck.transform)
        {
            CardDrag script = child.GetComponent<CardDrag>();
            if (script.flg_Act == true) // まだ実施中のものがあれば処理を廃棄
                return;
        }
        for (int i = 0; i < Card_All_str.Length; i++)
        {
            if (Card_All_str[i] == "")
            {  // empty
                // カード探す
                GameObject child = GameObject.Find("GameCanvas/Deck/card_" + i);
                if (Card_queue.Count > 0)  // dequeueが空でなくば
                {
                    CardUpdating(i, CardDequeue()); // カードを格納
                    child.SetActive(true);  // デッキを再表示にする
                    CardDrag script = child.GetComponent<CardDrag>();
                    script.Card_Revival(); // カード復活処理
                    if (Card_queue.Count == 0)
                        DeckMasterDisabledFunc(false);  // デッキを再度非表示にする
                }
                else
                {
                    child.SetActive(false); // 非表示に
                    FieldCountCheck(i); // 値を空にする+カウントチェック
                }
            }
        }
        PaneClearCheck();
        DrawScreen();   // デッキ枚数描画等
    }

    // -------------------------------------------------------------------------
    // GetCardBring()
    // カード保持情報の外部からの取得
    // -------------------------------------------------------------------------
    public bool GetCardBring()
    {
        return flg_CardBring;
   }
}