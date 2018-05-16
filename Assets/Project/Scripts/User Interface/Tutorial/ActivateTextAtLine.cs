using UnityEngine;
using System.Collections;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager textBox;
    public Controller player;

    public bool stopPlayerMovement;
    public bool destroyWhenActivated;




    // Use this for initialization
    void Start () {

        player = FindObjectOfType<Controller>();
        textBox = FindObjectOfType<TextBoxManager>();

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            if (stopPlayerMovement)
            {                
                player.canMove = false;
            }

            textBox.ReloadScript(theText);
            textBox.currentLine = startLine;
            textBox.endLine = endLine;
            textBox.EnableTextBox();

            if(destroyWhenActivated)
            {
                stopPlayerMovement = false;
                Destroy(gameObject);
            }
        }
    }

}
