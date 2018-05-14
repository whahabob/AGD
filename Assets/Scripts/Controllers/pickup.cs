using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pickup : MonoBehaviour {

    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;

    void onCollisionEnter(Collision collision)
    {

        foreach(Transform child in inventoryPanel.transform)
        {
            Debug.Log("Child vind die");
            if (child.gameObject.tag == collision.gameObject.tag)
            {
                
                string c = child.Find("Text").GetComponent<Text>().text;
                int tcount = System.Int32.Parse(c) + 1;
                child.Find("Text").GetComponent<Text>().text = "" + tcount;
                return;
            }
        }

        GameObject i;
        if(collision.gameObject.tag == "red")
        {
            i = Instantiate(inventoryIcons[0]);
            i.transform.SetParent(inventoryPanel.transform);
        }
    }

	
}
