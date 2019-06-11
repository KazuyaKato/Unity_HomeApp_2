using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class ChooseModeScript : MonoBehaviour
{
    // SaveDataManager
    SaveDataManager SDM;

    // Start is called before the first frame update
    void Start()
    {
        SDM = GetComponent<SaveDataManager>();  // SaveDataManager
        // セーブデータ読み込み
        string DataWork = PlayerPrefs.GetString(SDM.GetStmpName(), "");
        // セーブデータ分解
        StringReader reader = new StringReader(DataWork);
        // 今日のデータ
        int iyear = System.DateTime.Now.Year;
        int imonth = System.DateTime.Now.Month;
        int iday = System.DateTime.Now.Day;
        string todaysdata = iyear.ToString() + "," + 
                            imonth.ToString() + "," +
                            iday.ToString();
        // 家庭科
        string strHomeDate;
        // 家庭科用フラグ
        bool flg_home = false;
        // データ比較
        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            // 読み込み後
            strHomeDate = todaysdata + ",1";
            if (todaysdata + ",1" == line)
            {
                Image img = GameObject.Find("1_Button_Effect/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",2" == line)
            {
                Image img = GameObject.Find("2_Button_Point/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",3" == line)
            {
                Image img = GameObject.Find("3_Button_Aim/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",4" == line)
            {
                Image img = GameObject.Find("4_Button_Contents/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",5" == line)
            {
                Image img = GameObject.Find("5_Button_Family/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",6" == line)
            {
                Image img = GameObject.Find("6_Button_Life/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",7" == line)
            {
                Image img = GameObject.Find("7_Button_Food/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",8" == line)
            {
                Image img = GameObject.Find("8_Button_Wear/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",9" == line)
            {
                Image img = GameObject.Find("9_Button_Live/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",10" == line)
            {
                Image img = GameObject.Find("10_Button_Consume/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",11" == line)
            {
                Image img = GameObject.Find("11_Button_Guide/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
            if (todaysdata + ",12" == line)
            {
                Image img = GameObject.Find("12_Button_Practice/Image_Hanamaru").
                            GetComponent<Image>();
                img.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
