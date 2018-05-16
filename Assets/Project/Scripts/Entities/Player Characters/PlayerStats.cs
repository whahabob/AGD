using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	public static int score, life = 2;
	UiFadeScript uiFadeScript;

	public int keyAmount, cooldown, cooldownMax;
	public float sanity, sanityMax, breath, breathMax, rationality, rationalityMax, breathIncrement, batteryLife, batteryLifeMax, batteryIncrement, batteryFlashTime, batteryWarning;
	public GameObject rationalityBar, sanityBar, breathBar, batteryBar, batteryIndicator, cooldownBar, cooldownIndicator;
	public Text scoreDisplayer;
	private Light flashlight;
	private bool warningTrigger, isDead;
	private Color batteryIndicatorBaseColor;
	public string[,] lines;
	public int linesMax = 6;
	private playerHandleShouts shoutScript;

	//Transition scene bonus 
	public int killedEnemies;
	public static int highestKilledEnemies;
	public static float sanityBonus;

	//Time bonus
	public int timeLevel;
	public static int levelCompleteTime;

	// Use this for initialization
	void Start () {

		//Ui fade reference
		shoutScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerHandleShouts> ();
		uiFadeScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UiFadeScript> ();

		//Flashlight battery init
		InvokeRepeating ("DrainBattery", 1, 1);
		flashlight = GetComponentInChildren<Light> ();
		batteryBar.SetActive (false);
		warningTrigger = false;
		batteryIndicatorBaseColor = batteryIndicator.GetComponent<Image> ().color;
		lines = new string[linesMax, 3];

		lines[0, 0] = "WAAAAAAAAHHHHH!";
		lines[0, 1] = "BLEEEEEAAAAARGH!";
		lines[0, 2] = "RRRRRHAAAAAAAA!!";

		lines[1, 0] = "WWWHHHH... AAA!!";
		lines[1, 1] = "HAAAAAaaaaal....";
		lines[1, 2] = "GGHHOOOOOOOOOOAA!";

		lines[2, 0] = "WAAAAAYYY! AAAAWW!";
		lines[2, 1] = "LEEEAAAAAAAAAAVVVEE.";

		lines[2, 2] = "FFFFHHHUUUUUUU!....";
		lines[3, 0] = "GGHHOO... AAWAAAAAAYY!..";
		lines[3, 1] = "DOONNN'TT... WANT...";
		lines[3, 2] = "WWHHHYY.. NOOOO...";

		lines[4, 0] = "GEEETT LOOOOSSSSTT!!";
		lines[4, 1] = "IIII... CAAANNN...";
		lines[4, 2] = "LEEAAVEE MEEEEE!";

		lines[5, 0] = "FUCK OFF!";
		lines[5, 1] = "STAY BACK!";
		lines[5, 2] = "ENJOY THE GODDAMN PLAYTEST!";
	}
	
	// Update is called once per frame
	void Update () {
		TimerLevel ();
		SetBonus ();

		breath = Mathf.Clamp (breath + breathIncrement, 0, breathMax);
		scoreDisplayer.text = score.ToString();

		rationalityBar.transform.localScale = new Vector3 (rationality / rationalityMax, rationalityBar.transform.localScale.y, rationalityBar.transform.localScale.z);
		sanityBar.transform.localScale = new Vector3 (sanity / sanityMax, sanityBar.transform.localScale.y, sanityBar.transform.localScale.z);
		breathBar.transform.localScale = new Vector3 (breath / breathMax, breathBar.transform.localScale.y, breathBar.transform.localScale.z);
		batteryBar.transform.position = gameObject.transform.position + new Vector3 (0, 5.25f, -0.15f);
		batteryIndicator.transform.localScale = new Vector3 (batteryLife / batteryLifeMax, batteryIndicator.transform.localScale.y, batteryIndicator.transform.localScale.z);

		cooldown = shoutScript.currentCooldown;
		cooldownMax = shoutScript.shoutCooldown [shoutScript.currentShout];

		if (cooldown > 0)
			cooldownBar.SetActive (true);
		else
			cooldownBar.SetActive (false);			

		cooldownBar.transform.position = gameObject.transform.position + new Vector3 (0, 5.25f, -0.45f);
		cooldownIndicator.transform.localScale = new Vector3 (Mathf.Clamp((float)cooldown / (float)cooldownMax, 0.0f, 1.0f), cooldownIndicator.transform.localScale.y, cooldownIndicator.transform.localScale.z);

		if (Input.GetKeyDown (KeyCode.F)){
			if (!flashlight.gameObject.activeSelf) {
				if (batteryLife > 0) {
					InvokeRepeating ("DrainBattery", 0, 1);
					flashlight.gameObject.SetActive (true);
				}
				if (!batteryBar.activeSelf)
					FlashBatteryIndicator ();
			} else {
				CancelInvoke ("DrainBattery");
				flashlight.gameObject.SetActive (false);
			}
		}
	}

	void OnCollisionStay (Collision other) {
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "FireTrap") {
			
			killedEnemies = 0;

			sanity = Mathf.Clamp (sanity - 1, 0, sanityMax);

			if (sanity <= 0 && !isDead) {
				isDead = true;
				life--;
				highestKilledEnemies = 0;

				uiFadeScript.fadeOut = true;
				if (life <= 0) {
					uiFadeScript.fadeOut = true;
					uiFadeScript.transitionType = "GameOver";
				}
			}
		}
	}






	public void DrainBattery () {

			batteryLife = Mathf.Clamp (batteryLife - batteryIncrement, 0, batteryLifeMax);


		if (batteryLife / batteryLifeMax > batteryWarning) {
			if (warningTrigger == true) {
				CancelInvoke ("WarningBatteryIndicator");
				batteryBar.SetActive (false);
				batteryIndicator.GetComponent<Image> ().color = batteryIndicatorBaseColor;
				warningTrigger = false;
			}
		} else if (batteryLife / batteryLifeMax > 0.0f) {
			if (warningTrigger == false) {
				CancelInvoke ("FlashBatteryIndicator");
				Invoke ("WarningBatteryIndicator", 0.5f);
				batteryBar.SetActive (true);
				batteryIndicator.GetComponent<Image> ().color = new Color (1, 0, 0, batteryIndicator.GetComponent<Image> ().color.a);
				warningTrigger = true;
			}
		} else {
			CancelInvoke ("FlashBatteryIndicator");
			CancelInvoke ("WarningBatteryIndicator");
			batteryBar.SetActive (false);
		}

		if (batteryLife <= 0) {
			if (flashlight.gameObject.activeSelf) {
				flashlight.gameObject.SetActive (false);
			}
		}
	}

	public void FillBattery (float batteryLevel) {
		batteryLife = Mathf.Clamp (batteryLife + batteryLevel, 0, batteryLifeMax);

		if (batteryLife / batteryLifeMax > batteryWarning)
			batteryIndicator.GetComponent<Image> ().color = batteryIndicatorBaseColor;
	}

	public void FlashBatteryIndicator() {
		if (batteryBar.activeSelf)
			batteryBar.SetActive (false);
		else {
			batteryBar.SetActive (true);
			Invoke ("FlashBatteryIndicator", batteryFlashTime);
		}
	}

	public void WarningBatteryIndicator() {
		batteryBar.SetActive (!batteryBar.activeSelf);
		Invoke ("WarningBatteryIndicator", 0.5f);
	}

	public void TimerLevel () {
		timeLevel ++;
	}

	public void SetBonus () {

		//highest killed enemies without getting hit
		if (killedEnemies > highestKilledEnemies) {
			highestKilledEnemies = killedEnemies;
		}

		//set current hp for hp bonus
		sanityBonus = sanity;
	}
}