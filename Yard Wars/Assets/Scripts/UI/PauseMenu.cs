using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool IsPaused = false;  //Begins at a false state so when you press escape it makes it true. Pressing esc again makes it unpause.
    public GameObject pauseMenuUI; //script needs to know where the Pause Menu is. Drag it in when your done.
    public GameObject HUDpanel;
    public AlertHUD AlreadyAlerted;
    public bool isSpicyMus = false;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("RegularMusic");
        HUDpanel.SetActive(true);
        pauseMenuUI.SetActive(false); // To make sure the pause menu only shows up when its key is pressed.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume(); //function is below. 
            }
            else
            {
                Pause(); //ITS BELOW
            }
        }
    }
    
    public void Resume()
    {
        HUDpanel.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; //resumes life as we know it. 
        IsPaused = false;
        FindObjectOfType<AudioManager>().Pause("PauseMusic");
        
        if(!AlreadyAlerted.AlreadyAlerted)
        {
            FindObjectOfType<AudioManager>().Play("RegularMusic");
        }
        else if(AlreadyAlerted.AlreadyAlerted)
        {
            FindObjectOfType<AudioManager>().Play("AlertMusic");
        }
        
    }

    void Pause()
    {
        HUDpanel.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //STOP. Time has halted. time has reached 0F. which means no time.
        IsPaused = true;
        FindObjectOfType<AudioManager>().Play("PauseMusic");

        if (!AlreadyAlerted.AlreadyAlerted)
        {
            FindObjectOfType<AudioManager>().Pause("RegularMusic");
        }
        else if (AlreadyAlerted.AlreadyAlerted)
        {
            FindObjectOfType<AudioManager>().Pause("AlertMusic");
        }

    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("Main Menu"); //loads the Main Menu scene. Simple as that.
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
