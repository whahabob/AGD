using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class invCollect : MonoBehaviour {

    public AudioClip coinCollect;

    private int tcount;
    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;

    // Use this for initialization
    void Start () {
		inventoryPanel = GameObject.Find ("Inventory");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
       // Debug.Log("Komt hierin begin");
        if (col.gameObject.tag == "Coin")
        {
          //  Debug.Log("Komt hierin");
            foreach (Transform child in inventoryPanel.transform)
            {
            //    Debug.Log("Komt hierin foreach");
                if (child.gameObject.tag == "Coin")
                {
              //      Debug.Log("Komt hierin child");
                    string c = child.Find("Text").GetComponentInChildren<Text>().text;
                //    Debug.Log("heeft gereturned" + c);
                    tcount = System.Int32.Parse(c) + 1;
                  //  Debug.Log("heeft gereturned after" + tcount);
                    child.Find("Text").GetComponentInChildren<Text>().text = "" + tcount;
                    AudioSource.PlayClipAtPoint(coinCollect, transform.position);
                    PlayerStats.score++;
                    Destroy(col.gameObject);
                    return;
                }
            }
            //  GameObject i
            
            //   i = Instantiate(inventoryIcons[0]);
            //   i.transform.SetParent(inventoryPanel.transform);

        }

        GameObject i;
        if (col.gameObject.tag == "Coin")
        {
            i = Instantiate(inventoryIcons[1]);
            i.transform.SetParent(inventoryPanel.transform);
            AudioSource.PlayClipAtPoint(coinCollect, transform.position);
            PlayerStats.score++;
            Destroy(col.gameObject);
        }


        if (col.gameObject.tag == "Key")
        {
               i = Instantiate(inventoryIcons[0]);
               i.transform.SetParent(inventoryPanel.transform);
        }
        

    }


}
