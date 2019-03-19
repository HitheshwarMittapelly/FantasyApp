using System;
using System.Collections;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyApp {
	public class RegisterScreenHandler : ScreenHandler {
		public InputField email;
		public InputField displayName;
		public InputField password;
		public InputField confirmPass;
		public GameObject signinUpWindow;

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
			signinUpWindow.SetActive(false);
		}
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void OnRegisterButtonClick() {
			string message;
			bool isValid = CheckIfValidEmail(email.text);
			if (!isValid) {
				message = "Badly formatted email";
				Debug.Log(message);
				return;
			}
			if (string.IsNullOrEmpty(displayName.text)) {
				message = ("Please enter a valid Name");
				Debug.Log(message);
				return;
			}
			if (string.IsNullOrEmpty(password.text)) {
				message = ("Please enter a valid password");
				Debug.Log(message);
				return;
			}
			if (string.IsNullOrEmpty(confirmPass.text)) {
				message = "Confirm password field is not valid";
				Debug.Log(message);
				return;
			}
			if (confirmPass.text != password.text) {
				message = ("Passwords are not the same");
				Debug.Log(message);
				return;
			}
			
			message = "Registered";
			FirebaseTools.Register(email.text,password.text,RegisterCallback);
			Debug.Log(message);
			
			

		}

		private void ShowSigningUp() {
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
				parentMenu.GotoLoginScreen();
			}
			else {
				string str = exception.ToString().Split(new string[] { "Firebase.FirebaseException: " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
				Debug.Log(str);
			}

		}
		private bool CheckIfValidEmail(string email) {
			if (email != null) {
				return Regex.IsMatch(email, MatchEmailPattern);
			}
			else {
				return false;
			}
		}
	}
}

