using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Singleplayer Level Preview Images")]
    [SerializeField] Image LevelPreviewImage;
    [SerializeField] List<Sprite> PreviewImages;

    // Start is called before the first frame update
    void Start()
    {
        currentState = MenuState.Title;
        UpdateMenuState();
    }

    void Update()
    {
        if(currentState == MenuState.Title && Input.anyKeyDown)
        {
            OnClick_TitleScreen();
        }
    }

    // Update is called once per frame
    void UpdateMenuState()
    {
        TitleScreen.SetActive(false);
        MainMenu.SetActive(false);
        Settings.SetActive(false);
        Singleplayer.SetActive(false);
        Multiplayer.SetActive(false);

        // Choose new menu to activate based on state and animate
        switch (currentState)
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
                currentState = MenuState.Title;
                UpdateMenuState();
                break;
        }
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

        //TitleScreen.SetActive(false);

        UpdateMenuState();
    }

    public void OnClick_MainMenu()
    {
        AudioManager.Select();
        currentState = MenuState.MainMenu;

        UpdateMenuState();
    }

    public void OnClick_Settings()
    {
        AudioManager.Select();
        currentState = MenuState.Settings;

        UpdateMenuState();
    }

    public void OnClick_Singleplayer()
    {
        AudioManager.Select();
        currentState = MenuState.Singleplayer;

        UpdateMenuState();

        LevelPreviewImage.sprite = PreviewImages[0];
    }

    public void OnClick_Multiplayer()
    {
        AudioManager.Select();
        currentState = MenuState.Multiplayer;

        UpdateMenuState();
    }

    public void OnClick_ConfirmSettings()
    {
        Debug.Log("Confirm Settings Clicked");
    }

    public void OnClick_UpdateLevelImage(int selectedLevel)
    {
        LevelPreviewImage.sprite = PreviewImages[selectedLevel];
    }

    public void OnClick_SingleplayerLoadLevel()
    {
        Debug.Log("Singleplayer Load Level Clicked");
    }

    public void OnClick_MultiplayerJoinGame()
    {
        Debug.Log("Multiplayer Join Game Clicked");
    }
}
