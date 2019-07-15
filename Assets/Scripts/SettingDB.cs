using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDB : MonoBehaviour {

	[SerializeField]
	public class SetDB{
		[SerializeField]
	public int Q_num;
	public bool BGMToggle;
	public bool CardInclude;
    public bool SETTING_WORD_DISPLAY;
	public bool Song;
	public bool Chord;
	public bool PushNoticeA;
	public bool PushNoticeB;
	public int HourA;
	public int MinA;
	public int HourB;
	public int MinB;


	public SetDB(){
		Q_num = 5;
		BGMToggle = false;
		CardInclude = true;
        SETTING_WORD_DISPLAY = false;
		Song = true;
		Chord = true;
		PushNoticeA = true;
		PushNoticeB = true;
		HourA = 7;
		MinA = 0;
		HourB = 19;
		MinB = 0;
		}
	}
}
