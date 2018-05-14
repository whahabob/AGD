using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
	//text variables in main menu
	public Text startText, settingsText, exitText;

	//text variables in settings menu
	public Text volumeNumber, okButton;
	public static int currentVolume;

	//press scene variables
	public string startScene;

	public int currentButtonMainMenu, currentButtonSettingsMenu;

	//settings menu variables
	public GameObject settingsMenu;
	public bool pressedSettingsMenu;

	//images in main menu
	public Image logo;
	public Image blackFadeIn;


	//sound effects
	public AudioClip menuNavigate;
	public AudioClip menuEnter;

	//delay timer
	public int settingsDelayTimer; //settingsDelayTimer is a delay for preventing changing settings on the first click (while clicking on settings button)

	// Use this for initialization
	void Start () {
		currentButtonMainMenu = 0;
		settingsMenu.SetActive (false);
		currentVolume = 100;
	}
	
	// Update is called once per frame
	void Update () {

		delayTimer ();
		// are you in the settings menu?
		if (!pressedSettingsMenu) {
			MainMenuButtonSelector ();
		}

		if (pressedSettingsMenu) {
			SettingsMenuButtonSelector ();
		}
	}

	//method for choosing a button
	void MainMenuButtonSelector () {

		//checks if the (alpha of) the logo is fully visable
		if (logo.color.a >= 1) {

			//Gets the variables of fade in script of blackFadeIn Image
			ImageFadeScript blackFadeInScript = blackFadeIn.GetComponent <ImageFadeScript> ();

			//Value cannot be lower than 0 and higher than 2
			currentButtonMainMenu = Mathf.Clamp (currentButtonMainMenu, 0, 2);


			//Navigate through keyboard
			if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
				currentButtonMainMenu--;
				AudioSource.PlayClipAtPoint (menuNavigate, new Vector3 (0, 0, 0), 0.3f);
			} 

			if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
				currentButtonMainMenu++;
				AudioSource.PlayClipAtPoint (menuNavigate, new Vector3 (0, 0, 0), 0.3f);
			}
				
			//switch to check where the current selected button is
			switch (currentButtonMainMenu) {

			case 0:
				GoToLevel1 ();
				break;

			case 1:
				GoToSettings ();
				break;

			case 2:
				ExitTheGame ();
				break;
			}
		}
	}

	//settings menu button selector
	void SettingsMenuButtonSelector () {
		volumeNumber.text = "" + currentVolume + "%";
		settingsMenu.SetActive (true);

		//Value cannot be lower than 0 and higher than 2
		currentButtonSettingsMenu = Mathf.Clamp (currentButtonSettingsMenu, 0, 1);

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			currentButtonSettingsMenu--;
			AudioSource.PlayClipAtPoint(menuNavigate,new Vector3(0,0,0),0.3f);
		} 

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			currentButtonSettingsMenu++;
			AudioSource.PlayClipAtPoint(menuNavigate,new Vector3(0,0,0),0.3f);
		}

		//switch to check where the current selected button is
		switch (currentButtonSettingsMenu) {

		case 0:
			AdjustVolume ();
			break;

		case 1:
			BackToMainMenu ();
			break;

		}
	}

	public void GoToLevel1() {
		//Gets the variables of fade in script of blackFadeIn Image
		ImageFadeScript blackFadeInScript = blackFadeIn.GetComponent <ImageFadeScript> ();

		startText.color = Color.red;
		settingsText.color = Color.black;
		exitText.color = Color.black;

		if (Input.GetKeyDown (KeyCode.Return) || Input.GetMouseButtonDown(0)) {
			blackFadeInScript.fadeIn = true;
			AudioSource.PlayClipAtPoint(menuEnter,new Vector3(0,0,0),0.3f);
		}

		if (blackFadeIn.color.a >= 1)
			SceneManager.LoadScene (startScene);
	}

	public void GoToSettings() {
		settingsText.color = new Color (255, 0, 0);
		exitText.color = Color.black;
		startText.color = Color.black;

		if (Input.GetKeyDown (KeyCode.Return) || Input.GetMouseButtonDown(0)) {
			settingsDelayTimer = 10;
			pressedSettingsMenu = true;
			settingsMenu.SetActive (true);
			AudioSource.PlayClipAtPoint (menuEnter, new Vector3 (0, 0, 0), 0.3f);
		}

	}

	public void ExitTheGame () {
		//Gets the variables of fade in script of blackFadeIn Image
		ImageFadeScript blackFadeInScript = blackFadeIn.GetComponent <ImageFadeScript> ();

		exitText.color = Color.red;
		settingsText.color = Color.black;
		startText.color = Color.black;

		if (Input.GetKeyDown (KeyCode.Return) || Input.GetMouseButtonDown(0)) {
			AudioSource.PlayClipAtPoint(menuEnter,new Vector3(0,0,0),0.3f);
			blackFadeInScript.fadeIn = true;
		}
		if (blackFadeIn.color.a >= 1)
			Application.Quit();

	}

	public void AdjustVolume() {
		volumeNumber.color = Color.red;
		okButton.color = Color.black;
		if (settingsDelayTimer <= 0) {
			if (currentVolume > 0 && Input.GetKeyDown (KeyCode.LeftArrow) || currentVolume > 0 && Input.GetKeyDown (KeyCode.A) || currentVolume > 0 && Input.GetMouseButtonDown (0)) {
				AudioListener.volume -= 0.1f;
				currentVolume -= 10;
				AudioSource.PlayClipAtPoint (menuEnter, new Vector3 (0, 0, 0), 0.3f);
			}
			if (currentVolume < 200 && Input.GetKeyDown (KeyCode.RightArrow) || currentVolume < 200 && Input.GetKeyDown (KeyCode.D) || currentVolume < 200 && Input.GetMouseButtonDown (1)) {
				AudioListener.volume += 0.1f;
				currentVolume += 10;
				AudioSource.PlayClipAtPoint (menuEnter, new Vector3 (0, 0, 0), 0.3f);
			}
		}

	}

	public void BackToMainMenu () {
		okButton.color = Color.red;
		volumeNumber.color = Color.black;
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetMouseButtonDown(0)) {
			currentButtonSettingsMenu = 0;
			settingsMenu.SetActive (false);
			pressedSettingsMenu = false;
			AudioSource.PlayClipAtPoint (menuEnter, new Vector3 (0, 0, 0), 0.3f);
		}

	}

	void delayTimer () {

		settingsDelayTimer = Mathf.Clamp (settingsDelayTimer, 0, 10);

		if (settingsDelayTimer > 0) {
			settingsDelayTimer--;
		}
	}
}
