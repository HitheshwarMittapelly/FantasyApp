using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyApp {

	public class CurrentTeamScreenHandler : ScreenHandler {

		public ScrollRect scrollRect;
		public GameObject scrollContent;
		public GameObject scrollItem;

		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.SelectedTeamPlayersScreen;
		}

		public override void OnShow() {
			base.OnShow();
			LoadCurrentTeam();
		}

		// Update is called once per frame
		void Update() {

		}

		public void GenerateScrollItem(string name, string price, string type) {
			GameObject obj = Instantiate(scrollItem);
			obj.transform.SetParent(scrollContent.transform, false);
			obj.transform.Find("PlayerName").gameObject.GetComponent<Text>().text = name;
			obj.transform.Find("PlayerPrice").gameObject.GetComponent<Text>().text = price;
			

		}

		public void LoadCurrentTeam() {
			foreach(var player in parentMenu.currentTeam) {
				GenerateScrollItem(player.playerName, player.price, player.playerType);
			}
		}
	}
}
