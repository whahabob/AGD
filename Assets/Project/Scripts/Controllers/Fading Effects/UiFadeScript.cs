using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiFadeScript : MonoBehaviour {


	public Image fadeBackground;

	float currentAlpha;
	float alphaFactor = 0.01f;

	public bool fadeIn;
	public bool fadeOut;

	//types of scene transition
	public string transitionType;

	//public bool transitionByBrain;
	//public bool transitionByGameOver;

	// Use this for initialization
	void Start () {
		fadeBackground.enabled = true;
		fadeIn = true;
	}

	// Update is called once per frame
	void Update () {
		Fade ();
	}

	void Fade () {
		currentAlpha = fadeBackground.color.a;

		if (fadeIn) {
			currentAlpha -= alphaFactor;
			if (currentAlpha <= 0) {
				currentAlpha = 0;
				fadeIn = false;
			}
		} else if (fadeOut) {
			currentAlpha += alphaFactor;
			if (currentAlpha >= 1) {
				currentAlpha = 1;
				fadeOut = false;

				switch (transitionType) {

				case "Brain":
                       // SceneManager.LoadScene("LevelGeneration");
                    //SceneManager.LoadScene ("StatsTransition");
					break;
				case "GameOver":
					SceneManager.LoadScene ("GameOver");
					break;

				default: 
					SceneManager.LoadScene (Application.loadedLevelName);
					break;
				}



			}
		}

		fadeBackground.color = new Color (fadeBackground.color.r,fadeBackground.color.g,fadeBackground.color.b,currentAlpha);
	}
}
