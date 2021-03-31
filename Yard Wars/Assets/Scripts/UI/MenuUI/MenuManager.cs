using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject CampaignMenu;
    public GameObject VersusMenu;
    public GameObject Campaign_LobbyMenu;
    public GameObject Versus_LobbyMenu;
    public int MissionSelect;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void startMenu()
    {
        MainMenu.gameObject.SetActive(true);
        StartMenu.gameObject.SetActive(false);
    }
    public void mainMenu(GameObject prevMenu)
    {
        MainMenu.gameObject.SetActive(true);
        prevMenu.gameObject.SetActive(false);
    }

    public void Campaign_Menu()
    {
        CampaignMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
    }

    public void Campaign_Back_Menu(GameObject prevMenu)
    {
        CampaignMenu.gameObject.SetActive(true);
        prevMenu.gameObject.SetActive(false);
    }
    public void Versus_Menu()
    {
        VersusMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
    }

    public void Coop_Lobby(GameObject prevMenu)
    {
        Campaign_LobbyMenu.gameObject.SetActive(true);
        prevMenu.gameObject.SetActive(false);
    }

    public void SetMission(int mission)
    {
        MissionSelect = mission;
    }
    public void Versus_lobby(GameObject prevMenu)
    {
        Versus_LobbyMenu.gameObject.SetActive(true);
        prevMenu.gameObject.SetActive(false);
    }

    public void OptionsMenu()
    {
        SettingsMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void OnFadeOutComplete()
    {
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);

        //PlayerPrefs.SetInt("LevelToLoad", 2);
    }

}
