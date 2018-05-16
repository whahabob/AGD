using UnityEngine;
using System.Collections;

public class KeyGate : MonoBehaviour {

    private PlayerStats statScript;

    void Start()
    {
        statScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && statScript.keyAmount > 0)
        {
            statScript.keyAmount--;
            Destroy(gameObject);
           
        }
    }
}
