using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour
{
    public Text Text_SC;    // 詳細欄
    public GameObject GameMain;
    GameMainScript GMScript;    // ゲームメインスクリプト

    float posx;
    float posy;

    bool colflg = false; // 衝突用フラグ
    bool pointerflg = false; // 持っているかどうかのフラグ
    public bool flg_Reduction = false; //  縮小フラグ

    // --------------------------------------------------------------------------------
    // OnDrag
    // ドラッグ処理
    // --------------------------------------------------------------------------------
    public void OnDrag() // ドラッグ処理
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

    // --------------------------------------------------------------------------------
    // PointerDown
    // 振れた時の処理。詳細欄に内容が表示される
    // --------------------------------------------------------------------------------
    public void PointerDown()   // 振れた時の処理。詳細欄に内容が表示される
    {
        Text_SC.text = this.gameObject.GetComponentInChildren<Text>().text;
        pointerflg = true; // 持っている
        GMScript.Changeflg_CardBring(true); // 持ってますよフラグオン
        Debug.Log("Down");
    }

    // --------------------------------------------------------------------------------
    // PointerUp
    // 手を離した時の処理。
    // --------------------------------------------------------------------------------
    public void PointerUp() // 手を離した時の処理。
    {
        Debug.Log("PointerUp");
        GMScript.Changeflg_CardBring(false); // 持ってますよフラグoff
        if (colflg == false)
        {
            InitPos();  // 初期位置へ戻す
            pointerflg = false; // 持っていない
            GMScript.Changeflg_Put(false); // 置きflgをoffへ？

        }
        else
        {
    //        GameMainScript script;
  //          script = GameMain.GetComponent<GameMainScript>();
//            script.JudgeQuestion(this.gameObject.name);
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
    }

    // Update is called once per frame
    void Update()
    {
        if(colflg == false && pointerflg == false)
            transform.position = Vector3.Lerp(new Vector3(
            this.transform.position.x,this.transform.position.y, 
                this.transform.position.z),
            new Vector3(posx, posy, this.transform.position.z),
        Time.deltaTime * 4);
        if(flg_Reduction　== true)
        {
            Transform tra = this.transform; // トランスフォーム情報を保存
            Vector3 tmp = tra.position;
            if (tmp.y > 0.001f)
                transform.localScale -= new Vector3(0.0001f, 0.0001f, 0);
            else
            {
                flg_Reduction = false;
                Card_Revival();
            }
        }
    }

    // --------------------------------------------------------------------------------
    // Card_Revival()
    // カード復活処理
    // --------------------------------------------------------------------------------
    void Card_Revival()
    {
        Transform tra = GameObject.Find("DeckMaster").transform;
        // デッキマスターの位置を取得
        Vector3 tmp = tra.position;
        transform.position = new Vector3(tmp.x, tmp.y, 0); // 位置を移動
        // 縮小されたサイズを元に戻す
        tmp = tra.localScale;
        transform.localScale = new Vector3(tmp.x, tmp.y, 0); // サイズを元に戻す
    }

    // --------------------------------------------------------------------------------
    // Card_MoveInitPos()
    // カード元の位置戻る処理
    // --------------------------------------------------------------------------------

}