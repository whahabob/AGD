using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour
{
         
    public GameObject enemy;                // The enemy prefab to be spawned.
	public int enemyCounter;				// The max count enemies in the level
    public float spawnTime = 3f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	private int enemyCount;


    void Start ()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
        
    }
    void FixedUpdate()
    {
        /*if (Time.time == 3)
        {
            Spawn();
        }*/
    }

    void Spawn ()
    {
               // Find a random index between zero and one less than the number of spawn points.
			   //GameObject[] gos = new GameObject[enemyCounter];
			     
		if(enemyCount < enemyCounter){	   
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		enemyCount++;
		} 
    }
}