using UnityEngine;
using System.Collections;

public class torchObjectBehaviour : MonoBehaviour {

	//Variables for the torch effect
	public float flickerSize;
	public float flickerInterval;
	public float flickerSmoothing;

	//Core private variables for references/positions etc.
	private Light torchLight;
	private float lightRange;
	private float targetRange;

	//Setup
	void Start () {
		//Get the Light component of this torch, save its default range and start repeating the flicker function
		torchLight = GetComponent<Light> ();
		lightRange = torchLight.range;
		InvokeRepeating ("Flicker", flickerInterval, flickerInterval);
	}

	//Smoothens the range of the torch towards a newly randomized range (see Flicker function)
	void Update () {
		torchLight.range = Mathf.SmoothDamp (torchLight.range, targetRange, ref flickerSmoothing, flickerInterval);
	}

	//(Re-)sets a target range for the torch to smooth towards, using the default range so it never goes out of bounds
	void Flicker () {
		targetRange = lightRange + Random.Range (-flickerSize, flickerSize);
	}
}