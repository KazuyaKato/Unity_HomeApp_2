using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour {

	string SaveDataName = "AuthTheory"; 	// setting Data
	string stmpDataName = "AuthStmp";		// stamp Data
	string AuthRevName = "EPYAuthRevName";	// 著書復習データ
	string TheoRevName = "EPYTheoRevName";	// 人物復習データ
	string AuthComName = "EPYAuthComName";	// 著書自己解説データ
	string TheoComName = "EPYTheoComName";	// 人物自己解説データ

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string GetName(){
		return SaveDataName;
	}

	public string GetStmpName(){
		return stmpDataName;
	}

	public string GetAuthRevName(){
		return AuthRevName;
	}

	public string GetTheoRevName(){
		return TheoRevName;
	}

	public string GetAuthComName(){
		return AuthComName;
	}

	public string GetTheoComName(){
		return TheoComName;
	}
}
