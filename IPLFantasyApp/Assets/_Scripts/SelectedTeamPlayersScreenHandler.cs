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
			LoadPlayersFromFile(currentTeam);
		}
		// Update is called once per frame
		void Update() {
			if (Input.GetKeyDown(KeyCode.Space)) {
				GenerateScrollItem("Kumara sangakkara " + num, Random.Range(35, 110).ToString() + "k", "Bat");
				num++;
			}
		}

		public void GenerateScrollItem(string name, string price, string type) {
			GameObject obj = Instantiate(scrollItem);
			obj.transform.SetParent(scrollContent.transform, false);
			obj.transform.Find("PlayerName").gameObject.GetComponent<Text>().text = name;
			obj.transform.Find("PlayerPrice").gameObject.GetComponent<Text>().text = price;
			PlayerInfo player = new PlayerInfo(name, price, type);
			obj.GetComponentInChildren<Button>().onClick.AddListener(() => {
				parentMenu.AddToCurrentTeam(player);
			});

		}

		private void LoadPlayersFromFile(string currentTeam) {

			string directory = Directory.GetCurrentDirectory();
			
			string path = string.Format(directory + "\\Assets\\PlayerData\\{0}.csv", currentTeam);
			StreamReader streamReader = new StreamReader(path);
			bool endOfFile = false;
			bool isFirst = true;
			while (!endOfFile) {
				string data_string = streamReader.ReadLine();
				if(data_string == null) {
					endOfFile = true;
					break;
				}
				var data_values = data_string.Split(',');
				if (isFirst) {
					isFirst = false;
					continue;
				}
				Debug.Log(data_values[0].ToString() + " " + data_values[1].ToString() + " " + data_values[2].ToString());
				GenerateScrollItem(data_values[0].ToString(), data_values[1].ToString(), data_values[2].ToString());
			}

			
			
		}
	}
}