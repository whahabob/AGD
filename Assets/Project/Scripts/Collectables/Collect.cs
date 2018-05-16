using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Collect : MonoBehaviour
{
    public AudioClip coinCollect;
    PlayerStats statScript;
    public GameObject gate;
    UiFadeScript uiFadeScript;

    void Start() {
        statScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        uiFadeScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UiFadeScript>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Brain")
            {
                AudioSource.PlayClipAtPoint(coinCollect, transform.position);
                PlayerStats.levelCompleteTime = statScript.timeLevel;
                statScript.rationality += 5;
                uiFadeScript.fadeOut = true;
                uiFadeScript.transitionType = "Brain";
                Destroy(gameObject);
            }

            if (gameObject.tag == "Key")
            {
                AudioSource.PlayClipAtPoint(coinCollect, transform.position);                               
                Destroy(gameObject);
                Destroy(gate);
            }          
                      
        }
    }
}


