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
		CurrentTeamScreen,
		TeamSelectionScreen,
		SelectedTeamPlayersScreen,
		
	}

	public struct PlayerInfo{
		public string playerName;
		public string price;
		public string playerType;
		public string pid;
		public PlayerInfo(string id, string name, string pprice, string type) {
			pid = id;
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
		[HideInInspector]
		public int numOfBowlers;
		[HideInInspector]
		public int numOfBatsmen;
		[HideInInspector]
		public int numOfAllRounders;
		[HideInInspector]
		public int numOfWicketKeepers;
		[HideInInspector]
		public int currentBudget;
		
		private ScreenType CurrentScreen;
		private int lastScreenIndex;
		private bool checkForTeamUpdate;
		private bool checkForSquadUpdate;
		private int gameBudget = 1000;
		
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
			UpdateAllSquads();
		}

		// Update is called once per frame
		void Update() {
			if(checkForTeamUpdate && FirebaseTools.teamUpdateComplete) {
				UnityMainThreadDispatcher.Instance().Enqueue(UpdateFromServerTeam());
				checkForTeamUpdate = false;
			}
			if(checkForSquadUpdate && FirebaseTools.squadUpdateComplete) {
				checkForSquadUpdate = false;
				Debug.Log("squad update complete");
			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				
			}
			
		}
		private void CopyFromServerTeam() {
			foreach (var playerPID in FirebaseTools.serverTeam) {
				LookUpForPIDAndAddToCurrentTeam(playerPID);
			}
		}
		private IEnumerator UpdateFromServerTeam() {
			if (!FirebaseTools.teamUpdateComplete) {
				yield return null;
			}
			foreach(var playerPID in FirebaseTools.serverTeam) {
				LookUpForPIDAndAddToCurrentTeam(playerPID);
			}
		}

		

		private void LookUpForPIDAndAddToCurrentTeam(string playerPID) {
			foreach(var player in FirebaseTools.batTeam) {
				if(player.pid == playerPID) {
					AddToCurrentTeam(player);
					return;
				}
			}
			foreach (var player in FirebaseTools.bwlTeam) {
				if (player.pid == playerPID) {
					AddToCurrentTeam(player);
					return;
				}
			}
			foreach (var player in FirebaseTools.allTeam) {
				if (player.pid == playerPID) {
					AddToCurrentTeam(player);
					return;
				}
			}
			foreach (var player in FirebaseTools.wkTeam) {
				if (player.pid == playerPID) {
					AddToCurrentTeam(player);
					return;
				}
			}
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
			//currentTeamButton.SetActive(false);
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
			//currentTeamButton.SetActive(true);
		}
		public void GotoTeamSelectionScreen() {
			
			GoToScreen(ScreenType.TeamSelectionScreen);
			//currentTeamButton.SetActive(true);
		}

		public void GotoSelectedTeamPlayersScreen() {
			
			GoToScreen(ScreenType.SelectedTeamPlayersScreen);
			//currentTeamButton.SetActive(true);
		}
		public void GotoDashboardScreen() {
			
			GoToScreen(ScreenType.DashboardScreen);
			//currentTeamButton.SetActive(true);
		}
		public void GotoCurrentTeamScreen() {
			GoToScreen(ScreenType.CurrentTeamScreen);
			//currentTeamButton.SetActive(false);
			//screenHandlers[(int)ScreenType.CurrentTeamScreen].ShowFromLeft();
		}
		public void GotoPrevScreen() {
			//currentTeamButton.SetActive(true);
			screenHandlers[(int)ScreenType.CurrentTeamScreen].Hide();
		}
		public void OpenSelectedTeam(string teamName) {
			GotoSelectedTeamPlayersScreen();
			
		}

		public string ReplacePlayerWith( PlayerInfo newPlayer) {
			string message;
			PlayerInfo oldPlayer = screenHandlers[5].GetComponent<CurrentTeamScreenHandler>().selectedPlayer;
			if ( oldPlayer.playerName != "dummy") {
				RemoveFromCurrentTeam(oldPlayer);
				message = AddToTeamWithReturnValue(newPlayer);
				if(message.ToLower()!= "budget exceeded 1000k") {
					message = "success";
				}
				screenHandlers[5].GetComponent<CurrentTeamScreenHandler>().selectedPlayer.playerName = "dummy";
			}
			else {
				message = AddToTeamWithReturnValue(newPlayer);
				
			}
			return message;
		}
		public void AddToCurrentTeam(PlayerInfo newPlayer) {
			if ((currentBudget + System.Int32.Parse(newPlayer.price.ToLower().Replace("k", string.Empty)) > gameBudget)) {
				return;
			}
			
			if (currentTeam.Count < 11 && !currentTeam.Contains(newPlayer)) {
				if (newPlayer.playerType.ToUpper() == "BAT") {
					numOfBatsmen++;
				}
				else if (newPlayer.playerType.ToUpper() == "BWL") {
					numOfBowlers++;
				}
				else if (newPlayer.playerType.ToUpper() == "ALL") {
					numOfAllRounders++;
				}
				else if (newPlayer.playerType.ToUpper() == "WK") {
					numOfWicketKeepers++;
				}
				currentBudget += System.Int32.Parse(newPlayer.price.ToLower().Replace("k", string.Empty));
				currentTeam.Add(newPlayer);
				
			}
			
			
		}
		public string AddToTeamWithReturnValue(PlayerInfo newPlayer) {
			if((currentBudget + System.Int32.Parse(newPlayer.price.ToLower().Replace("k", string.Empty)) > gameBudget) ){
				return "Budget exceeded 1000k";
			}
			string message;
			message = "success";
			if (currentTeam.Count >= 11) {
				message = "Players exceeding 11";
			}
			if (currentTeam.Count < 11 && !currentTeam.Contains(newPlayer)) {
				if(newPlayer.playerType.ToUpper() == "BAT") {
					numOfBatsmen++;
				}
				else if (newPlayer.playerType.ToUpper() == "BWL") {
					numOfBowlers++;
				}
				else if (newPlayer.playerType.ToUpper() == "ALL") {
					numOfAllRounders++;
				}
				else if (newPlayer.playerType.ToUpper() == "WK") {
					numOfWicketKeepers++;
				}
				currentBudget += System.Int32.Parse(newPlayer.price.ToLower().Replace("k", string.Empty));
				currentTeam.Add(newPlayer);
				message = "success";
			}
			
			 
			return message;
		}
		
		public void RemoveFromCurrentTeam(PlayerInfo player) {
			if (player.playerType.ToUpper() == "BAT") {
				numOfBatsmen--;
			}
			else if (player.playerType.ToUpper() == "BWL") {
				numOfBowlers--;
			}
			else if (player.playerType.ToUpper() == "ALL") {
				numOfAllRounders--;
			}
			else if (player.playerType.ToUpper() == "WK") {
				numOfWicketKeepers--;
			}
			currentBudget -= System.Int32.Parse(player.price.ToLower().Replace("k", string.Empty));
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
		public void UpdateTeamIfFoundOnServer() {
			checkForTeamUpdate = true;
			FirebaseTools.GetTeamIfExists(result => { });

		}

		public void UpdateAllSquads() {
			checkForSquadUpdate = true;
			FirebaseTools.GetSquadFromDatabase( "RCB",result=> { });
			FirebaseTools.GetSquadFromDatabase("SRH", result => { });
			FirebaseTools.GetSquadFromDatabase("CSK", result => { });
			FirebaseTools.GetSquadFromDatabase("MI", result => { });
			FirebaseTools.GetSquadFromDatabase("KKR", result => { });
			FirebaseTools.GetSquadFromDatabase("DC", result => { });
			FirebaseTools.GetSquadFromDatabase("KXIP", result => { });
			FirebaseTools.GetSquadFromDatabase("RR", result => { });
			FirebaseTools.GetSquadFromDatabase("ALL", result => { });
			FirebaseTools.GetSquadFromDatabase("BWL", result => { });
			FirebaseTools.GetSquadFromDatabase("WK", result => { });
			FirebaseTools.GetSquadFromDatabase("BAT", result => { });
			Debug.Log("called Update squads");

		}
		public void DiscardAllPlayers() {
			currentTeam.Clear();
			currentBudget = 0;
			numOfAllRounders = 0;
			numOfBatsmen = 0;
			numOfBowlers = 0;
			numOfWicketKeepers = 0;
			CopyFromServerTeam();
		}

		public void ClearAllPlayersFromCurrentTeam() {
			currentTeam.Clear();
			currentBudget = 0;
			numOfAllRounders = 0;
			numOfBatsmen = 0;
			numOfBowlers = 0;
			numOfWicketKeepers = 0;
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
