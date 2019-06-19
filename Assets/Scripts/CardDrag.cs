using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour
{
    public GameObject GameMain;
    GameMainScript GMScript;    // ゲームメインスクリプト

    float posx;
    float posy;
    int thisnamenum;    // 本オブジェクトの名前

    bool colflg = false; // 衝突用フラグ
    bool pointerflg = false; // 持っているかどうかのフラグ
    private bool flg_Reduction = false; //  縮小フラグ
    public bool flg_EnableMove = true;  // カード操作可能フラグ
    private bool flg_DeckBack = false;   // デッキに戻るフラグ
    public bool flg_Act = false;    // 処理中フラグ 処理中はオン

    // help機能用
    public bool flg_Help = false;   // ヘルプ機能のonoff
    public string str_Help = "";    // ヘルプ内容

    Image Hanamaru; // 花丸マーク
    Image Batu; // 罰マーク
    Vector3 DeckMasterPos;  // デッキマスター座標

    // --------------------------------------------------------------------------------
    // OnDrag
    // ドラッグ処理
    // --------------------------------------------------------------------------------
    public void OnDrag() // ドラッグ処理
    {
        if(flg_EnableMove == true)  // フラグ操作可能時
        {
            Vector3 TapPos = Input.mousePosition;
            TapPos.z = 10f;
            transform.position = Camera.main.ScreenToWorldPoint(TapPos);
            if(pointerflg == false)
            {
                GMScript.Changeflg_CardBring(true);
                pointerflg = true;
            }
        }
    }

    // -------------------------------------------------------------------------
    // PointerDown
    // 振れた時の処理。詳細欄に内容が表示される
    // -------------------------------------------------------------------------
    public void PointerDown()   // 振れた時の処理。詳細欄に内容が表示される
    {
        DisplayText_SC(); // 詳細欄への記入処理
        pointerflg = true; // 持っている
        GMScript.Changeflg_CardBring(true); // 持ってますよフラグオン
        GMScript.strDisplayNow = this.gameObject.name;  // 本オブジェクトの名前
    }

    // -------------------------------------------------------------------------
    // DisplayText_SC
    // 詳細欄への記入処理
    // -------------------------------------------------------------------------
    public void DisplayText_SC()
    {
        if (flg_Help == false)  // ヘルプフラグチェック_True時はヘルプ反映のものを表示
        {  
            GMScript.ChangeText_SC // テキストの変更
            (this.gameObject.GetComponentInChildren<Text>().text);
            GMScript.HelpButtonEnabled(true);   // ボタン表示ON
        }
        else
        {
            GMScript.ChangeText_SC(str_Help); // helpの文章で表示
            GMScript.HelpButtonEnabled(false);  // ボタン表示OFF
        }
    }

    // -------------------------------------------------------------------------
    // PointerUp
    // 手を離した時の処理。
    // -------------------------------------------------------------------------
    public void PointerUp() // 手を離した時の処理。
    {
/*        GameObject obj = GameObject.Find("SoundMaster");
        if (obj != null)
        {
            SoundMaster script = obj.GetComponent<SoundMaster>();
            script.PlaySEPaperFlip();
        }
        */

        GMScript.Changeflg_CardBring(false); // 持ってますよフラグoff
        if (colflg == false)
        {
            InitPos();  // 初期位置へ戻す
            pointerflg = false; // 持っていない
            GMScript.Changeflg_Put(false); // 置きflgをoffへ？
        }
    }

    // --------------------------------------------------------------------------------
    // InitPos
    // 初期位置処理
    // --------------------------------------------------------------------------------
    public void InitPos()
    {
        colflg = false;
    }

    // --------------------------------------------------------------------------------
    // DeckBackFunc()
    // 初期位置処理
    // --------------------------------------------------------------------------------
    public void DeckBackFunc()
    {
        flg_DeckBack = true;    // デッキにカードを戻す処理
        GMScript.DeckMasterOverrideSorting(2);  // マスターデッキを浮かせる
        flg_Act = true; // フラグ挙動
    }

    // --------------------------------------------------------------------------------
    // OnTriggerExit2D
    // 衝突から離れた処理
    // --------------------------------------------------------------------------------
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Common.Left(collision.gameObject.name, 7) == "ColPos_")
        {
            colflg = false;
        }
    }

    // --------------------------------------------------------------------------------
    // OnTriggerStay2D
    // 衝突中の処理
    // --------------------------------------------------------------------------------
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Common.Left(collision.gameObject.name, 7) == "ColPos_" 
        && pointerflg == true)
            colflg = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        posx = this.transform.position.x;
        posy = this.transform.position.y;
        GameMain = GameObject.Find("GameMain");
        GMScript = GameMain.GetComponent<GameMainScript>();
        Hanamaru = this.transform.Find("Hanamaru").GetComponent<Image>();
        Batu = this.transform.Find("Batu").GetComponent<Image>();
        DeckMasterPos = GameObject.Find("DeckMaster").transform.Find("card").gameObject
            .transform.position;
        thisnamenum = int.Parse(this.name.Replace("card_", ""));
    }

    // Update is called once per frame
    void Update()
    {
        if(colflg == false && pointerflg == false)
            transform.position = Vector3.Lerp(new Vector3(
            this.transform.position.x,this.transform.position.y, 
                this.transform.position.z),
            new Vector3(posx, posy, this.transform.position.z),
        Time.deltaTime * 3);

        if (flg_Reduction　== true)  // カード縮小処理
        {
            Transform tra = this.transform; // トランスフォーム情報を保存
            Vector3 tmp = tra.localScale;
            if (tmp.y > 0.1f)
            {
                transform.localScale -= new Vector3(0.04f, 0.04f, 0);   // 縮小スピード
            }
            else
            {
                flg_Reduction = false;
                flg_Act = false;
                GMScript.FieldCountCheck(thisnamenum);
                GMScript.CardDrag_flgActCheck();    // フラグアクトチェック
            }
        }

        // デッキに戻る処理
        if (flg_DeckBack == true)
        {
            transform.position = Vector3.Lerp(new Vector3(
            this.transform.position.x, this.transform.position.y,
                this.transform.position.z),
            new Vector3(DeckMasterPos.x, DeckMasterPos.y, this.transform.position.z),
        Time.deltaTime * 4);
            if (this.transform.position.y < DeckMasterPos.y + 0.1f)
            {
                flg_DeckBack = false;
                GMScript.DeckMasterOverrideSorting(1);  // マスターデッキを落とす
                GMScript.FieldCountCheck(thisnamenum); // カードデータ初期化
                flg_Act = false;
                GMScript.CardDrag_flgActCheck();    // フラグアクトチェック
            }
        }

    }

    // --------------------------------------------------------------------------------
    // Card_Revival()
    // カード復活処理
    // --------------------------------------------------------------------------------
    public void Card_Revival()
    {
        GMScript.DeckMasterDisabledFunc(true);
        GameObject obj = GameObject.Find("DeckMaster");
        Transform tra = obj.transform.Find("card").gameObject.transform;
        // デッキマスターの位置を取得
        Vector3 tmp = tra.position;
        transform.position = new Vector3(tmp.x, tmp.y, 0); // 位置を移動
        // 縮小されたサイズを元に戻す
        tmp = tra.localScale;
        transform.localScale = new Vector3(tmp.x, tmp.y, 0); // サイズを元に戻す

        Hanamaru.enabled = false; // 花丸マークdisabled
        Batu.enabled = false; // 罰マークdisabled
        flg_Help = false;   // ヘルプフラグ初期化

        // カードが元の位置に戻る処理
        colflg = false;
        pointerflg = false;
        flg_EnableMove = true;  // 操作フラグ許可

    }

    // --------------------------------------------------------------------------------
    // Card_Reduct_Order()
    // カード縮小命令
    // --------------------------------------------------------------------------------
    public void Card_Reduct_Order()
    {
        flg_Reduction = true;
        flg_Act = true;
    }
    // --------------------------------------------------------------------------------
    // Getcolflg()
    // カード衝突判定取得
    // --------------------------------------------------------------------------------
    public bool Getcolflg()
    {
        return colflg;
    }


}