using UnityEngine;
using System.Collections;

public class batteryPowerUpBehaviour : MonoBehaviour {
	public float energy;

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerStats> ().FillBattery (energy);
			other.gameObject.GetComponent<PlayerStats> ().FlashBatteryIndicator ();
			Destroy (gameObject);
		}
	}
}