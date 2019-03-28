using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
	using NotificationServices = UnityEngine.iOS.NotificationServices;
	using LocalNotification = UnityEngine.iOS.LocalNotification;
#endif

using System;

public class LocalNoticeScript : MonoBehaviour {
//public class LocalNoticeScript {

	// Use this for initialization
	void Start () {
		#if UNITY_IOS
			NotificationServices.RegisterForNotifications (
				UnityEngine.iOS.NotificationType.Alert |
				UnityEngine.iOS.NotificationType.Badge |
				UnityEngine.iOS.NotificationType.Sound
			);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setLocalNotification(int hour, int min,string alAct){ // 通知設定
		#if UNITY_IOS
			LocalNotification simpleNotification = new LocalNotification ();
			simpleNotification.alertAction = alAct;
			simpleNotification.alertBody = hour + ":" + (min).ToString("00") + " It's time to stydy!!";
			simpleNotification.applicationIconBadgeNumber = 2;
			DateTime dt = DateTime.Now;
			if (hour < dt.Hour)
				dt = dt.AddDays (1);
			simpleNotification.fireDate = new DateTime (dt.Year, dt.Month, dt.Day, hour, min, 0, DateTimeKind.Local);
			// まだ分からないが通知が２つ来てしまうのはここでリセットをかませればなんとかなるかもしれない
			NotificationServices.ScheduleLocalNotification (simpleNotification);
		#endif
	}

	public void ClearLocalNotification(){
		#if UNITY_IOS
			NotificationServices.ClearLocalNotifications ();
		#endif
	}

	public void CancelAllLocalNotification(){
		#if UNITY_IOS
			NotificationServices.CancelAllLocalNotifications ();
		#endif
	}

}
