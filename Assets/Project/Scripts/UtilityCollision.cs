using UnityEngine;
using System.Collections;

public class UtilityCollision : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.transform.position = transform.GetChild(0).position;

        }
    }
}
