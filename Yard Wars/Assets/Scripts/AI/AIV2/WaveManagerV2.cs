using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerV2 : MonoBehaviour
{
    public bool CoInProgress; // if this bool is true then wave spawning is still happening

    public GameObject minionPrefabTypeOne;//different minion types
    public GameObject minionPrefabTypeTwo;
    public GameObject minionPrefabTypeThree;
    public GameObject minionPrefabTypeFour;
    public GameObject minionPrefabTypeFive;
    public PathHolder[] path1Holder;//holds the paths, including the nodes for the minions, and the spawn pos

    private Coroutine Co;

    public float waveCooldownTimer;

    public bool ResetWave;

    [SerializeField]
    private int waveNumber;


    void Start()
    {
        CoInProgress = false;
        ResetWave = false;
        waveNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player has pressed ready. If they have call the wave function
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Debug.Log("trySpawn");
            if (WaveCheck() && !CoInProgress)//if enemies are not alive then start a new wave routine.
            {
                Wave();
            }
        }

        //This function is strictly for debug purposes. It kills all enemies currently spawned
        if (Input.GetKeyDown(KeyCode.O))
        {
            DestroyAllEnemies();
        }

        //This function is strictly for debug purposes. It kills all enemies currently spawned and resets the wave coroutine
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetWave = true;
            DestroyAllEnemies();
        }
    }
    void Wave()//begin the wave coroutine
    {
        waveNumber = 1;
        CoInProgress = true;
        ResetWave = false;

        try
        {
            StopCoroutine(Co);
        }
        catch { }
        Co = StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (!ResetWave)//this will continue indefinately.  or until debugstopwave is called
        {
            int MinionSpawnAmount = waveNumber * Random.Range(8, 10);//get the current amount of minions for that wave
            int LanesToSpawn = waveNumber;

            if (waveNumber > path1Holder.Length)//spawn minions in at most path1Holder.Length lanes
            {
                LanesToSpawn = path1Holder.Length;
            }
            int minionsPerSpawn = MinionSpawnAmount / LanesToSpawn;//get the mions to spawn per lane.  If lanes to spawn is 20, but we only have 3 lanes, this will be a small number.  Thus the above if statement is necessary

            for (int i = 0; i < LanesToSpawn; i++)//spawn minions in a specifed number of lanes
            {
                for (int j = 0; j < minionsPerSpawn; j++)//spawn only a specified amount of minions for each wave
                {
                    GameObject obj = Instantiate(minionPrefabTypeOne, path1Holder[i].SpawnPos.transform.position, this.transform.rotation, this.transform);//spawn the minion prefab type 1 at the spawn pos
                    obj.GetComponent<AINodePath>().path = path1Holder[i].path;//this will send the nodes to the currently spawned minion
                    obj.GetComponent<AINodePath>().TheOrb = path1Holder[i].Goal;//this will send the nodes to the currently spawned minion
                }
            }

            bool waveFinished = false;
            while (!waveFinished)//spawn next wave after the current wave is defeated.  This needs to be changed for multipalyer so that it spawns in intervals
            {
                waveFinished = WaveCheck();
                yield return null;
            }

            if (!ResetWave)
            {
                yield return new WaitForSeconds(waveCooldownTimer);
            }
            waveNumber++;
        }

        CoInProgress = false;//routine ahs finished
    }


    bool WaveCheck()//check to see if any enemies are alive
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void DestroyAllEnemies()//kill all enemies
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (var i = 0; i < enemyList.Length; i++)
        {
            Destroy(enemyList[i]);
        }
    }
}
