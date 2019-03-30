using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace FantasyApp {
	public class SelectedTeamPlayersScreenHandler : ScreenHandler {

		public ScrollRect scrollRect;
		public GameObject scrollContent;
		public GameObject scrollItem;
		public string currentTeam;
		public GameObject errorPanel;
		public CurrentTeamScreenHandler currentTeamScreenHandler;
		private int num = 0;
		// Use this for initialization
		void Start() {

		}
		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.SelectedTeamPlayersScreen;

		}

		public override void OnShow() {
			base.OnShow();
			errorPanel.SetActive(false);
			DestroyAllButtons(scrollContent.transform);
			LoadPlayersFromFile(currentTeam);
		}

		private void DestroyAllButtons(Transform target) {
			for (int i = 0; i < target.childCount; i++) {
				GameObject.Destroy(target.GetChild(i).gameObject);
			}
		}
		// Update is called once per frame
		void Update() {
			
		}

		public void GenerateScrollItem(string id,string name, string price, string type) {
			GameObject obj = Instantiate(scrollItem);
			obj.transform.SetParent(scrollContent.transform, false);
			obj.transform.Find("PlayerName").gameObject.GetComponent<Text>().text = name;
			obj.transform.Find("PlayerPrice").gameObject.GetComponent<Text>().text = price;
			PlayerInfo player = new PlayerInfo(id,name, price, type);
			obj.GetComponentInChildren<Button>().onClick.AddListener(() => {
				ReplacePlayer(player);
				
			});

		}

		public void ReplacePlayer(PlayerInfo player) {
			string message = parentMenu.ReplacePlayerWith(player);
			if (message.ToLower() != "success") {
				errorPanel.gameObject.SetActive(true);
				errorPanel.GetComponentInChildren<Text>().text = message;
				return;
			}
			DestroyAllButtons(scrollContent.transform);
			LoadPlayersFromFile(currentTeam);
		}
		private void LoadPlayersFromFile(string teamName) {

			List<PlayerInfo> oneTeam = new List<PlayerInfo>();
			if (teamName == "RCB") {
				oneTeam = FirebaseTools.rcbTeam;
			}
			else if (teamName == "MI") {
				oneTeam = FirebaseTools.miTeam;
			}
			else if (teamName == "SRH") {
				oneTeam = FirebaseTools.srhTeam;
			}
			else if (teamName == "CSK") {
				oneTeam = FirebaseTools.cskTeam;
			}
			else if (teamName == "KKR") {
				oneTeam = FirebaseTools.kkrTeam;
			}
			else if (teamName == "DC") {
				oneTeam = FirebaseTools.dcTeam;
			}
			else if (teamName == "RR") {
				oneTeam = FirebaseTools.rrTeam;
			}
			else if (teamName == "KXIP") {
				oneTeam = FirebaseTools.kxipTeam;
			}
			else if (teamName == "ALL") {
				oneTeam = FirebaseTools.allTeam;
			}
			else if (teamName == "WK") {
				oneTeam = FirebaseTools.wkTeam;
			}
			else if (teamName == "BWL") {
				oneTeam = FirebaseTools.bwlTeam;
			}
			else if (teamName == "BAT") {
				oneTeam = FirebaseTools.batTeam;

			}
			foreach (var player in oneTeam) {
				if (!parentMenu.currentTeam.Contains(player)) {
					GenerateScrollItem(player.pid,player.playerName,player.price,player.playerType);
				}
			}




		}

		private void UpdateSquad() {

			
		}
	}
}