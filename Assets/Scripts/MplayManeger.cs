using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MplayManeger : MonoBehaviour {
	[SerializeField]
	private GameObject btnPref; // ボタンプレハブ

	struct SongList{
		public string song;
		public string arthist;
		public int no;
		public int listno;
		public SongList(string song,string arthist,int no,int listno){
			this.song = song;
			this.arthist = arthist;
			this.no = no;
			this.listno = listno;
		}
	}

	List<SongList> list;
	Text NowPlayText;

	AudioClip song00;	// 00_ふるさと
	AudioClip song01;	// 01_くるみ割り人形
	AudioClip song02;	// 02_白鳥
	AudioClip song03;	// 03_赤とんぼ
	AudioClip song04;	// 04_展覧会の絵
	AudioClip song05;	// 05_新世界より
	AudioClip song06;	// 06_組曲惑星木星
	AudioClip song07;	// 07_浜辺の歌
	AudioClip song08;	// 08_荒城の月
	AudioClip song09;	// 09_待ちぼうけ
	AudioClip song10;	// 10_シャボン玉
	AudioClip song11;	// 11_ます
	AudioClip song12;	// 12_タンホイザー
	AudioClip song13;	// 13_春の海
	AudioClip song14;	// 14_軽騎兵
	AudioClip song15;	// 15_ボレロ
	AudioClip song16;	// 16_PROKOFIEV-PeterAndTheWolf
	AudioClip song17;	// 17_179-Mozart-Magic-Flute-Overture
	AudioClip song18;	// 18_六段の調
	AudioClip song21;	// 21_日のまる
	AudioClip song23;	// 23_春がきた
	AudioClip song25;	// 25_春の小川
	AudioClip song26;	// 26_ふじ山
	AudioClip song27;	// 27_まきばの朝
	AudioClip song28;	// 28_もみじ
	AudioClip song30;	// 30_越天楽今様
	AudioClip song31;	// 31_おぼろ月夜
	AudioClip song32;	// 32_アメリカン・パトロール
	AudioClip song33;	// 33_おもちゃの兵隊
	AudioClip song35;	// 35_おどる人形
	AudioClip song36;	// 36_かじやのポルカ
	AudioClip song37;	// 37_かっこうワルツ
	AudioClip song38;	// 38_S.Prokofev._Syuita_Zimnij_kostyor_soch.122_-_Otezd_(xMuzic.me)
	AudioClip song39;	// 39_Beethoven-Turkish-March
	AudioClip song40;	// 40_歌劇《アルチーナ》から「メヌエット」
	AudioClip song41;	// 41_Dvorak-Humoresque
	AudioClip song42;	// 42_金婚式
	AudioClip song43;	// 43_金と銀
	AudioClip song44;	// 44_Bizet-LArlesienne-Menuett
	AudioClip song45;	// 45_Beethoven-Menuett-inG
	AudioClip song46;	// 46_Bach-SuiteNo2-Bdenerie
	AudioClip song47;	// 47_Schubert-March-Military-No1
	AudioClip song48;	// 48_219-Waldteufel-The-Skaters-Waltz
	AudioClip song49;	// 49_ノルウェー舞曲
	AudioClip song50;	// 50_Rossini-Guillaume-Tell-Overture
	AudioClip song52;	// 52_箱根八里
	AudioClip song53;	// 53_Rentaro-Taki-Four-Seasons-Hana
	AudioClip song54;	// 54_この道
	AudioClip song56;	// 56_Grieg-PeerGynt-Morning
	AudioClip song57;	// 57_流浪の民


	private AudioSource audioSource;

	public Toggle NextFlg;	// 連続再生フラグ

	bool AudioPlaying = false;
	bool PlayTxtDisp = false;
	int PlayNo = 0; // 現在の再生曲を保存

	JukeBox jukebox;

	// Use this for initialization
	void Start () {

		// BGMを止める
		GameObject obj = GameObject.Find ("SoundMaster");
		if (obj != null) {
			SoundMaster script = obj.GetComponent<SoundMaster> ();
			script.StopBGM ();
		}


		// 再生中テキストの表示用
		NowPlayText = GameObject.Find("NowPlayText").GetComponent<Text>();

		audioSource = gameObject.GetComponent<AudioSource>();	// audioInit

		// Content取得(ボタンを並べる場所)
		RectTransform content = GameObject.Find("Canvas/Scroll View/Viewport/Content").GetComponent<RectTransform>();
		// Contentの高さ決定
		// (ボタンの高さ+ボタン同士の感覚)*ボタン数
		float btnSpace = content.GetComponent<VerticalLayoutGroup>().spacing;
		float btnHeight = btnPref.GetComponent<LayoutElement> ().preferredHeight;

		jukebox = GetComponent<JukeBox> ();

		// songデータリストを作成
		list = new List<SongList>();
		list.Add(new SongList("ふるさと","高野辰之・岡野貞一",0,list.Count));
		list.Add (new SongList("組曲《くるみ割り人形》","P.I.チャイコフスキー",1,list.Count));	//  1
		list.Add (new SongList("組曲《動物の謝肉祭》","C.サン・サーンス",2,list.Count));			//  2
		list.Add (new SongList("赤とんぼ","山田耕筰",3,list.Count));								//  3
		list.Add (new SongList("組曲《展覧会の絵》","M・ムソルグスキー",4,list.Count));			//  4
		list.Add (new SongList("《交響曲第９番「新世界より」","A.ドボルザーク",5,list.Count));	//  5
		list.Add (new SongList("組曲《惑星》","グスターヴ・ホルスト",6,list.Count));				//  6
		list.Add (new SongList("浜辺の歌","林古渓・成田為三",7,list.Count));						//  7
		list.Add (new SongList("荒城の月","土井晩翠・瀧廉太郎",8,list.Count));					//  8
		list.Add (new SongList("待ちぼうけ","山田耕作",9,list.Count));							//  9
		list.Add (new SongList("シャボン玉","野口雨情・中山晋平",10,list.Count));				// 10
		list.Add (new SongList("ピアノ五重奏曲「ます」","シューベルト",11,list.Count));			// 11
		list.Add (new SongList("タンホイザー行進曲","R.ワーグナー",12,list.Count));				// 12
		list.Add (new SongList("春の海","宮城道雄",13,list.Count));								// 13
		list.Add (new SongList("歌劇「軽騎兵」","F.v.スッペ",14,list.Count));					// 14
		list.Add (new SongList("バレエ音楽「ボレロ」","モーリス・ラヴェル",15,list.Count));		// 15
		list.Add (new SongList("交響的物語「ピーターと狼」","S.プロコフィエフ",16,list.Count));	// 16
		list.Add (new SongList("歌劇「魔笛」","W.A.モーツァルト",17,list.Count));				// 17
		list.Add (new SongList("筝曲「六段の調」","八橋検校",18,list.Count));					// 18
		list.Add (new SongList("日のまる","高野辰之・岡野貞一",21,list.Count));					// 21
		list.Add (new SongList("春がきた","高野辰之・岡野貞一",23,list.Count));					// 23
		list.Add (new SongList("春の小川","高野辰之・岡野貞一",25,list.Count));					// 25
		list.Add (new SongList("ふじ山","巌谷小波",26,list.Count));								// 26
		list.Add (new SongList("まきばの朝","船橋栄吉",27,list.Count));							// 27
		list.Add (new SongList("もみじ","高野辰之・岡野貞一",28,list.Count));					// 28
		list.Add (new SongList("越天楽今様","慈鎮和尚",30,list.Count));							// 30
		list.Add (new SongList("おぼろ月夜","高野辰之・岡野貞一",31,list.Count));				// 31
		list.Add (new SongList("アメリカン・パトロール","ミーチャム",32,list.Count));				// 32
		list.Add (new SongList("おもちゃの兵隊","イェッセル",33,list.Count));					// 33
		list.Add (new SongList("おどる人形","ポルディーニ",35,list.Count));						// 35
		list.Add (new SongList("かじやのポルカ","ヨーゼフ・シュトラウス",36,list.Count));			// 36
		list.Add (new SongList("かっこうワルツ","ヨナッソン",37,list.Count));					// 37
		list.Add (new SongList("組曲《冬のかがり火》","S.プロコフィエフ",38,list.Count));			// 38
		list.Add (new SongList("《トルコ行進曲》「アテネの廃墟」","ベートーベン",39,list.Count));	// 39
		list.Add (new SongList("歌劇《アルチーナ》から「メヌエット」","ヘンデル",40,list.Count));	// 40
		list.Add (new SongList("８つのユーモレスク","A.ドボルザーク",41,list.Count));				// 41
		list.Add (new SongList("金婚式","マリー",42,list.Count));								// 42
		list.Add (new SongList("金と銀","レハール",43,list.Count));								// 43
		list.Add (new SongList("組曲《アルルの女》","ビゼー",44,list.Count));					// 44
		list.Add (new SongList("６つのメヌエット","ベートーベン",45,list.Count));				// 45
		list.Add (new SongList("管弦楽組曲第２番から「バディネリ」","バッハ",46,list.Count));		// 46
		list.Add (new SongList("軍隊行進曲","シューベルト",47,list.Count));						// 47
		list.Add (new SongList("スケーターズワルツ","ワルトトイフェル",48,list.Count));			// 48
		list.Add (new SongList("ノルウェー舞曲","グリーグ",49,list.Count));						// 49
		list.Add (new SongList("歌劇《ウィリアム・テル》","ロッシーニ",50,list.Count));			// 50
		list.Add (new SongList("箱根八里","滝廉太郎",52,list.Count));							// 52
		list.Add (new SongList("花","滝廉太郎",53,list.Count));									// 53
		list.Add (new SongList("この道","山田耕作",54,list.Count));								// 54
		list.Add (new SongList("組曲「ペール・ギュント」","グリーグ",56,list.Count));				// 56
		list.Add (new SongList("流浪の民","シューマン",57,list.Count));							// 57

		content.sizeDelta = new Vector2(0,(btnHeight + btnSpace) * list.Count);

		for(int i = 0; i < list.Count;i++){
			// ボタン生成
			GameObject btn = (GameObject)Instantiate(btnPref);
			// ボタンをContentの子に設定
			btn.transform.SetParent(content,false);

			string work = list [i].song + System.Environment.NewLine + list[i].arthist;

			// ボタンのテキスト変更
			btn.transform.GetComponentInChildren<Text>().text = work;
			// ボタンのクリックイベント登録
			int ino = list[i].no;
			int _listno = list [i].listno;
			btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(ino,_listno));

		}

	}

	public void OnClick(int no,int _PlayNo){	// no は曲の番号,_PlayNoは配列の番号
		PlayNo = _PlayNo;
		switch (no) {
		case 0:
			if (song00 == null)
				song00 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song00)
				audioSource.clip = song00;
			break;
		case 1:
			if (song01 == null)
				song01 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song01)
				audioSource.clip = song01;
			break;
		case 2:
			if (song02 == null)
				song02 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song02)
				audioSource.clip = song02;
			break;
		case 3:
			if (song03 == null)
				song03 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song03)
				audioSource.clip = song03;
			break;
		case 4:
			if (song04 == null)
				song04 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song04)
				audioSource.clip = song04;
			break;
		case 5:
			if (song05 == null)
				song05 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song05)
				audioSource.clip = song05;
			break;
		case 6:
			if (song06 == null)
				song06 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song06)
				audioSource.clip = song06;
			break;
		case 7:
			if (song07 == null)
				song07 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song07)
				audioSource.clip = song07;
			break;
		case 8:
			if (song08 == null)
				song08 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song08)
				audioSource.clip = song08;
			break;
		case 9:
			if (song09 == null)
				song09 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song09)
				audioSource.clip = song09;
			break;
		case 10:
			if (song10 == null)
				song10 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song10)
				audioSource.clip = song10;
			break;
		case 11:
			if (song11 == null)
				song11 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song11)
				audioSource.clip = song11;
			break;
		case 12:
			if (song12 == null)
				song12 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song12)
				audioSource.clip = song12;
			break;
		case 13:
			if (song13 == null)
				song13 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song13)
				audioSource.clip = song13;
			break;
		case 14:
			if (song14 == null)
				song14 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song14)
				audioSource.clip = song14;
			break;
		case 15:
			if (song15 == null)
				song15 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song15)
				audioSource.clip = song15;
			break;
		case 16:
			if (song16 == null)
				song16 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song16)
				audioSource.clip = song16;
			break;
		case 17:
			if (song17 == null)
				song17 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song17)
				audioSource.clip = song17;
			break;
		case 18:
			if (song18 == null)
				song18 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song18)
				audioSource.clip = song18;
			break;
		case 21:
			if (song21 == null)
				song21 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song21)
				audioSource.clip = song21;
			break;
		case 23:
			if (song23 == null)
				song23 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song23)
				audioSource.clip = song23;
			break;
		case 25:
			if (song25 == null)
				song25 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song25)
				audioSource.clip = song25;
			break;
		case 26:
			if (song26 == null)
				song26 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song26)
				audioSource.clip = song26;
			break;
		case 27:
			if (song27 == null)
				song27 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song27)
				audioSource.clip = song27;
			break;
		case 28:
			if (song28 == null)
				song28 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song28)
				audioSource.clip = song28;
			break;
		case 30:
			if (song30 == null)
				song30 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song30)
				audioSource.clip = song30;
			break;
		case 31:
			if (song31 == null)
				song31 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song31)
				audioSource.clip = song31;
			break;
		case 32:
			if (song32 == null)
				song32 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song32)
				audioSource.clip = song32;
			break;
		case 33:
			if (song33 == null)
				song33 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song33)
				audioSource.clip = song33;
			break;
		case 35:
			if (song35 == null)
				song35 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song35)
				audioSource.clip = song35;
			break;
		case 36:
			if (song36 == null)
				song36 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song36)
				audioSource.clip = song36;
			break;
		case 37:
			if (song37 == null)
				song37 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song37)
				audioSource.clip = song37;
			break;
		case 38:
			if (song38 == null)
				song38 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song38)
				audioSource.clip = song38;
			break;
		case 39:
			if (song39 == null)
				song39 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song39)
				audioSource.clip = song39;
			break;
		case 40:
			if (song40 == null)
				song40 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song40)
				audioSource.clip = song40;
			break;
		case 41:
			if (song41 == null)
				song41 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song41)
				audioSource.clip = song41;
			break;
		case 42:
			if (song42 == null)
				song42 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song42)
				audioSource.clip = song42;
			break;
		case 43:
			if (song43 == null)
				song43 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song43)
				audioSource.clip = song43;
			break;
		case 44:
			if (song44 == null)
				song44 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song44)
				audioSource.clip = song44;
			break;
		case 45:
			if (song45 == null)
				song45 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song45)
				audioSource.clip = song45;
			break;
		case 46:
			if (song46 == null)
				song46 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song46)
				audioSource.clip = song46;
			break;
		case 47:
			if (song47 == null)
				song47 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song47)
				audioSource.clip = song47;
			break;
		case 48:
			if (song48 == null)
				song48 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song48)
				audioSource.clip = song48;
			break;
		case 49:
			if (song49 == null)
				song49 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song49)
				audioSource.clip = song49;
			break;
		case 50:
			if (song50 == null)
				song50 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song50)
				audioSource.clip = song50;
			break;
		case 52:
			if (song52 == null)
				song52 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song52)
				audioSource.clip = song52;
			break;
		case 53:
			if (song53 == null)
				song53 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song53)
				audioSource.clip = song53;
			break;
		case 54:
			if (song54 == null)
				song54 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song54)
				audioSource.clip = song54;
			break;
		case 56:
			if (song56 == null)
				song56 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song56)
				audioSource.clip = song56;
			break;
		case 57:
			if (song57 == null)
				song57 = Resources.Load<AudioClip> (jukebox.Music_load(no));
			if (audioSource.clip != song57)
				audioSource.clip = song57;
			break;

		default:
			return;
		}
		// 音楽を再生
		audioSource.Play ();
		PlayTxtDisp = true;
		AudioPlaying = true;


		// 再生中の音楽を表示
		for (int i = 0; i < list.Count; i++) {
			if(no.Equals(list[i].no)){
				NowPlayText.text = list [i].song + ("  ") +  list [i].arthist;
				break;
			}
		}


	}

	// Update is called once per frame
	void Update () {
		if ((audioSource.isPlaying == false)&&
			(AudioPlaying == true)){
			if ((NextFlg.isOn == true)) {
				PlayNo += 1;
				if (PlayNo > list.Count)
					PlayNo = 0;
				for (int i = 0; i < list.Count; i++) {
					if (PlayNo == list [i].listno)
						OnClick (list [i].no, PlayNo);
				}
			} else if(PlayTxtDisp.Equals(true)){
				NowPlayText.text = "";
				AudioPlaying = false;
				PlayTxtDisp = false;
			}
		}
	}

	// 音楽停止ボタン
	public void OnClick_Stop(){
		audioSource.Stop ();
		AudioPlaying = false;
		// テキストを空にする
		NowPlayText.text = "";
	}

	// 戻るボタン
	public void backButtonOnClick(){
		Common.backButtonOnClick (0);	// 引数は通常タイプ
	}

}
