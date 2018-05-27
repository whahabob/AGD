using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class currentShoutIndicatorBehaviour : MonoBehaviour {
	private Text text;
	private playerHandleShouts shoutScript;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!shoutScript) 
			shoutScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerHandleShouts> ();
		
		text.text = shoutScript.shoutName [shoutScript.currentShout];
	}
}
