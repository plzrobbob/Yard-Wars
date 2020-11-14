using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  //Really important

public class HUD : MonoBehaviour
{
  //  public PauseMenu pausemenuscript; //to take the bool IsPaused from pausemenu script           IGNORE LINES 8 AND 9 IT WAS USELESS CAUSE ANOTHER METHOD WAS FOUNDED THAT WAS BETTER
   // public GameObject HUDpanel; // To connect the HUD to the SETACTIVE components
    
    public CoverScript coverscript;  // This line is a bit weird. If you want to know when the PLAYER is inCover. Don't take a related script. 
                                     // You HAVE to take the PLAYER GAMEOBJECT into the PUBLIC COVERSCRIPT empty slot. It will then
                                     // take in the GAMEOBJECT but specifically look into the gameobject's coverscript. Therefore taking in only when
                                     // the player himself has entered cover. Its wierd but makes sense
    public string walking = "walking";  
    public string standing = "standing";
    public Text stance;  // To connect the script to the text you want to change.
    bool isMoving = false;

    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode interact = KeyCode.E;
    public KeyCode dash = KeyCode.Space;
    public KeyCode cover = KeyCode.LeftControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(moveLeft) || Input.GetKey(moveRight) || Input.GetKey(moveUp) || Input.GetKey(moveDown))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (isMoving)
        {
            stance.text = walking;
            if(coverscript.inCover)
            {
                Debug.Log("At some point I was in cover");  // A test to see if the script is referencing correctly
                stance.text = "taking cover";
            }
        }
       
        else
        {
            stance.text = standing;
            if (coverscript.inCover)
            {
                stance.text = "taking cover";
            }
            
        }
    }
}
