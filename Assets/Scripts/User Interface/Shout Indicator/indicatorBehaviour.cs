using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class indicatorBehaviour : MonoBehaviour {
	//public bool selected = false;
	public float id, scaleDistance, fadeDistance;
	private float imageDefaultAlpha, textDefaultAlpha;
	public Vector3 centerPosition, currentPosition, targetPosition, velocity, distance;
	private Image image;
	private Text text;
	private shoutIndicatorBehaviour shoutIndicatorScript;

	void Start () {
		shoutIndicatorScript = GetComponentInParent<shoutIndicatorBehaviour> ();

		centerPosition = new Vector3 (0f, 0f, 0f);
		currentPosition = transform.localPosition;
		targetPosition = currentPosition;

		image = GetComponentInChildren<Image> ();
		imageDefaultAlpha = image.color.a;
		text = GetComponentInChildren<Text> ();
		text.text = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerHandleShouts> ().shoutName [(int)id];
		textDefaultAlpha = text.color.a;
	}
		
	void Update () {
		transform.localPosition = Vector3.SmoothDamp (transform.localPosition, targetPosition, ref velocity, 0.1f); currentPosition = transform.localPosition;

		float scaleFactor = Mathf.Clamp (1.0f - (Mathf.Abs (centerPosition.magnitude - currentPosition.magnitude) / scaleDistance), 0.0f, 1.0f);
		float alphaFactor = Mathf.Clamp (1.0f - (Mathf.Abs (centerPosition.magnitude - currentPosition.magnitude) / fadeDistance), 0.0f, 1.0f);
		transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
		image.color = new Color (image.color.r, image.color.g, image.color.b, (imageDefaultAlpha * alphaFactor) * shoutIndicatorScript.alpha);
		text.color = new Color (text.color.r, text.color.g, text.color.b, (textDefaultAlpha * alphaFactor) * shoutIndicatorScript.alpha);
	}
}
