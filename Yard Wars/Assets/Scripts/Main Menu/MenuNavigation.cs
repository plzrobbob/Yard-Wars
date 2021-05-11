using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    enum MenuState
    {
        Title,
        MainMenu,
        Settings,
        Singleplayer,
        Multiplayer
    };

    MenuState currentState;


    [Header("Menu Screen Game Objects")]
    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Settings;
    [SerializeField] GameObject Singleplayer;
    [SerializeField] GameObject Multiplayer;

    [Header("Audio")]
    [SerializeField] MenuAudioManager AudioManager;

    [Header("Animations")]
    [SerializeField] Animator MenuAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentState = MenuState.Title;
    }

    // Update is called once per frame
    void UpdateMenuState()
    {
        // Choose new menu to activate based on state and animate
        switch(currentState)
        {
            case MenuState.Title:
                TitleScreen.SetActive(true);
                // Fade in anim
                break;
            case MenuState.MainMenu:
                MainMenu.SetActive(true);
                // Fade in anim
                break;
            case MenuState.Settings:
                Settings.SetActive(true);
                // Fade in anim
                break;
            case MenuState.Singleplayer:
                Singleplayer.SetActive(true);
                // Fade in anim
                break;
            case MenuState.Multiplayer:
                Multiplayer.SetActive(true);
                // Fade in anim
                break;
            default:    // MenuState glitched, reset all menus and states, restart
                ResetMenuState();
                break;
        }
    }

    void ResetMenuState()
    {
        currentState = MenuState.Title;
        TitleScreen.SetActive(false);
        MainMenu.SetActive(false);
        Settings.SetActive(false);
        Singleplayer.SetActive(false);
        Multiplayer.SetActive(false);
        UpdateMenuState();
    }

    public void OnClick_ExitGame()
    {
        Application.Quit();
    }

    public void OnClick_TitleScreen()
    {
        AudioManager.Confirm();

        currentState = MenuState.MainMenu;

        //Play Animation of menu closing out

        UpdateMenuState();
    }
}
