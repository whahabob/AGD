using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class enemyTextBoxBehaviour : MonoBehaviour {

	public float timeBetweenTalksMin;
	public float timeBetweenTalksMax;
	public float displayTime;
	public float textScale;
	public int crazyness;
	public Color textColor;
	public GameObject talkDisplayParent;

	private PlayerStats nameScript;
	private int lineIndex;
	private float lineLength;
	private string[,] lines;
	public int linesMax = 5;

	// Use this for initialization
	void Start () {
		Invoke ("Talk", Random.Range(timeBetweenTalksMin, timeBetweenTalksMax));
		lines = new string[linesMax, 3];

		lines[0, 0] = "Bweaaarrgghh...";
		lines[0, 1] = "Grrrraaahhhh...";
		lines[0, 2] = "nhhhhaaaarrr...";

		lines[1, 0] = "Bbb.. Braaii...";
		lines[1, 1] = "Hellpp.. leesss";
		lines[1, 2] = "Kkk.. kiiill...";

		lines[2, 0] = "What aaarre you..";
		lines[2, 1] = "Neeeeedd.. to..";
		lines[2, 2] = "Whhhyyyy...";

		lines[3, 0] = "Need... help..";
		lines[3, 1] = "Can... I..";
		lines[3, 2] = "What.. do.. you..";

		lines[4, 0] = "What do you need?";
		lines[4, 1] = "Do you need help?";
		lines[4, 2] = "What are you doing?";
	}

	void Talk () {
		nameScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ();
		lineLength = (float)(linesMax - 1);
		lineIndex = (int)((nameScript.rationality / nameScript.rationalityMax) * lineLength);

		GameObject talkObject = (GameObject) Instantiate(talkDisplayParent, new Vector3(transform.position.x, 2, transform.position.z), Quaternion.identity);
		talkObject.GetComponentInChildren<Text> ().text = lines [lineIndex, Random.Range(0, 3)];
		talkObject.GetComponentInChildren<Text> ().color = textColor;
		talkObject.GetComponentInChildren<Text> ().transform.localScale = new Vector3 (textScale, textScale, textScale);
		talkObject.GetComponentInChildren<shoutTextDisplayBehaviour> ().alpha = 1;
		talkObject.GetComponentInChildren<shoutTextDisplayBehaviour> ().textAngle = Vector3.zero;
		talkObject.GetComponentInChildren<shoutTextDisplayBehaviour> ().displayTime = displayTime;
		Invoke ("Talk", Random.Range(timeBetweenTalksMin, timeBetweenTalksMax));
	}
}