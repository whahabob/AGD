using UnityEngine;
using System.Collections;

public class NextLevelSpawn : MonoBehaviour {

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            CheckpointController.spawnpoint = new Vector3(-42.52f, 0, 38.83f);
        }
    }
}
