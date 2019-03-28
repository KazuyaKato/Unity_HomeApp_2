using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct strData{
	public int year;
	public int month;
	public int day;
	public int meridiem;

	public strData(int year,int month,int day,int meridiem){
		this.year = year;
		this.month = month;
		this.day = day;
		this.meridiem = meridiem;
	}
}
