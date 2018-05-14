using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBoxManager : MonoBehaviour {

    public GameObject textBox;
    public Text theText;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endLine;

    public Controller player;

    public bool isActive;

    public bool stopPlayerMovement;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Controller>();      
        

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));

        }

        if(endLine == 0)
        {
            endLine = textLines.Length - 1;
        }

        if(isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
       
    }

    void Update()
    {
        theText.text = textLines[currentLine];

        if (!isActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine++;
        }
        
        if(currentLine > endLine)
        {
            DisableTextBox();

        }
    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;

        if(stopPlayerMovement)
        {
            player.canMove = false;
        }
    }

    public void DisableTextBox()
    {
        textBox.SetActive(false);
        isActive = false;

        player.canMove = true;
    }

    public void ReloadScript(TextAsset theText)
    {
        if(theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }

    }
}
