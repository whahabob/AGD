using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverScript : MonoBehaviour {

	public Button restartText;
	public Button backText;
	// Use this for initialization
	void Start () {
		restartText = restartText.GetComponent<Button> ();
		backText = backText.GetComponent<Button> ();
	}

	public void BackPress() {

		SceneManager.LoadScene ("MainMenu");
	}


	public void StartLevel(){
		SceneManager.LoadScene ("Level 1");
	}

}