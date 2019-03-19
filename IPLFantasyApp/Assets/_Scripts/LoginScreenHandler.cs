using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FantasyApp {
	public class LoginScreenHandler : ScreenHandler {
		public InputField emailField;
		
		public InputField passwordField;
		public GameObject loggingInWindow;
		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.OpeningScreen;
		}
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void OnLoginButtonClick() {
			string email = emailField.text;
			string password = passwordField.text;

			
			FirebaseTools.LogIn(email, password, LoginCallback);
			ClearInputFields();
		}

		private void LoginCallback(AggregateException exception) {
			if(exception == null) {
				Debug.Log("Login Success");
				parentMenu.GotoHomeScreen();
			}
			else {
				string str = exception.ToString().Split(new string[] { "Firebase.FirebaseException: " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
				Debug.Log(str);
			}
		}
		private void ShowLoggingIn() {
			ClearInputFields();
			loggingInWindow.SetActive(true);
		}
		private void ClearInputFields() {
			InputField[] fields = GetComponentsInChildren<InputField>();
			foreach (var field in fields)
				field.text = "";
			
		}
	}
}

