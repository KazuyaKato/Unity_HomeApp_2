using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyScroll : MonoBehaviour,IEndDragHandler {
	public int cnt = 0;

	private List<int> intList;
	public int thisTime;
	// Use this for initialization
	void Start () {
		GameObject _child = transform.Find ("Content").gameObject;
		GameObject _sonimage;
		UnityEngine.UI.Text textComponent;
		intList = new List<int> ();
		foreach(Transform _son in _child.transform){
			_sonimage = _son.transform.Find ("Text").gameObject;
			textComponent = _sonimage.GetComponent<UnityEngine.UI.Text> ();
			intList.Add(int.Parse(textComponent.text));
			cnt++;
		}
		GetSetData();
	}
	
	public void OnEndDrag(PointerEventData data){
		GetSetData();
	}

	void GetSetData(){
		float max = cnt - 1;
		float pos = Mathf.Clamp(this.GetComponent<ScrollRect>().verticalNormalizedPosition,0,1);
		int num = Mathf.RoundToInt (max - max * pos);
		thisTime = intList [num]; // 現在値を保存
		this.GetComponent<ScrollRect> ().verticalNormalizedPosition = Mathf.RoundToInt (max * pos) / max;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
