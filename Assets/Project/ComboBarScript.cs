using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboBarScript : MonoBehaviour {

	//Bonus variables
	public int bonusTime;
	bool timerActive;
	public int bonusTimer;
	CanvasGroup canvasGroup;
	public float alphaFactor;

	public Text killNumberDisplay;

	public int killedEnemiesNumber;
	int currentKilledEnemiesNumber = 0;
	//reference to scripts
	PlayerStats playerStats;

	// Use this for initialization
	void Start () {
        if (GameObject.FindGameObjectWithTag("Player"))
		    if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats> ())
			    playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats> ();
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!playerStats)
			playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats> ();

		BonusTimer ();
		UpdateBonus ();
	}

	void UpdateBonus () {
		killNumberDisplay.text = "" + killedEnemiesNumber;
		killedEnemiesNumber = playerStats.killedEnemies;
	}
		

	void BonusTimer() {

		//did you kill 1 or more enemies within the timer?
		if (killedEnemiesNumber > currentKilledEnemiesNumber) {
			bonusTimer = bonusTime;
			currentKilledEnemiesNumber = killedEnemiesNumber;
			timerActive = true;
			if (killedEnemiesNumber >= 3) {
				canvasGroup.alpha = 1;
			} 
		}

		if (killedEnemiesNumber == 0) {
			canvasGroup.alpha = 0;
		}

		if (bonusTimer > 0) {
			bonusTimer--;
			if (canvasGroup.alpha > 0)
				canvasGroup.alpha -= alphaFactor;
		}
		else if (bonusTimer <= 0 && timerActive){
			canvasGroup.alpha = 0;
			playerStats.killedEnemies = 0;
			currentKilledEnemiesNumber = 0;
			timerActive = false;
		}
	}
}
