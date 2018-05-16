using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TrapCollision : MonoBehaviour
{
    PlayerStats statScript;
    UiFadeScript uiFadeScript;



    void Start()
    {
        statScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        uiFadeScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UiFadeScript>();
    }
    

    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Player")
        {
            statScript.sanity -= 15;
        }
     
    }
}