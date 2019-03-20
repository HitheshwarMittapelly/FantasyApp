using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FantasyApp {
	public enum ScreenType {
		OpeningScreen,
		RegisterScreen,
		LoginScreen,
		HomeScreen,
		DashboardScreen,
		TeamSelectionScreen,
		SelectedTeamPlayersScreen,
		CurrentTeamScreen,
	}

	public struct PlayerInfo{
		public string playerName;
		public string price;
		public string playerType;
		public PlayerInfo(string name, string pprice, string type) {
			playerName = name;
			price = pprice;
			playerType = type;
		}
	}
	public class MainCanvasHandler : MonoBehaviour {
		public List<ScreenHandler> screenHandlers;

		public GameObject currentTeamButton;

		public List<PlayerInfo> currentTeam;

		public List<PlayerInfo> savedInDBTeam;
		private ScreenType CurrentScreen;
		private int lastScreenIndex;
		public static MainCanvasHandler GetObject() {
			MainCanvasHandler theObjectInQuestion = FindObjectOfType<MainCanvasHandler>() as MainCanvasHandler;
			return theObjectInQuestion;
		}

		public void Awake() {
			foreach (var handler in screenHandlers) {
				handler.Initialize(this);
			}
			GotoOpeningScreen();
			currentTeam = new List<PlayerInfo>();
		}

		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}

		public bool GoToScreen(ScreenType targetScreen) {
			int prevScreenIndex = (int)CurrentScreen;
			lastScreenIndex = (int)CurrentScreen;
			
			int currentScreenIndex = (int)targetScreen;

			HideAllScreens();
			screenHandlers[currentScreenIndex].ShowFromLeft();


			CurrentScreen = (ScreenType)currentScreenIndex;

			return true;
		}

		public void HideAllScreens() {
			currentTeamButton.SetActive(false);
			foreach (var handler in screenHandlers) {
				handler.Hide();
			}
		}
		public void GotoLastScreen() {
			GoToScreen((ScreenType)lastScreenIndex);
		}
		public void GotoOpeningScreen() {
			GoToScreen(ScreenType.OpeningScreen);
		}
		public void GotoRegisterScreen() {
			GoToScreen(ScreenType.RegisterScreen);
		}
		public void GotoLoginScreen() {
			GoToScreen(ScreenType.LoginScreen);
		}
		public void GotoHomeScreen() {
			
			GoToScreen(ScreenType.HomeScreen);
			currentTeamButton.SetActive(true);
		}
		public void GotoTeamSelectionScreen() {
			
			GoToScreen(ScreenType.TeamSelectionScreen);
			currentTeamButton.SetActive(true);
		}

		public void GotoSelectedTeamPlayersScreen() {
			
			GoToScreen(ScreenType.SelectedTeamPlayersScreen);
			currentTeamButton.SetActive(true);
		}
		public void GotoDashboardScreen() {
			
			GoToScreen(ScreenType.DashboardScreen);
			currentTeamButton.SetActive(true);
		}
		public void GotoCurrentTeamScreen() {
			lastScreenIndex = (int)CurrentScreen;
			currentTeamButton.SetActive(false);
			screenHandlers[(int)ScreenType.CurrentTeamScreen].ShowFromLeft();
		}
		public void GotoPrevScreen() {
			currentTeamButton.SetActive(true);
			screenHandlers[(int)ScreenType.CurrentTeamScreen].Hide();
		}
		public void OpenSelectedTeam(string teamName) {
			GotoSelectedTeamPlayersScreen();
			
		}

		public void AddToCurrentTeam(PlayerInfo newPlayer) {
			if (currentTeam.Count < 11 && !currentTeam.Contains(newPlayer)) {
				currentTeam.Add(newPlayer);
			}
		}
		
		public void RemoveFromCurrentTeam(PlayerInfo player) {
			currentTeam.Remove(player);
		}

		public void OverWriteCurrentSaveTeam() {
			bool result = CheckIfPlayerTypeRulesFollowed();
			if (result) {
				Debug.Log("trying to update");
				FirebaseTools.UpdateTeam(currentTeam);
			}
			else {
				Debug.Log("Result is false");
			}
		}

		public void DiscardAllPlayers() {
			currentTeam.Clear();
		}

		public void ClearAllPlayersFromCurrentTeam() {
			currentTeam.Clear();
		}

		private bool CheckIfPlayerTypeRulesFollowed() {
			int bat = 0, bwl = 0, wk = 0;
			if (currentTeam.Count != 11) {
				return false;
			}
			const string batsmen = "BAT";
			const string bowler = "BWL";
			const string wicketKeeper = "WK";
			foreach(var player in currentTeam) {
				switch (player.playerType.ToUpper()) {
					case batsmen: bat++;
						break;
					case bowler: bwl++;
						break;
					case wicketKeeper: wk++;
						break;
				}

			}
			if (bat >= 2 && bwl > 2 && wk > 0) {
				return true;
			}
			else {
				return false;
			}
		}

	}
}
