using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour {

    string stmpDataName = "AuthStmp";		// stamp Data
    string ReviewName = "ReviewName";   // 復習データ

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string GetStmpName(){
        return stmpDataName;
    }

    public string GetReviewName(){
		return ReviewName;
	}
}
