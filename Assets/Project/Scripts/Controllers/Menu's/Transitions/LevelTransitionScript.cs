using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour {

	//text variables 
	public Text levelTitle, levelSubTitle;

	//background image variables
	public Image blackBackground;
	public float alphaFactor;
	float blackBackgroundAlpha;


	//scene transition variable
	public string transitionScene;

	//transition type
	public bool transitionByTextAlpha;
	public bool transitionByButtonPress;
	bool pressedButton = false;

	//delay for button press
	int timer = 180;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//get black background color variables
		blackBackground.color = new Color (0,0,0, blackBackgroundAlpha);
		blackBackgroundAlpha = blackBackground.color.a;

		Timer ();
		TransitionByText ();
		TransitionByButton ();
	}

	//transition when text has appeared
	void TransitionByText () {
		if (transitionByTextAlpha && levelTitle.color.a >= 1 && levelSubTitle.color.a >= 1) {
			blackBackgroundAlpha += alphaFactor;
			if (blackBackgroundAlpha >= 1) {
				SceneManager.LoadScene (transitionScene);
			}
		}
	}

	//transition when a button is pressed
	void TransitionByButton (){
		if (Input.anyKeyDown && timer <= 0) {
			pressedButton = true;
		}

		if (transitionByButtonPress && pressedButton) {
			blackBackgroundAlpha += alphaFactor;

			if (blackBackgroundAlpha >= 1)
			{

				SceneManager.LoadScene (transitionScene);
			}
		}
	}

	void Timer() {
		timer--;
	}
}