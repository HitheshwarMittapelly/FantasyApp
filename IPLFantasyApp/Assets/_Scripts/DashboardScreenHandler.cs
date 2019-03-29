using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyApp {
	public class DashboardScreenHandler : ScreenHandler {
		public ScrollRect scrollRect;
		public GameObject scrollContent;
		public GameObject scrollItem;

		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.DashboardScreen;
		}
		public override void OnShow() {
			base.OnShow();
			parentMenu.UpdateDashBoard();
			DestroyAllButtons(scrollContent.transform);
			LoadDashBoardFromDatabase();
			
		}

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}

		private void DestroyAllButtons(Transform target) {
			for (int i = 0; i < target.childCount; i++) {
				GameObject.Destroy(target.GetChild(i).gameObject);
			}
		}

		public void LoadDashBoardFromDatabase() {
			Debug.Log("Generating dashboard entry");
			foreach (var entry in FirebaseTools.dashboardEntries) {
				GenerateScrollItem(entry);
			}
		}

		public void GenerateScrollItem(DashBoardEntry dbEntry) {
			Debug.Log("Generating dashboard entry");
			GameObject obj = Instantiate(scrollItem);
			obj.transform.SetParent(scrollContent.transform, false);
			obj.transform.Find("UserName").gameObject.GetComponent<Text>().text = dbEntry.UserName;
			obj.transform.Find("UserScore").gameObject.GetComponent<Text>().text = dbEntry.TotalScore.ToString();
			if (dbEntry.UserID != null && dbEntry.UserID == FirebaseTools.GetCurrentUser()) {
				obj.GetComponentInChildren<Button>().onClick.AddListener(() => {
					//If current user can click on it to see his score in the prev matches maybe?
				});
			}

		}
	}
}
