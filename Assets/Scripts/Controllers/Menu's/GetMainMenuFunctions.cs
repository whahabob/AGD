using UnityEngine;
using System.Collections;

public class GetMainMenuFunctions : MonoBehaviour {

	MainMenuScript mainMenuScript;
	// Use this for initialization
	void Start () {
		mainMenuScript = GetComponentInParent<MainMenuScript> ();
	}

	//main menu
	public void GoToLevel1 () {
		if (!mainMenuScript.pressedSettingsMenu) {
			mainMenuScript.currentButtonMainMenu = 0;
		}

	}

	public void GoToSettings () {
		if (!mainMenuScript.pressedSettingsMenu) {
			mainMenuScript.currentButtonMainMenu = 1;
		}
	}

	public void ExitTheGame () {
		if (!mainMenuScript.pressedSettingsMenu) {
			mainMenuScript.currentButtonMainMenu = 2;
		}
	}

	//settings menu
	public void AdjustVolume () {
		if (mainMenuScript.pressedSettingsMenu) {
			mainMenuScript.currentButtonSettingsMenu = 0;
		}
	}

	public void BackToMainMenu () {
		if (mainMenuScript.pressedSettingsMenu) {
		mainMenuScript.currentButtonSettingsMenu = 1;
		}
	}
}
