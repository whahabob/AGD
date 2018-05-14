using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class shoutTextDisplayBehaviour : MonoBehaviour {

	private Text thisText;
	private PlayerStats nameScript;
	private string shoutToPrint;
	public float displayTime;
	private float timer;
	public float maxTextAngleWiggle;
	public GameObject parent;
	public Vector3 textAngle;
	public float alpha;
	public float alphaIncrement;
	private int lineIndex;
	private float lineLength;

	// Use this for initialization
	void Awake () {
		thisText = gameObject.GetComponent<Text> ();
		nameScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ();

		lineLength = (float)(nameScript.linesMax - 1);
		lineIndex = (int)((nameScript.rationality / nameScript.rationalityMax) * lineLength);
		shoutToPrint = nameScript.lines [lineIndex, (int) Random.Range(0, 3)];

		textAngle = new Vector3 (0, 0, Random.Range (-maxTextAngleWiggle, maxTextAngleWiggle));
		transform.Rotate (textAngle);
		thisText.text = shoutToPrint;
		timer = displayTime;
	}

	void Update () {
		timer -= Time.deltaTime;

		if (timer <= 0) {
			alpha -= alphaIncrement;
			thisText.color = new Color (thisText.color.r, thisText.color.g, thisText.color.b, alpha);

			if (alpha <= 0) {
				Destroy (gameObject);
				Destroy (parent);
			}
		}
	}
}