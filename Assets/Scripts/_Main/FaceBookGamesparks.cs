﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using GameSparks.Api.Requests;
using Facebook.MiniJSON;

public class FaceBookGamesparks : MonoBehaviour
{
	public static FaceBookGamesparks instance = null;

	void Awake (){
		instance = this;
	}

	void Start (){
		FB.Init (null, OnHideUnity);
	}

	void OnHideUnity (bool isGameShown){
		Debug.Log ("Is game showing? " + isGameShown);
	}

	public void CallFBLogin (){
		var perms = new List<string> (){ "public_profile", "email", "user_friends" };
		FB.LogInWithReadPermissions (perms, AuthCallback);
	}

	void LoginCallback2(ILoginResult result){

		IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.ToString()) as IDictionary;
		string fbname = dict["first_name"].ToString();
		print("your name is: " + fbname);
		
	}

	private void AuthCallback (ILoginResult result){
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log (aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log (perm);
			}

			Debug.Log (result.RawResult);

			FB.API("/me?fields=name,email", HttpMethod.GET, graphResult =>
				{
					if(result.Error == null){

						Dictionary<string, object> poodle = (Dictionary<string,object>)graphResult.ResultDictionary;

						string value = "";
						if (poodle.TryGetValue("name",out value)){
							string name = (string)poodle["name"];
							GameManager.userName = name;
						}
						if(poodle.TryGetValue("email",out value)){
							string email = (string)poodle["email"];
							GameManager.email = email;
						}
						GameSparksLogin (aToken);
					}
				});
			
		} else {
			Debug.Log ("User cancelled login");
		}
	}


	void GameSparksLogin (AccessToken token){
		new FacebookConnectRequest ().SetAccessToken (AccessToken.CurrentAccessToken.TokenString).Send ((response) => {
			//If our response has errors we can check what went wrong
			if (response.HasErrors) {
				Debug.Log ("Something failed when connecting with Facebook " + response.Errors);
			} else {
				if ((bool)response.NewPlayer) {
					//GameManager.instance.FirstPlay ();
					SignupUI.instance.ShowCheckEmailMenu();
				} else {
					GameManager.instance.CurrentState (GameStates.Mainmenu);
				}
			}
		});
	}

	public void Share (){

		Uri link = new Uri ("http://www.theverge.com/");

		FB.ShareLink (contentTitle: "Mazoin", contentDescription: "Temporary Mazoin description", contentURL: link);
	}

}


