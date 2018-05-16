using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class playerHandleShouts : MonoBehaviour {

	public int startShout = 0;
	private int maxShouts = 3;
	public int currentShout;
	public int currentCooldown;
	private int wall;

	public string[] shoutName;
	private int[] shoutRange;
	private int[] shoutRadius;
	private int[] shoutDamage;
	public int[] shoutCooldown;
	private int[] shoutCost;
	private bool[] shoutIsUnlocked;

	private float maxBreath;
	private float breath;
	public float knockbackForce;
	private float angleDifference;

	public GameObject shoutDisplayPrefab;
	public GameObject shoutObject;

	private GameObject mainCamera, shoutIndicator;
	private CameraController cameraScript;
	private PlayerStats playerStats;

	// Use this for initialization
	void Awake () {

		//Init
		wall = 1 << 8;
		currentShout = startShout;
		currentCooldown = 0;

		//Create shout arrays
		shoutName = new string[maxShouts];
		shoutRange = new int[maxShouts];
		shoutRadius = new int[maxShouts];
		shoutDamage = new int[maxShouts];
		shoutCooldown = new int[maxShouts];
		shoutCost = new int[maxShouts];
		shoutIsUnlocked = new bool[maxShouts];

		//Initialize knockback/slow shout
		shoutName[0] = "Knockback";
		shoutRange [0] = 4;
		shoutRadius [0] = 360;
		shoutDamage [0] = 15;
		shoutCooldown [0] = 100;
		shoutCost [0] = 60;
		shoutIsUnlocked [0] = true;

		//Initialize cone shout
		shoutName[1] = "Cone";
		shoutRange [1] = 7;
		shoutRadius [1] = 90;
		shoutDamage [1] = 40;
		shoutCooldown [1] = 40;
		shoutCost [1] = 35;
		shoutIsUnlocked [1] = true;

		//To be announced
		shoutName[2] = "To be announced";
		shoutRange [2] = 0;
		shoutRadius [2] = 0;
		shoutDamage [2] = 0;
		shoutCooldown [2] = 1;
		shoutCost [2] = 0;
		shoutIsUnlocked [2] = true;

		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		shoutIndicator = GameObject.FindGameObjectWithTag ("ShoutIndicator");
		cameraScript = mainCamera.GetComponent<CameraController> ();
		playerStats = GetComponent<PlayerStats> ();

		maxBreath = playerStats.breathMax;
		breath = maxBreath;
	}
	
	// Update is called once per frame
	void Update () {
		maxBreath = playerStats.breathMax;
		breath = playerStats.breath;
	}

	void FixedUpdate() {
		//Get mouse scrollwheel input
		var scrollWheelInput = Input.GetAxis ("Mouse ScrollWheel");

		//Adapt the current shout to this input
		if (scrollWheelInput > 0) {
			if (currentShout < maxShouts - 1) {
				currentShout++;
				shoutIndicator.GetComponent<shoutIndicatorBehaviour>().FlashIndicator ();
			}
		} else if (scrollWheelInput < 0) {
			if (currentShout > 0) {
				currentShout--;
				shoutIndicator.GetComponent<shoutIndicatorBehaviour>().FlashIndicator ();
			}
		}

		//Check for shout trigger
		if (Input.GetMouseButtonDown (0)) {
			if (breath >= shoutCost [currentShout]) {
				if (currentCooldown <= 0) {
					triggerShout (currentShout);
					currentCooldown = shoutCooldown [currentShout];
				}
			}
		}

		if (currentCooldown > 0)
			currentCooldown--;
		else if (currentCooldown < 0)
			currentCooldown = 0;
	}

	void triggerShout(int shout) {		
		int range = shoutRange [shout];
		int damage = shoutDamage [shout];
		int cost = shoutCost [shout];
		playerStats.breath -= (float) cost;

		Instantiate (shoutDisplayPrefab, transform.position + new Vector3 (Random.Range(-3, 3), 5f, Random.Range(-3, 3)), Quaternion.identity);
		GameObject shoutInstance = (GameObject) Instantiate (shoutObject, transform.position + new Vector3 (0, 1f, 0), Quaternion.identity);
		shoutInstance.GetComponent<shoutObjectBehaviour> ().UpdateRange (range); shoutInstance.GetComponent<shoutObjectBehaviour> ().shoutDirection = transform.rotation;
		cameraScript.shake = cameraScript.maxShake;

		switch (shout) {

			//Slow and knockback shout
			case 0: {
				shoutInstance.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("KnockbackShout", typeof(Sprite)) as Sprite;
					
				//Get all possible affected targets
				GameObject[] Enemies = GameObject.FindGameObjectsWithTag ("Enemy");
				GameObject[] Props = GameObject.FindGameObjectsWithTag ("Prop");

				//Loop through enemies
				for (int i = 0; i < Enemies.Length; i++) {					
					GameObject thisEnemy = Enemies [i];
					if (Vector3.Distance(transform.position, thisEnemy.transform.position) <= range) {						
						float xDif = thisEnemy.transform.position.x - transform.position.x;
						float yDif = thisEnemy.transform.position.y - transform.position.y;
						float zDif = thisEnemy.transform.position.z - transform.position.z;
						Vector3 enemyDirection = new Vector3(xDif, yDif, zDif).normalized;

						if (!Physics.Raycast (transform.position, enemyDirection, range, wall)) {
							thisEnemy.GetComponent<AI1> ().changePatience (-damage);
							thisEnemy.GetComponent<AI1> ().Slow (false, 0.25f, 3);
							thisEnemy.GetComponent<Rigidbody> ().AddForce (enemyDirection * knockbackForce);
						}
					}
				}

				//Loop through props
				for (int i = 0; i < Props.Length; i++) {					
					GameObject thisProp = Props [i];
					if (Vector3.Distance(transform.position, thisProp.transform.position) <= range) {
						float xDif = thisProp.transform.position.x - transform.position.x;
						float yDif = thisProp.transform.position.y - transform.position.y;
						float zDif = thisProp.transform.position.z - transform.position.z;
						Vector3 propDirection = new Vector3(xDif, yDif, zDif).normalized;

						if (!Physics.Raycast (transform.position, propDirection, range, wall)) {
							thisProp.GetComponent<Rigidbody> ().AddForce (propDirection * knockbackForce);
						}
					}
				}
			}
			break;

			//Cone shout
			case 1: {
				shoutInstance.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("ConeShout", typeof(Sprite)) as Sprite;

				//Get all possible affected targets
				GameObject[] Enemies = GameObject.FindGameObjectsWithTag ("Enemy");

				//Loop through enemies
				for (int i = 0; i < Enemies.Length; i++) {
					GameObject thisEnemy = Enemies [i];
					if (Vector3.Distance(transform.position, thisEnemy.transform.position) <= range) {
						float xDif = thisEnemy.transform.position.x - transform.position.x;
						float yDif = thisEnemy.transform.position.y - transform.position.y;
						float zDif = thisEnemy.transform.position.z - transform.position.z;
						Vector3 enemyDirection = new Vector3(xDif, yDif, zDif).normalized;
						angleDifference = Vector3.Angle (transform.forward, enemyDirection);

						if (!Physics.Raycast (transform.position, enemyDirection, range, wall) &&
							angleDifference < (float) shoutRadius [shout] / 2) {
							thisEnemy.GetComponent<AI1> ().changePatience (-damage);
						}
					}
				}
			}
			break;

			default:
				break;
		}
	}
}