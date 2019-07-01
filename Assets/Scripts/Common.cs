using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Common : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    // --------------------------------------------------------------------------------
    // Left
    // VBA's Left
    // --------------------------------------------------------------------------------
    public static string Left(string str, int len)
    {
        if (len < 0)
        {
            Debug.Log("引数'len'は0以上でなければなりません。");
        }
        if (str == null)
        {
            return "";
        }
        if (str.Length <= len)
        {
            return str;
        }
        return str.Substring(0, len);
    }

    /// <summary>
    /// 文字列の末尾から指定した長さの文字列を取得する
    /// </summary>
    /// <param name="str">文字列</param>
    /// <param name="len">長さ</param>
    /// <returns>取得した文字列</returns>
    public static string Right(string str, int len)
    {
        if (len < 0)
        {
            Debug.Log("引数'len'は0以上でなければなりません。");
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
    // SetSubject
    // 数値に応じた単元名を返す
    // --------------------------------------------------------------------------------
    public static string SetSubject(int mode){
		string work = "";
		switch (mode) {
		case 0:
			work = "学習指導要領";
			break;
		case 1:
			work = "調・音階";
			break;
		case 2:
			work = "曲名と作曲者";
			break;
		case 3:
			work = "和音とコードネーム";
			break;
		case 5:
			work = "和音とコードネーム";
			break;
		case 6:
			work = "音楽を聴く";
			break;
		default:
			break;
		}

		return work;
	}

	// --------------------------------------------------------------------------------
	// btnsnd
	// ボタン音を鳴らす
	// --------------------------------------------------------------------------------
	public static void btnsnd(){
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.PlaySEStgSnd ();
		}
	}

	// --------------------------------------------------------------------------------
	// backButtonOnClick
	// バックボタン押下時の処理
	// --------------------------------------------------------------------------------
	public static void backButtonOnClick(int _stage){
		btnsnd ();

		if (_stage.Equals (0))
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("ChooseMode");
		else
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("DCMain");
	}

	// --------------------------------------------------------------------------------
	// ChordRet
	// 数値に応じたコード名を返す
	// --------------------------------------------------------------------------------
	public static string ChordRet(int _mode){
		switch (_mode) {
		case 0:
			return "C";
		case 1:
			return "D";
		case 2:
			return "E";
		case 3:
			return "F";
		case 4:
			return "G";
		case 5:
			return "A";
		case 6:
			return "B";
		}
		return "";
	}

	// --------------------------------------------------------------------------------
	// ChordImg
	// 数値に応じたコード画像を返す
	// --------------------------------------------------------------------------------
	public static Sprite ChordImg(int _Chord,int _var){
		if (_Chord.Equals (0)) {
			switch (_var) {
			case 0:
				return Resources.Load<Sprite> ("Code/C/Image/C01_C");
			case 1:
				return Resources.Load<Sprite> ("Code/C/Image/C02_Cm");
			case 2:
				return Resources.Load<Sprite> ("Code/C/Image/C03_C7");
			case 3:
				return Resources.Load<Sprite> ("Code/C/Image/C04_C_Bass");
			default:
				return null;
			}
		}
		if (_Chord.Equals (1)) {
			switch (_var) {
			case 0:
				return Resources.Load<Sprite> ("Code/D/Image/D01_D");
			case 1:
				return Resources.Load<Sprite> ("Code/D/Image/D02_Dm");
			case 2:
				return Resources.Load<Sprite> ("Code/D/Image/D03_D7");
			case 3:
				return Resources.Load<Sprite> ("Code/D/Image/D04_D_Bass");
			default:
				break;
			}
		}
		if (_Chord.Equals (2)) {
			switch (_var) {
			case 0:
				return Resources.Load<Sprite> ("Code/E/Image/E01_E");
			case 1:
				return Resources.Load<Sprite> ("Code/E/Image/E02_Em");
			case 2:
				return Resources.Load<Sprite> ("Code/E/Image/E03_E7");
			case 3:
				return Resources.Load<Sprite> ("Code/E/Image/E04_E_Bass");
			default:
				break;
			}
		}
		if (_Chord.Equals (3)) {
			switch (_var) {
			case 0:
				return Resources.Load<Sprite> ("Code/F/Image/F01_F");
			case 1:
				return Resources.Load<Sprite> ("Code/F/Image/F02_Fm");
			case 2:
				return Resources.Load<Sprite> ("Code/F/Image/F03_F7");
			case 3:
				return Resources.Load<Sprite> ("Code/F/Image/F04_F_Bass");
			default:
				break;
			}
		}
		if (_Chord.Equals (4)) {
			switch (_var) {
			case 0:
				return Resources.Load<Sprite> ("Code/G/Image/G01_G");
			case 1:
				return Resources.Load<Sprite> ("Code/G/Image/G02_Gm");
			case 2:
				return Resources.Load<Sprite> ("Code/G/Image/G03_G7");
			case 3:
				return Resources.Load<Sprite> ("Code/G/Image/G04_G_Bass");
			default:
				break;
			}
		}
		if (_Chord.Equals (5)) {
			switch (_var) {
			case 0:
				return Resources.Load<Sprite> ("Code/A/Image/A01_A");
			case 1:
				return Resources.Load<Sprite> ("Code/A/Image/A02_Am");
			case 2:
				return Resources.Load<Sprite> ("Code/A/Image/A03_A7");
			case 3:
				return Resources.Load<Sprite> ("Code/A/Image/A04_Bass");
			default:
				break;
			}
		}
		if (_Chord.Equals (6)) {
			switch (_var) {
			case 0:
				return Resources.Load<Sprite> ("Code/B/Image/B01_B");
			case 1:
				return Resources.Load<Sprite> ("Code/B/Image/B02_Bm");
			case 2:
				return Resources.Load<Sprite> ("Code/B/Image/B03_B7");
			case 3:
				return Resources.Load<Sprite> ("Code/B/Image/B04_B_Bass");
			default:
				break;
			}
		}
		return null;
	}
    // --------------------------------------------------------------------------------
    // GetDate
    // 今日日付を8桁で返す
    // --------------------------------------------------------------------------------
    public static string GetDate()
    {
        // 今日日付作成
        int listyear = System.DateTime.Now.Year;
        int listmonth = System.DateTime.Now.Month;
        int listday = System.DateTime.Now.Day;
        return listyear.ToString() + listmonth.ToString("00") + listday.ToString("00");
    }
}
