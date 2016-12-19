﻿using UnityEngine;
using System.Collections;
using System;

public class NotificationManager : MonoBehaviour {

	void Start(){

		RegisterForNotif ();
	}

	void RegisterForNotif(){
		
		UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert| UnityEngine.iOS.NotificationType.Badge |  UnityEngine.iOS.NotificationType.Sound);
	}

	void ScheduleNotification(){

		UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification();
		notif.fireDate = DateTime.Now.AddSeconds (5);
		notif.alertBody = "Come back and play!";
		UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
	}

	void OnApplicationPause (bool isPause){

		if( isPause ){ // App going to background
			#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
			ScheduleNotification ();
			#endif
		}

		else {

			#if UNITY_IOS
			/*
			Debug.Log("Local notification count = " + UnityEngine.iOS.NotificationServices.localNotificationCount);
			if (UnityEngine.iOS.NotificationServices.localNotificationCount > 0) {
				Debug.Log(UnityEngine.iOS.NotificationServices.localNotifications[0].alertBody);
			}
*/
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();

			#endif
		}
	}
}
