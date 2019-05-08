using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct strData{
	public int year;
	public int month;
	public int day;
	public int type;

	public strData(int year,int month,int day,int type){
		this.year = year;
		this.month = month;
		this.day = day;
		this.type = type;
	}
}
