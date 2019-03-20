using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FantasyApp;
public static class FirebaseTools  {
	#region static 
	private static readonly DatabaseReference DB;
	private static readonly FirebaseAuth Auth = FirebaseAuth.DefaultInstance;

	static FirebaseTools() {
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			DB = FirebaseDatabase.DefaultInstance.RootReference;
		}
		else {
			Debug.LogWarning("Offline mode enabled.");
		}
	}
	#endregion

	private static string EMAIL_PATH = "users/{0}/info/email";
	private static string DISPLAYNAME_PATH = "users/{0}/info/displayName";
	private static string TEAM_PATH = "users/{0}/info/team";
	public static bool registerComplete;
	

	public static bool loginComplete;
	public static void LogIn(string email, string password, Action<AggregateException> callback = null) {
		SignOut();

		Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if(callback == null) {
				callback(task.Exception);
			}
			if (task.IsFaulted) {
				return;
			}

			WriteEmail();
			loginComplete = true;
		});

	}
	
	public static void Register(string email, string password,Action<AggregateException> callback = null) {
		SignOut();
		Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if (callback != null) {
				callback(task.Exception);
			}

			if (task.IsFaulted) {
				return;
			}

			WriteEmail();
			registerComplete = true;
		});
	}

	public static void SignOut() {
		if (Auth.CurrentUser != null) {
			Auth.SignOut();
		}
	}

	public static void SendResetEmail(string email) {
		Auth.SendPasswordResetEmailAsync(email);
	}

	public static bool IsSignedIn() {
		return Auth.CurrentUser != null;
	}

	public static string GetCurrentUser() {
		if (!EnsureSignedIn()) {
			return "Test";
		}

		return Auth.CurrentUser.UserId.ToString();
	}

	public static string GetCurrentUserEmail() {
		return Auth.CurrentUser.Email.ToString();
	}

	public static string GetCurrentUserDisplayName() {
		return Auth.CurrentUser.DisplayName;
	}

	

	public static void UpdateDisplayName(string displayName, Action<bool> callback = null) {
		if (!EnsureSignedIn()) {
			return;
		}

		UserProfile profile = new Firebase.Auth.UserProfile {
			DisplayName = displayName
		};
		Auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWith(task => {
			bool result = true;
			if (task.IsCanceled) {
				Debug.LogError("UpdateProfileAsync was cancled");
				result = false;
			}
			else
			if (task.IsFaulted) {
				Debug.LogError("UpdateProfileAsync encounterd an Error: " + task.Exception);
				result = false;
			}

			if (callback != null) {
				callback(result);
				Debug.Log("Name changed to "+displayName);
			}

		});
	}

	public static bool EnsureSignedIn() {
		if (Auth.CurrentUser == null) {
			Debug.LogWarning("Firebase: Not signed in.");
			return false;
		}
		return true;
	}

	private static void WriteEmail() {
		
		if (!EnsureSignedIn()) {
			return;
		}
		string path = string.Format(EMAIL_PATH, Auth.CurrentUser.UserId);
		var databaseReference = DB.Child(path);
		databaseReference.SetValueAsync(Auth.CurrentUser.Email);
	}
	public static void WriteDisplayName(string name) {

		if (!EnsureSignedIn()) {
			return;
		}
		string path = string.Format(DISPLAYNAME_PATH, Auth.CurrentUser.UserId);
		var databaseReference = DB.Child(path);
		databaseReference.SetValueAsync(name);
	}


	public static void UpdateTeam(List<PlayerInfo> team) {
		if (!EnsureSignedIn()) {
			return;
		}
		string path = string.Format(TEAM_PATH, Auth.CurrentUser.UserId);
		var databaseReference = DB.Child(path);
		int index = 0;
		foreach (var p in team) {
			
			databaseReference.Child(index.ToString()).SetValueAsync(JsonUtility.ToJson(p));
			index++;
		}
		Debug.Log("Team updated");
	}

	
}
