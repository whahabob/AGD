using UnityEngine;
using System.Collections;

public class shoutObjectBehaviour : MonoBehaviour {

	public float alpha = 1f;
	public float alphaIncrement = 0.025f;
	private float alphaMax;

	public float range;
	public float spriteRadius = 1.28f;

	public Quaternion shoutDirection;

	// Use this for initialization
	void Start () {
		alphaMax = alpha;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = shoutDirection;
		transform.Rotate (Vector3.right * 90);
		transform.Rotate (Vector3.forward * 45);
		
		if (alpha > 0.0f)
			alpha = Mathf.Clamp (alpha - alphaIncrement, 0, alphaMax);
		else
			Destroy (gameObject);

		GetComponent <SpriteRenderer> ().color = new Color (1f, 1f, 1f, alpha);
	}

	public void UpdateRange (float newRange) {
		range = newRange;
		transform.localScale = new Vector3 (range / spriteRadius, range / spriteRadius, range / spriteRadius);
	}
}