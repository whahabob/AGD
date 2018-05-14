using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFadeScript : MonoBehaviour {

	public float fadeInFactor, currentAlpha;
	private Text text;
	public Image logo;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
			FadeIn ();
	}

	void FadeIn() {
		//Value cannot be lower than 0 and higher than 1
		currentAlpha = Mathf.Clamp(currentAlpha,0,1);

		text.color = new Color (text.color.r,text.color.g,text.color.b, currentAlpha);

		if (logo.color.a >= 1) {
			currentAlpha += fadeInFactor;
		} else {
			currentAlpha = 0;
		}
	}
}
