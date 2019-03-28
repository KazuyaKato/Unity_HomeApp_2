using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCube : MonoBehaviour
{
    public bool flg_Put; // カード置きフラグ

    public GameObject GameMain;
    GameMainScript GMScript;    // ゲームメインスクリプト
     public int Card_Count = 0;
    bool[] blArray;  // 置かれたオブジェクト名を保持するArray

    // Start is called before the first frame update
    void Start()
    {
        GameMain = GameObject.Find("GameMain");
        GMScript = GameMain.GetComponent<GameMainScript>();

        // カードオブジェクト管理用
        int cnt = 0;
        GameObject Deck = GameObject.Find("Deck");
        foreach(Transform child in Deck.transform)
        {
            foreach(Transform son in child.transform)
            {
                cnt++;
            }
        }
        blArray = new bool[cnt];
        for(int i = 0; i < cnt; i++)
        {
            blArray[i] = false;
        }
        //---

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Card_Count = " + Card_Count);
    }

    // --------------------------------------------------------------------------------
    // OnTriggerEnter2D
    // 衝突した際の処理
    // --------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Common.Left(collision.gameObject.name, 5) == "card_")
        {
            if (flg_Put == false)
                flg_Put = true;
            GMScript.Changeflg_Put(true);
            Card_Count++;

            // 衝突したカード情報を保存
            //            int i = int.Parse(collision.gameObject.name.Substring(5));
            //            blArray[i] = true;
            blArray[int.Parse(collision.gameObject.name.Substring(5))] = true;
            Debug.Log("配列状態出力  0 = " + blArray[0] + "  1 = " + blArray[1] +
                "  2 = " + blArray[2] + "  3 = " + blArray[3] + "  4 = " + blArray[4]);
        }
    }

    // --------------------------------------------------------------------------------
    // OnTriggerExit2D
    // 衝突から離れた処理
    // --------------------------------------------------------------------------------
    private void OnTriggerExit2D(Collider2D collision)
    { 
        if (Common.Left(collision.gameObject.name, 5) == "card_")
        {
            // 衝突判定を
            Debug.Log("Exit2D");
            Card_Count--;
            if (Card_Count < 1)
                flg_Put = false;

            // 衝突から抜けたカード情報を反映
            blArray[int.Parse(collision.gameObject.name.Substring(5))] = false;
            Debug.Log("配列状態出力  0 = " + blArray[0] + "  1 = " + blArray[1] +
    "  2 = " + blArray[2] + "  3 = " + blArray[3] + "  4 = " + blArray[4]);

        }
    }

    // --------------------------------------------------------------------------------
    // Card_Put
    // カードが置かれているかの判定
    // --------------------------------------------------------------------------------
    public bool Card_Put(int _num)
    {
        return blArray[_num];
    }
}
