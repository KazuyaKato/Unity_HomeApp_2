using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using UniRx;



public class CalendarButton : MonoBehaviour {

	public CalendarManager calendar;

	public Button button;

	public Text text;



	/// <summary>マスの日時</summary>

	[HideInInspector]public DateTime dateValue;

	/// <summary>ボタン番号</summary>

	[HideInInspector]public int index;



	// Use this for initialization

	void Start () {

		calendar = FindObjectOfType<CalendarManager>();

		button = GetComponent<Button>();

		text = button.GetComponentInChildren<Text>();

		text.fontSize = 30;

		this.ObserveEveryValueChanged(date => date.dateValue)

			.Subscribe(_ =>

				{

					text.text = dateValue.isDefault() ? "" : dateValue.Day.ToString();

					text.color = GetColorDayOfWeek(dateValue.DayOfWeek);

					if(dateValue == DateTime.Today)

					{

						GetComponent<Image>().enabled = true;

					}

					else

					{

						GetComponent<Image>().enabled = false;

					}

				});

	}



	/// <summary>色を指定</summary>

	Color GetColorDayOfWeek(DayOfWeek dayOfWeek)

	{

		if (dateValue.Month == calendar.current.Month)

		{

			switch (dayOfWeek)

			{

			case DayOfWeek.Saturday:

				return Color.blue;

			case DayOfWeek.Sunday:

				return Color.red;

			default:

				return Color.white;

			}

		}

		else

		{

			return Color.gray;

		}

	}

}



/// <summary>DateTime拡張</summary>

public static class DateTimeExtension

{

	/// <summary>デフォルトの0001/01/01が入る</summary>

	static DateTime Default = new DateTime();

	/// <summary>デフォルト値と比較</summary>

	public static bool isDefault(this DateTime d) { return d.Equals(Default); }

}