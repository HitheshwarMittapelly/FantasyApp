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

		private bool checkForLoginComplete;
		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.OpeningScreen;
		}
		public override void OnShow() {
			base.OnShow();
			ClearInputFields();
			loggingInWindow.SetActive(false);

		}

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
			if(FirebaseTools.loginComplete && checkForLoginComplete) {
				checkForLoginComplete = false;
				UnityMainThreadDispatcher.Instance().Enqueue(CheckIfCanChangeScreen());
			}
		}

		public void OnLoginButtonClick() {
			string email = emailField.text;
			string password = passwordField.text;

			UnityMainThreadDispatcher.Instance().Enqueue(ShowLoggingIn());

			FirebaseTools.LogIn(email, password, LoginCallback);
			checkForLoginComplete = true;
			ClearInputFields();
		}

		private void LoginCallback(AggregateException exception) {
			if(exception == null) {
				Debug.Log("Login Success");
				
			}
			else {
				string str = exception.ToString().Split(new string[] { "Firebase.FirebaseException: " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
				Debug.Log(str);
			}
		}
		private IEnumerator ShowLoggingIn() {
			if (FirebaseTools.loginComplete)
				yield return null;
			loggingInWindow.SetActive(true);
		}
		private IEnumerator CheckIfCanChangeScreen() {
			if (!FirebaseTools.registerComplete) {
			
				yield return null;
			}
			parentMenu.GotoHomeScreen();
		}
		
		private void ClearInputFields() {
			InputField[] fields = GetComponentsInChildren<InputField>();
			foreach (var field in fields)
				field.text = "";
			
		}
	}
}

