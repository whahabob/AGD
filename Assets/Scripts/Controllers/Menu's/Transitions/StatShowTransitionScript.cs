using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatShowTransitionScript : MonoBehaviour {

	public Text sanityNumber, attackNumber, timeNumber, scoreNumber;

	//variables for stats
	float sanity;
	int score;
	int time;
	int currentScore;
	int highestKilledEnemies;
	bool calculated;

	//variables for calculating the bonusses
	float sanityBonus;
	int killBonus;
	int timeBonus;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetStats ();
		CalculateScore ();
		SetStats ();
		ShowStats ();
	}
	void GetStats() {
		sanity = PlayerStats.sanityBonus;
		highestKilledEnemies = PlayerStats.highestKilledEnemies;
		currentScore = PlayerStats.score;
		time = PlayerStats.levelCompleteTime;
	}

	void CalculateScore () {
		if (!calculated) {
			sanityBonus = sanity;
			killBonus = highestKilledEnemies * 100;
			timeBonus = 1000 - time/60; 
			if (timeBonus < 0) {
				timeBonus = 0;
			}

			calculated = true;
		}
	}

	void SetStats() {
		score = (int)sanityBonus + killBonus + timeBonus; 
		PlayerStats.score = score;
	}

	void ShowStats() {
		sanityNumber.text = "" + sanityBonus; 
		attackNumber.text = "" + killBonus;
		timeNumber.text = "" + timeBonus;
		scoreNumber.text = "" + score; 
		//		timeNumber = <level>  (er moet nog een timer komen in de level)
		//		attackNumber = highestCombo/killed monsters (moet nog komen)
	}
}
