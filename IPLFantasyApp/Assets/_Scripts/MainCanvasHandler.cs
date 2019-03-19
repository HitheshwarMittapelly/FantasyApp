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

		private ScreenType CurrentScreen;
		private int lastScreenIndex;
		public static MainCanvasHandler GetObject() {
			MainCanvasHandler theObjectInQuestion = FindObjectOfType<MainCanvasHandler>() as MainCanvasHandler;
			return theObjectInQuestion;
		}

		public void Awake() {
			foreach(var handler in screenHandlers) {
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

			//Once we have a reference to the current game object we update the current screen to be our target screen
			int currentScreenIndex = (int)targetScreen;

			HideAllScreens();
			screenHandlers[currentScreenIndex].ShowFromLeft();

			//GameObject screenToTransitionTo = Instance.screens[currentScreenIndex];

			CurrentScreen = (ScreenType)currentScreenIndex;

			return true;
		}

		public void HideAllScreens() {
			currentTeamButton.SetActive(false);
			foreach (var handler in screenHandlers) {
				handler.Hide();
			}
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
			currentTeam.Add(newPlayer);
		}
		
		public void RemoveFromCurrentTeam(PlayerInfo player) {
			currentTeam.Remove(player);
		}
	}
}
