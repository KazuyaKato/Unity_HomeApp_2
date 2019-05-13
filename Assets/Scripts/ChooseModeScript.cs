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
        string strHomeDate = todaysdata + ",0";
        // 家庭科用フラグ
        bool flg_home = false;
        // データ比較
        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            // 読み込み後
            if (strHomeDate == line)
                flg_home = true;
        }
        if (flg_home == true)
        {
            Image img = GameObject.Find("ToHomeButton/Image_Hanamaru").
                        GetComponent<Image>();
            img.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
