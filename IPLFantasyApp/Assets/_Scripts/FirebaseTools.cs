using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Text.RegularExpressions;
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
	const string MatchEmailPattern =
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
			+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
			  + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
			+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

	private static string EMAIL_PATH = "users/{0}/info/email";
	private static string DISPLAYNAME_PATH = "users/{0}/info/displayName";
	private static string SUBS_PATH = "users/{0}/subs";
	private static string TEAM_PATH = "users/{0}/team";
	private static string SQUAD_PATH = "Squads/{0}";
	public static bool registerComplete;
	
	public static int subsLeft;
	public static List<String> serverTeam;
	public static List<PlayerInfo> rcbTeam;
	public static List<PlayerInfo> srhTeam;
	public static List<PlayerInfo> cskTeam;
	public static List<PlayerInfo> miTeam;
	public static List<PlayerInfo> kkrTeam;
	public static List<PlayerInfo> dcTeam;
	public static List<PlayerInfo> rrTeam;
	public static List<PlayerInfo> kxipTeam;
	public static List<PlayerInfo> allTeam;
	public static List<PlayerInfo> batTeam;
	public static List<PlayerInfo> wkTeam;
	public static List<PlayerInfo> bwlTeam;
	public static bool teamUpdateComplete;
	public static bool loginComplete;
	public static bool squadUpdateComplete;
	
	public static void LogIn(string email, string password, Action<AggregateException> callback = null) {
		SignOut();

		Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if(callback != null) {
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
				Debug.Log("Faulted");
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

	public static void WriteSubstitutes(int num) {

		if (!EnsureSignedIn()) {
			return;
		}
		string path = string.Format(SUBS_PATH, Auth.CurrentUser.UserId);
		var databaseReference = DB.Child(path);
		databaseReference.SetValueAsync(num);
	}


	public static void UpdateTeam(List<PlayerInfo> team) {
		if (!EnsureSignedIn()) {
			return;
		}
		string path = string.Format(TEAM_PATH, Auth.CurrentUser.UserId);
		var databaseReference = DB.Child(path);
		int index = 0;
		foreach (var p in team) {
			string obj = p.pid ;
			databaseReference.Child(index.ToString()).SetValueAsync((obj));
			index++;
		}
		Debug.Log("Team updated");
	}

	public static void GetSubsLeft(Action<int> callback) {
		if (!EnsureSignedIn()) {
			return;
		}

		string path = string.Format(SUBS_PATH, Auth.CurrentUser.UserId);
		var databaseReference = DB.Child(path);
		databaseReference.GetValueAsync().ContinueWith((task) => {
			if (task.IsFaulted) {
				Debug.LogError("Getting Data From User Team Failed Due To: " + task.Exception);
			}

			if (task.IsCompleted) {
				if (task.Result.Exists) {
					DataSnapshot dataSnapshot = task.Result;

					
					subsLeft = System.Int32.Parse(dataSnapshot.Value.ToString());
					
					callback((int)dataSnapshot.Value);
					
				}
				else {
					callback(-1);
				}
			}
		});
	}
	public static void GetTeamIfExists(Action<AggregateException> callback) {
		if (!EnsureSignedIn()) {
			return;
		}

		string path = string.Format(TEAM_PATH, Auth.CurrentUser.UserId);
		var databaseReference = DB.Child(path);
		databaseReference.GetValueAsync().ContinueWith((task) => {
			if (task.IsFaulted) {
				Debug.LogError("Getting Data From User Team Failed Due To: " + task.Exception);
			}

			if (task.IsCompleted) {
				if (task.Result.Exists) {
					DataSnapshot dataSnapshot = task.Result;
					serverTeam = new List<String>();
					foreach(var child in dataSnapshot.Children) {
						serverTeam.Add((child.Value.ToString()));
					}
					teamUpdateComplete = true;
				}
				else {
					callback(null);
				}
			}
		});
	}
	public static void GetSquadFromDatabase(string teamName,Action<AggregateException> callback) {
		//if (!EnsureSignedIn()) {
		//	return ;
		//}
		
		List<PlayerInfo> oneTeam = new List<PlayerInfo>();
		string path = string.Format(SQUAD_PATH,teamName);
		var databaseReference = DB.Child(path);
		databaseReference.GetValueAsync().ContinueWith((task) => {
			if (task.IsFaulted) {
				Debug.LogError("Getting Data From Squads Failed Due To: " + task.Exception);
			}

			if (task.IsCompleted) {
				if (task.Result.Exists) {
					DataSnapshot dataSnapshot = task.Result;
					
					foreach (var child in dataSnapshot.Children) {
						PlayerInfo player = new PlayerInfo(child.Child("PID").Value.ToString(), child.Child("PLAYERNAME").Value.ToString(),
								child.Child("PRICE").Value.ToString(), child.Child("TYPE").Value.ToString());
						oneTeam.Add(player);
					}
					
					if(teamName == "RCB") {
						rcbTeam = new List<PlayerInfo>();
						rcbTeam = oneTeam;
					}
					else if (teamName == "MI") {
						miTeam = new List<PlayerInfo>();
						miTeam = oneTeam;
					}
					else if (teamName == "SRH") {
						srhTeam = new List<PlayerInfo>();
						srhTeam = oneTeam;
					}
					else if (teamName == "CSK") {
						cskTeam = new List<PlayerInfo>();
						cskTeam = oneTeam;
					}
					else if (teamName == "KKR") {
						kkrTeam = new List<PlayerInfo>();
						kkrTeam = oneTeam;
					}
					else if (teamName == "DC") {
						dcTeam = new List<PlayerInfo>();
						dcTeam = oneTeam;
					}
					else if (teamName == "RR") {
						rrTeam = new List<PlayerInfo>();
						rrTeam = oneTeam;
					}
					else if (teamName == "KXIP") {
						kxipTeam = new List<PlayerInfo>();
						kxipTeam = oneTeam;
					}
					else if (teamName == "ALL") {
						allTeam = new List<PlayerInfo>();
						allTeam = oneTeam;
					}
					else if (teamName == "WK") {
						wkTeam = new List<PlayerInfo>();
						wkTeam = oneTeam;
					}
					else if (teamName == "BWL") {
						bwlTeam = new List<PlayerInfo>();
						bwlTeam = oneTeam;
					}
					else if (teamName == "BAT") {
						batTeam = new List<PlayerInfo>();
						batTeam = oneTeam;
						Debug.Log("squad update completed in firebase");
						squadUpdateComplete = true;
					}

				}
				else {
					callback(null);
				}
			}
		});
	}
	public static bool CheckIfValidEmail(string email) {
		if (email != null) {
			return Regex.IsMatch(email, MatchEmailPattern);
		}
		else {
			return false;
		}
	}


}
