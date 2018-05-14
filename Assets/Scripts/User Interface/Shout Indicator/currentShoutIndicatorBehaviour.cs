using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class currentShoutIndicatorBehaviour : MonoBehaviour {
	private Text text;
	private playerHandleShouts shoutScript;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text> ();
		shoutScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerHandleShouts> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = shoutScript.shoutName [shoutScript.currentShout];
	}
}
