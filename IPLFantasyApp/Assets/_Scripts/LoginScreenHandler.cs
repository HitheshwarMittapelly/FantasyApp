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
		public Button backButton;
		
		private string message;
		private bool checkForLoginComplete;
		private bool loginFailed;
		private string loginFailReason;
		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.OpeningScreen;
		}
		public override void OnShow() {
			base.OnShow();
			ClearInputFields();
			message = null;
			loggingInWindow.SetActive(false);
			loggingInWindow.transform.SetAsLastSibling();

		}

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
			if(FirebaseTools.loginComplete && checkForLoginComplete) {
				checkForLoginComplete = false;
				if (loginFailed) {
					loggingInWindow.GetComponentInChildren<Text>().text = loginFailReason;
					backButton.transform.SetAsLastSibling();
					return;
				}
				UnityMainThreadDispatcher.Instance().Enqueue(CheckIfCanChangeScreen());
			}
		}

		public void OnLoginButtonClick() {
			string email = emailField.text;
			if (string.IsNullOrEmpty(email)) {
				email = "test@test.com";
			}
			
			string password = passwordField.text;
			//bool isValid = FirebaseTools.CheckIfValidEmail(email);
			if (string.IsNullOrEmpty(password)) {
				password = "password";
			}
			//if (!isValid) {
			//	return;
			//}
			UnityMainThreadDispatcher.Instance().Enqueue(ShowLoggingIn());

			FirebaseTools.LogIn(email, password, LoginCallback);
			
			checkForLoginComplete = true;
			ClearInputFields();
		}

		private void LoginCallback(AggregateException exception) {
			if(exception == null) {
				Debug.Log("Login Success");
				message = "success";
			}
			else {
				message = "login failed";
				string str = exception.ToString().Split(new string[] { "Firebase.FirebaseException: " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
				Debug.Log(str);
				FirebaseTools.loginComplete = true;
				loginFailed = true;
				var stringsObj = str.Split('.');
				loginFailReason = stringsObj[0] + stringsObj[1];
			}
		}
		private IEnumerator ShowLoggingIn() {
			if (FirebaseTools.loginComplete)
				yield return null;
			loggingInWindow.SetActive(true);
		}
		private IEnumerator CheckIfCanChangeScreen() {
			if (!FirebaseTools.loginComplete) {
			
				yield return null;
			}
			
			FirebaseTools.GetSubsLeft(result => { });
			parentMenu.GotoHomeScreen();
		}
		
		private void ClearInputFields() {
			InputField[] fields = GetComponentsInChildren<InputField>();
			foreach (var field in fields)
				field.text = "";
			
		}
	}
}

