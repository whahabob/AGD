using UnityEngine;
using System;
using System.Collections;

public class shoutIndicatorBehaviour : MonoBehaviour {

	public GameObject[] shoutIndicators;
	public float currentIndicator, displayTime, alpha, alphaIncrement;
	private float previousIndicator;
	private float targetIndicator;
	public bool active = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		previousIndicator = currentIndicator;

		if (player)
			currentIndicator = player.GetComponent<playerHandleShouts> ().currentShout;

		if (currentIndicator != previousIndicator) {
			previousIndicator = currentIndicator;
			UpdateIndicators (currentIndicator);
		}

		if (active)
			alpha = Mathf.Clamp (alpha + alphaIncrement, 0.0f, 1.0f);
		else
			alpha = Mathf.Clamp (alpha - alphaIncrement, 0.0f, 1.0f);
	}

	public GameObject FindIndicator (float id) {

		for (int i = 0; i < shoutIndicators.Length; i++) {
			GameObject shoutIndicator = shoutIndicators [i];

			if (shoutIndicator.GetComponent<indicatorBehaviour> ().id == id)
				return shoutIndicator;
		}
		return null;
	}

	public void UpdateIndicators (float currentIndicator) {

		Debug.Log ("KJhb");

		for (int i = 0; i < shoutIndicators.Length; i++) {
			GameObject shoutIndicator = shoutIndicators [i];
			Vector3 center = shoutIndicator.GetComponent<indicatorBehaviour> ().centerPosition;
			Vector3 separation = shoutIndicator.GetComponent<indicatorBehaviour> ().distance;
			float offset = Mathf.Round(shoutIndicator.GetComponent<indicatorBehaviour> ().id - currentIndicator);

			shoutIndicator.GetComponent<indicatorBehaviour> ().targetPosition = separation * offset;
		}
	}

	public void FlashIndicator () {
		CancelInvoke ();

		active = true;
		Invoke ("FadeIndicator", displayTime);
	}

	public void FadeIndicator () {
		active = false;
	}
}
