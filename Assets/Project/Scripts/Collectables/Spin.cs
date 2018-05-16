using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate (new Vector3(5, 0, 0));
	}
}
