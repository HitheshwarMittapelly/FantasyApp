using System;
using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace FantasyApp {
	public class RegisterScreenHandler : ScreenHandler {
		public InputField email;
		public InputField displayName;
		public InputField password;
		public InputField confirmPass;
		public GameObject signinUpWindow;
		public Button backButton;
		private string userName;
		private bool checkForRegisterComplete;
		private bool signUpFailed;
		private string signUpFailReason;
		const string MatchEmailPattern =
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
			+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
			  + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
			+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.RegisterScreen;
		}

		public override void OnShow() {
			base.OnShow();
			ClearInputFields();
			userName = null;
			signinUpWindow.SetActive(false);

			signinUpWindow.transform.SetAsLastSibling();
		}
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
			if (FirebaseTools.registerComplete && checkForRegisterComplete) {
				checkForRegisterComplete = false;
				if (signUpFailed) {
					signinUpWindow.GetComponentInChildren<Text>().text = signUpFailReason;
					backButton.transform.SetAsLastSibling();
					return;
				}
				UnityMainThreadDispatcher.Instance().Enqueue(CheckIfCanChangeScreen());
			}
		}

		public void OnRegisterButtonClick() {
			string message;
			bool isValid = FirebaseTools.CheckIfValidEmail(email.text);
			if (!isValid) {
				message = "Badly formatted email";
				Debug.Log(message);
				//return;
			}
			if (string.IsNullOrEmpty(displayName.text)) {
				message = ("Please enter a valid Name");
				Debug.Log(message);
				//return;
			}
			if (string.IsNullOrEmpty(password.text)) {
				message = ("Please enter a valid password");
				Debug.Log(message);
				//return;
			}
			if (string.IsNullOrEmpty(confirmPass.text)) {
				message = "Confirm password field is not valid";
				Debug.Log(message);
				//return;
			}
			if (confirmPass.text != password.text) {
				message = ("Passwords are not the same");
				Debug.Log(message);
				//return;
			}
			
			message = "Registered";
			if(message!= "Registered") {
				signinUpWindow.SetActive(true);
				signinUpWindow.GetComponentInChildren<Text>().text = message;
				//backButton.transform.SetAsLastSibling();
				return;
			}
			userName = displayName.text;
			UnityMainThreadDispatcher.Instance().Enqueue(ShowSigningUp());
			FirebaseTools.Register(email.text,password.text,RegisterCallback);
			checkForRegisterComplete = true;
			Debug.Log(message);
			
			

		}

		private IEnumerator ShowSigningUp() {
			if (FirebaseTools.registerComplete) {
				yield return null;
			}
			ClearInputFields();
			signinUpWindow.SetActive(true);
		}

		private void ClearInputFields() {
			InputField[] fields = GetComponentsInChildren<InputField>();
			foreach (var field in fields)
				field.text = "";

		}
		private void RegisterCallback(AggregateException exception) {
			if(exception == null) {
				Debug.Log("Register success");
				
			}
			else {
				
				string str = exception.ToString().Split(new string[] { "Firebase.FirebaseException: " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
				Debug.Log(str);
				FirebaseTools.registerComplete = true;
				signUpFailed = true;
				var stringsObj = str.Split('.');
				signUpFailReason = stringsObj[0] + stringsObj[1];
			}

		}

		private IEnumerator CheckIfCanChangeScreen() {
			if (!FirebaseTools.registerComplete) {
				yield return null;
			}
			parentMenu.GotoLoginScreen();
			FirebaseTools.UpdateDisplayName(userName, (result) => { });
			FirebaseTools.WriteDisplayName(userName);
			FirebaseTools.WriteSubstitutes(80);
		}
		
	}
}

