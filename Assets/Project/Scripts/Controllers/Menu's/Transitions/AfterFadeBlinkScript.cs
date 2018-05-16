using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AfterFadeBlinkScript : MonoBehaviour {

	//how fast it will blink
	public float blinkFactor;
	float currentAlpha;
	 bool reachedMaxAlpha;

	//delay timer (so the reference script wont get destroyed right away)
	int timer1 = 5;
	public Text text;

	//reference to script
	TextFadeScript textFadeScript;
	 bool textFadeScriptDisable;

	// Use this for initialization
	void Start () {
		textFadeScript = GetComponent<TextFadeScript> ();
		currentAlpha = 1;
	}
	
	// Update is called once per frame
	void Update () {
		Timer ();

		if (timer1 <= 0) {
			Blink ();
		}
	}

	void Blink () {

		//checks if script got deleted and if the text is visable
		if (text.color.a >= 1 && !textFadeScriptDisable) {

			//destroys the script component
			Destroy (textFadeScript);
			textFadeScriptDisable = true;
		}


		//start blinking when the reference script got removed
		if (textFadeScriptDisable) {
			currentAlpha = Mathf.Clamp (currentAlpha, 0, 1);

			//did the text reached the max alpha?
			if (currentAlpha >= 1 && !reachedMaxAlpha) {
				reachedMaxAlpha = true;
			} else if (currentAlpha <= 0 && reachedMaxAlpha) {
				reachedMaxAlpha = false;
			}

			//then add/lower the alpha
			if (!reachedMaxAlpha && currentAlpha >= 0) {
				currentAlpha += blinkFactor;
			} else if (reachedMaxAlpha  && currentAlpha <= 1) {
				currentAlpha -= blinkFactor;
			}

			text.color = new Color (text.color.r,text.color.g,text.color.b, currentAlpha);
		}
	}

//delay timer 
	void Timer () {
		timer1 = Mathf.Clamp (timer1, 0, 5);
		timer1--;
	}
  }
