using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FantasyApp {
	public class HomeScreenHandler : ScreenHandler {
		public Text infoText;
		public GameObject loadingWindow;
		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.HomeScreen;
		}

		public override void OnShow() {
			base.OnShow();
			loadingWindow.SetActive(true);
			StartCoroutine(UpdateText());
		}
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void OnLogoutButtonClick() {
			FirebaseTools.SignOut();
			parentMenu.GotoOpeningScreen();
		}
		private IEnumerator UpdateText() {
			if (!FirebaseTools.EnsureSignedIn()) {
				yield return null;
			}
			Debug.Log(FirebaseTools.GetCurrentUserDisplayName());
			infoText.text = string.Format("Welcome {0}",FirebaseTools.GetCurrentUserDisplayName());
			loadingWindow.SetActive(false);
		}
	}
}

