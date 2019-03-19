using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FantasyApp { 
	public abstract class ScreenHandler : MonoBehaviour {
		public ScreenType HandlerType { get { return m_handlerType; } set { m_handlerType = value; } }
	
		public MainCanvasHandler parentMenu;

		#region

		/// <summary>
		/// Called whenever the screen is shown. Override to implement functionality. Occurs at the moment the transition starts (if there is one).
		/// </summary>
		public virtual void OnShow() { }

		/// <summary>
		/// Called whenever the screen is hidden. Override to implement functionality. Occurs at the moment the transition starts (if there is one).
		/// </summary>
		public virtual void OnHide() { }

		#endregion

		public virtual void Initialize(MainCanvasHandler parentMenu) {
			this.parentMenu = parentMenu;
		}

		public virtual void ShowFromLeft() {
			gameObject.SetActive(true);
			
			OnShow();
		}

		public virtual void ShowFromRight() {
			gameObject.SetActive(true);
			
			OnShow();
		}

		public virtual void HideToRight() {
			if (!gameObject.activeInHierarchy) { return; }
			OnHide();
			Hide();
		}

		public virtual void HideToLeft() {
			if (!gameObject.activeInHierarchy) { return; }
			OnHide();
			Hide();
		}

		public virtual void Hide() {
			if (!gameObject.activeInHierarchy) { return; }
			gameObject.SetActive(false);
			OnHide();
		}
	

		private ScreenType m_handlerType;
	}


}
