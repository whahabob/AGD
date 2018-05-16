using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {

    public static Vector3 spawnpoint = new Vector3(-42.52f,0,38.83f);
    // Use this for initialization
    void Start () {
        transform.position = spawnpoint;
	}
}
