using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FantasyApp {
	public class TeamSelectionScreenHandler : ScreenHandler {
		public SelectedTeamPlayersScreenHandler selectedTeamPlayersScreenHandler;
		public override void Initialize(MainCanvasHandler parentMenu) {
			base.Initialize(parentMenu);
			this.HandlerType = ScreenType.TeamSelectionScreen;
		}
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void OpenTeam(string teamName) {
			
			selectedTeamPlayersScreenHandler.currentTeam = teamName;
			parentMenu.GotoSelectedTeamPlayersScreen();
		}
	}
}

