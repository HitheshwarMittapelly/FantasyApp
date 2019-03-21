using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyApp {

	public class CurrentTeamScreenHandler : ScreenHandler {

		public ScrollRect scrollRect;
		public GameObject scrollContent;
		public GameObject scrollItem;
		public Text batsmenCountText;
		public Text bowlerCountText;
		public Text allRounderCountText;
		public Text wicketKeeperCountText;
		public Text subsLeftText;
		public PlayerInfo selectedPlayer;

		
		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.SelectedTeamPlayersScreen;
		}

		public override void OnShow() {
			base.OnShow();
			selectedPlayer.playerName = "dummy";
			DestroyAllButtons(scrollContent.transform);
			CalculateSubs();
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
				RemoveFromList(player);
			});

		}

		public void RemoveFromList(PlayerInfo player) {
			selectedPlayer = player;
			//parentMenu.RemoveFromCurrentTeam(player);
			parentMenu.GotoTeamSelectionScreen();
		}
		public void LoadCurrentTeam() {
			
			batsmenCountText.text = "BAT : " + parentMenu.numOfBatsmen;
			bowlerCountText.text = "BWL : " + parentMenu.numOfBowlers;
			allRounderCountText.text = "ALL : " + parentMenu.numOfAllRounders;
			wicketKeeperCountText.text = "WK : " + parentMenu.numOfWicketKeepers;
			foreach (var player in parentMenu.currentTeam) {
				GenerateScrollItem(player);
			}
		}
		private void CalculateSubs() {
			int noChanges = 0;
			foreach(var playerPID in FirebaseTools.serverTeam) {
				foreach(var player in parentMenu.currentTeam) {
					if(playerPID == player.pid) {
						noChanges++;
					}
				}
			}
			int changes = FirebaseTools.subsLeft - (11 - noChanges);
			
			subsLeftText.text = "Subs left : " + changes;
		}
		public void ClearButtonClick() {
			parentMenu.ClearAllPlayersFromCurrentTeam();
			DestroyAllButtons(scrollContent.transform);
			LoadCurrentTeam();
			CalculateSubs();
		}
		public void SaveButtonClick() {

			parentMenu.OverWriteCurrentSaveTeam();
			
		}

		public void DiscardButtonClick() {
			parentMenu.DiscardAllPlayers();
			DestroyAllButtons(scrollContent.transform);
			LoadCurrentTeam();
			CalculateSubs();
		}
	}
}
