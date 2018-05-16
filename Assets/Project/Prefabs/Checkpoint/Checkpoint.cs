using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CheckpointController.spawnpoint = transform.position;
        }
    }
}
