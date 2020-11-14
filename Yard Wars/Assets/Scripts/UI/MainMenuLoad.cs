using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLoad : MonoBehaviour
{
    public GameObject immortal;
    public bool isLoaded = false;
    public void LoadGame()
    {
        isLoaded = true;
        DontDestroyOnLoad(immortal);
    }
}
