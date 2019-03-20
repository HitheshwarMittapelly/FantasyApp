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
			DestroyAllButtons(scrollContent.transform);
			LoadCurrentTeam();
		}

		private void DestroyAllButtons(Transform target) {
			for (int i = 0; i < target.childCount; i++) {
				GameObject.Destroy(target.GetChild(i).gameObject);
			}
		}
		// Update is called once per frame
		void Update() {

		}

		public void GenerateScrollItem(PlayerInfo player) {
			GameObject obj = Instantiate(scrollItem);
			obj.transform.SetParent(scrollContent.transform, false);
			obj.transform.Find("PlayerName").gameObject.GetComponent<Text>().text = player.playerName;
			obj.transform.Find("PlayerPrice").gameObject.GetComponent<Text>().text = player.price;
			obj.GetComponentInChildren<Button>().onClick.AddListener(() => {
				parentMenu.RemoveFromCurrentTeam(player);
			});

		}

		public void LoadCurrentTeam() {
			foreach(var player in parentMenu.currentTeam) {
				GenerateScrollItem(player);
			}
		}

		public void ClearButtonClick() {
			parentMenu.ClearAllPlayersFromCurrentTeam();
			DestroyAllButtons(scrollContent.transform);
			LoadCurrentTeam();
		}
		public void SaveButtonClick() {
			parentMenu.OverWriteCurrentSaveTeam();
			
		}

		public void DiscardButtonClick() {
			parentMenu.DiscardAllPlayers();
			DestroyAllButtons(scrollContent.transform);
			LoadCurrentTeam();
		}
	}
}
