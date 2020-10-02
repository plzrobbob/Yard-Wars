using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompletionManager : MonoBehaviour
{
    private WaveManager wavenums;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        wavenums = GetComponent<WaveManager>();
        target = GameObject.FindGameObjectWithTag("MinionGoal");
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.Log("GameLost");
            SceneManager.LoadScene(1);
        }
        else if (wavenums.waveNumber >10)
        {
            Debug.Log("GameWon");
            SceneManager.LoadScene(1);
        }
    }
}
