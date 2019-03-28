using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MplayMove : MonoBehaviour {

	public GameObject objtxt;
	Text txttxt;
	public GameObject objpnl;

	float panelwidth;
	float speed = 2f;
	float hlfscsz;
	bool openstay = false;
	bool moveflg;

	// Use this for initialization
	void Start () {
		txttxt = objtxt.GetComponent<Text> ();
		posInit ();
		panelwidth = objpnl.GetComponent<RectTransform> ().sizeDelta.x;

	}
	
	// Update is called once per frame
	void Update () {
		if(!openstay)
			TextMove ();
	}

	void posInit(){
		openstay = true;
		Vector3 v3 = objtxt.GetComponent<RectTransform>().anchoredPosition;
		v3.x = 0.0f;
		objtxt.GetComponent<RectTransform>().anchoredPosition = v3;
		Invoke ("openstayfalse", 1.0f);
	}

	void openstayfalse(){
		openstay = false;
	}

	void TextMove(){

		if (moveflg)
			return;

		float txtwidth = txttxt.preferredWidth;

		Vector3 m_pos = objtxt.transform.localPosition;	// txt pos x

		float checkf = txtwidth - panelwidth;

		// m_posが表示上のx座標ではない
		float txtposx = txttxt.GetComponent<RectTransform>().anchoredPosition.x;

		if (txtposx < (checkf * (-1))) {
			moveflg = true;
			Invoke ("FlgMax",1.0f);
			return;
		}

		m_pos.x -= speed;

		objtxt.transform.localPosition = m_pos;	

	}

	void FlgMax(){
		posInit ();
		moveflg = false;	
	}
}
