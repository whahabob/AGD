using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageFadeScript : MonoBehaviour {

	public float fadeInFactor, currentAlpha;
	private Image image;

	public bool fadeIn;
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeIn)
		FadeIn ();
	}

	void FadeIn() {
		//Value cannot be lower than 0 and higher than 1
		currentAlpha = Mathf.Clamp(currentAlpha,0,1);

		//Factor is the speed of the fade in
		currentAlpha += fadeInFactor;
		image.color = new Color (image.color.r,image.color.g,image.color.b, currentAlpha);
	}
}
