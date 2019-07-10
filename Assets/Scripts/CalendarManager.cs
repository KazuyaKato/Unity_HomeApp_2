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

	public GameObject stmp1;
    public GameObject stmp2;
    public GameObject stmp3;
    public GameObject stmp4;
    public GameObject stmp5;
    public GameObject stmp6;
    public GameObject stmp7;
    public GameObject stmp8;
    public GameObject stmp9;
    public GameObject stmp10;
    public GameObject stmp11;
    public GameObject stmp12;

    public GameObject stmpCanvas;

	public List<strData> ls = new List<strData>();

	public static string RevQues = "";

    // SaveDataManager
    SaveDataManager SDM;

    // For Store Review
    bool StoreReviewFlg = false;

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

        SDM = GetComponent<SaveDataManager>();  // SaveDataManager

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

        // Reviewモードボタン描画処理

        // 復習問題データを更新
        // このReviewMargeがどうやらやたら重い。
        // どうせ１日１回だけの処理なので１回だけしか行われないようにすればよい
        // 方法としてはせーぶデータを使う方法
        // DailyCheckと銘打ち
        // 今日の日付、結果を保持
        // ex 20190701F またはT
        // Debug
        // PlayerPrefs.SetString("DailyCheck", "19890207F");
        string str = PlayerPrefs.GetString("DailyCheck","19890207F");   // デイリーチェックの最終実施日をロード
        bool flg = false;   // 今日実施できる復習問題の有無フラグ
        string strToday = Common.GetDate();
        if (Common.Left(str, 8) == strToday)
        {    // 今日のものであれば
            if (Common.Right(str, 1) == "T") {    // 有りと判定がすでに出ていれば
                if (PlayerPrefs.GetString("RevWholeMiss", "") != "")
                flg = true; // 復習モードを解放
            }
            else
            {
                PlayerPrefs.SetString("DailyCheck", strToday + "F");   // デイリーチェックの最終実施日を更新
            }

        }
        else
            if (ReviewMerge() == true)
        {   // 今日実施分があれば
            flg = true; // 復習モードを解放
            PlayerPrefs.SetString("DailyCheck", strToday + "T");   // デイリーチェックの最終実施日を更新
        }
        else
            PlayerPrefs.SetString("DailyCheck", strToday + "F");   // デイリーチェックの最終実施日を更新


        if (flg == false)
        {
            obj = GameObject.Find("DCMain_reviewButton");
            Text txt = obj.transform.Find("Text").GetComponent<Text>();
            txt.text = "復習モード" + System.Environment.NewLine + "本日の問題はありません";
            txt.fontSize = 32;
            obj.GetComponent<Button>().interactable = false;
            obj.GetComponent<Image>().color = new Color(93.0f / 255.0f, 93.0f / 255.0f, 93.0f / 255.0f, 120.0f / 255.0f);
            // ボタンの色を変える

        }
	}

    // -------------------------------------------------------------------------
    // ReviewMerge
    // 復習用に問題を格納
    // -------------------------------------------------------------------------
    bool ReviewMerge()
    {
        // 間違えた問題について復習する処理
        // 復習ボタン押下後
        // まず前日含む前日以前のデータを取得
        ArrayList almiss = new ArrayList();
        ArrayList alRevWholeMiss = new ArrayList();
        string str = PlayerPrefs.GetString("RevWholeMiss","");
        StringReader reader = new StringReader(str);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            alRevWholeMiss.Add(line);
        }

        string strToday = Common.GetDate(); // 共通より日付取得

        str = PlayerPrefs.GetString("MissData", "");
        // Debug
        // str = "20190629,0635\n20190629,0795";

        reader = new StringReader(str);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            almiss.Add(line);   // ここで格納されるMissDataは日付付き
        }

        str = "";

        // マージする
        for (int i = 0; i < almiss.Count; i++)
        {
            if (Common.Left(almiss[i].ToString(), 8) != strToday)
            {
                string[] values = almiss[i].ToString().Split(',');
                if (!alRevWholeMiss.Contains(values[1]))
                    alRevWholeMiss.Add(values[1]);
            }
            else
            {
                if (str != "")
                    str += Environment.NewLine;
                str += almiss[i];
            }
        }

        // 反映後はそれぞれを一旦セーブ
        PlayerPrefs.SetString("MissData", str);

        return SaveRevWholeMiss(alRevWholeMiss); // 復習用ミスデータを保存
    }

// -------------------------------------------------------------------------
// SaveRevWholeMiss
// 復習用ミスデータを保存する
// -------------------------------------------------------------------------
bool SaveRevWholeMiss(ArrayList alRevWholeMiss)
{
    string str = "";
    for (int i = 0; i < alRevWholeMiss.Count; i++)
    {
        if (str != "")
            str += Environment.NewLine;
        str += alRevWholeMiss[i];
    }
    PlayerPrefs.SetString("RevWholeMiss", str);
        if (str != "")
            return true;
        else
            return false;
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
		string DataWork = PlayerPrefs.GetString(SDM.GetStmpName(), "");

		StringReader reader = new StringReader (DataWork);

		if (ls.Count.Equals (0)) {
			while (reader.Peek () > -1) {
				string line = reader.ReadLine ();
				string[] values = line.Split (',');
				if (values [0] != "")
					ls.Add (new strData (int.Parse (values [0]), int.Parse (values [1]), int.Parse (values [2]), int.Parse (values [3])));
			}
		}

        // For Store Review
        if (StoreReviewFlg == false &&
            ((ls.Count == 10) ||
            (ls.Count > 11 && ls.Count % 30 == 0)))
        {
            AskReview();    // ストアレビューをお願いする。
            StoreReviewFlg = true;  // 今回はもう表示しない。
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

                int stmpcnt = 0;
				while (true) {
					if (ls.Count > cnt) {
						if (stmpCheck (prevMonth.Year, prevMonth.Month, prevMonthDay, ls [cnt]).Equals (true)) {
                            stmpcnt++;
							cnt = cnt + 1;
						} else {
							break;
						}
					} else {
						break;
					}
				}
                if (stmpcnt > 0)
                    StmpDisp(cDay.transform.position.x, cDay.transform.position.y, stmpcnt);    // スタンプ描画処理
                prevMonthDay++;

			}

			//今月の最終日より後ろのマスには来月の日にちを入れる。

			else if(day > DateTime.DaysInMonth(current.Year, current.Month))

			{

				cDay.dateValue = new DateTime(nextMonth.Year, nextMonth.Month, nextMonthDay);

                int stmpcnt = 0;

				while (true) {
					if (ls.Count > cnt) {
						if (stmpCheck (nextMonth.Year, nextMonth.Month, nextMonthDay, ls [cnt]).Equals (true)) {
                            stmpcnt++;
							cnt = cnt + 1;
						} else {
							break;
						}
					} else {
						break;
					}
				}
                if(stmpcnt > 0)
                    StmpDisp(cDay.transform.position.x, cDay.transform.position.y, stmpcnt);
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

                int stmpcnt = 0;

				while (true) {
					if (ls.Count > cnt) {
						if (stmpCheck (current.Year, current.Month, day, ls [cnt]).Equals (true)) {
                            stmpcnt++;
							cnt = cnt + 1;
						} else {
							break;
						}
					} else {
						break;
					}
				}
                if (stmpcnt > 0)
                    StmpDisp(cDay.transform.position.x, cDay.transform.position.y, stmpcnt);

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
    // 家庭科ではカウントによってスタンプが異なる
	// --------------------------------------------------------------------------------
	void StmpDisp(float _x,float _y,int stmpCnt){
		Vector3 pos = new Vector3 (_x, _y, 0);
		GameObject obj = null;
        switch(stmpCnt)
        {
            case 1:
                obj = Instantiate(stmp1, pos, Quaternion.identity);
                break;
            case 2:
                obj = Instantiate(stmp2, pos, Quaternion.identity);
                break;
            case 3:
                obj = Instantiate(stmp3, pos, Quaternion.identity);
                break;
            case 4:
                obj = Instantiate(stmp4, pos, Quaternion.identity);
                break;
            case 5:
                obj = Instantiate(stmp5, pos, Quaternion.identity);
                break;
            case 6:
                obj = Instantiate(stmp6, pos, Quaternion.identity);
                break;
            case 7:
                obj = Instantiate(stmp7, pos, Quaternion.identity);
                break;
            case 8:
                obj = Instantiate(stmp8, pos, Quaternion.identity);
                break;
            case 9:
                obj = Instantiate(stmp9, pos, Quaternion.identity);
                break;
            case 10:
                obj = Instantiate(stmp10, pos, Quaternion.identity);
                break;
            case 11:
                obj = Instantiate(stmp11, pos, Quaternion.identity);
                break;
            case 12:
                obj = Instantiate(stmp12, pos, Quaternion.identity);
                break;
            default:
                break;
        }

		obj.transform.SetParent(stmpCanvas.transform);
    }

    // --------------------------------------------------------------------------------
    // StartOnClick
    // スタートボタン押下
    // --------------------------------------------------------------------------------
    public void StartOnClick(){
		btnsnd ();

		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("ChooseMode");

	}

    // --------------------------------------------------------------------------------
    // ReviewOnClick
    // スタートボタン押下
    // --------------------------------------------------------------------------------
    public void ReviewOnClick()
    {
        btnsnd();
        GameMainScript.mode = 13;   // 復習モード番号
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameMain");
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

    // --------------------------------------------------------------------------------
    // AskReview
    // レビューお伺い処理
    // --------------------------------------------------------------------------------
    Canvas StoreReviewCanvas;
    void AskReview()
    {
        // スタンプ10個になったらレビューを聞く
        // アプリ内レビューが可能な時はそのままレビュー催促

#if iOS_DEVICE
        // アプリ内レビューに対応している場合はアプリ内で表示
            if (StoreReviewManager.Instance.CanReviewInApp)
            {
                StoreReviewManager.Instance.RequestReview();
            }
            // そうでない時は確認
            else
            {
                // レビューしますか？みたいなUIを表示
                StoreReviewCanvas = GameObject.Find("StoreReviewCanvas").GetComponent<Canvas>();
                StoreReviewCanvas.enabled = true;
            }
#elif ANDROID_DEVICE
                StoreReviewCanvas = GameObject.Find("StoreReviewCanvas").GetComponent<Canvas>();
                StoreReviewCanvas.enabled = true;
#endif       
    }

    // --------------------------------------------------------------------------------
    // Click_StoreReviewYes
    // レビューしてもらえるボタン押下処理
    // --------------------------------------------------------------------------------
    public void Click_StoreReviewYes()
    {
        StoreReviewCanvas.enabled = false; // 評価キャンバス非表示
        StoreReviewManager.Instance.RequestReview();    // ストアに転送
    }

    // --------------------------------------------------------------------------------
    // Click_StoreReviewNo
    // レビューしてもらえないボタン押下処理
    // --------------------------------------------------------------------------------
    public void Click_StoreReviewNo()
    {
        StoreReviewCanvas.enabled = false; // 評価キャンバス非表示
    }

}