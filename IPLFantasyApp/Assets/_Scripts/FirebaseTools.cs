using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private static bool EnsureSignedIn() {
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
}
