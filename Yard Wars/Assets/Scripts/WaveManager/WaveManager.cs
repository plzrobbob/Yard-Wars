using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    bool ready4Wave; // if this bool is true then the wave is over and ready for the next one
    public bool waveInProgress; // if this bool is true then wave spawning is still happening
    public List<GameObject> spawnPointList = new List<GameObject>(); // List of Spawn point objects
    public List<GameObject> firstNodeList = new List<GameObject>(); // List of first nodes corresponding with each spawn point
    GameObject currEnemyPrefab; // The most current enemy prefab, meant to be overwritten at each iteration
    GameObject newFirstNode; // The first node for the pathfinding script to follow along a path for each enemy

   public int waveNumber;
    int waveAmount;

    public GameObject minionPrefabTypeOne;
    public GameObject minionPrefabTypeTwo;
    public GameObject minionPrefabTypeThree;
    public GameObject minionPrefabTypeFour;
    public GameObject minionPrefabTypeFive;

    private int spawn; // n is equal to spawn n 
                        // 1 is equal to spawn 1, 2 is equal to spawn 2, 3 is equal to spawn 3, 4 random pick spawner

    // Start is called before the first frame update
    void Start()
    {
        ready4Wave = true;

        waveInProgress = false;

        spawn = 0;
        waveNumber = 1;

        //Instantiate(minionPrefabTypeOne, spawnPointList[0], Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player has pressed ready. If they have call the wave function
        if (Input.GetKeyDown("r"))
        {
            //Check to make sure wave is not in progress
            WaveCheck();

            if (ready4Wave == true && waveInProgress == false)
            {
                ready4Wave = false;
                waveInProgress = true;

                Wave();
            }

        }

        //This function is strictly for debug purposes. It kills all enemies currently spawned
        if (Input.GetKeyDown("k"))
        {
            DestroyAllEnemies();
        }

        //check if wave is done with GameObject.FindGameObjectsWithTag("Enemy").Length

    }

    void WaveCheck()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            ready4Wave = true;
        }

    }

    void Wave()
    {

        //If wave is ready to start check the wave number and get the amount of minions for that wave using the minion count algorithm:

        waveAmount = waveNumber * Random.Range(5, 10);
        spawn++;
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        //Debug.Log("Coroutine started, bitch!");
        if (spawn > spawnPointList.Count)
        {
            for (int i = 0; i < waveAmount; i++)         //loop until the amount of minions for this wave have spawned
            {
                GameObject tempSpawnObject = spawnPointList[Random.Range(0, spawnPointList.Count)];
                Vector3 tempSpawnLocation = tempSpawnObject.transform.position;

                switch (tempSpawnObject == spawnPointList[0] ? "1" : tempSpawnObject == spawnPointList[1] ? "2" : tempSpawnObject == spawnPointList[2] ? "3" : "Other")
                {
                    case "1":
                        newFirstNode = firstNodeList[0];
                        break;
                    case "2":
                        newFirstNode = firstNodeList[1];
                        break;
                    case "3":
                        newFirstNode = firstNodeList[2];
                        break;
                    case "Other":
                        Debug.Log("Homeboy, what the fucking shit did you do? This shit ain't supposed to happen");
                        break;
                }

                currEnemyPrefab = Instantiate(minionPrefabTypeOne, tempSpawnLocation, Quaternion.identity); //spawn a specified amount of minions at a random spawn point
                currEnemyPrefab.GetComponent<Pathfinding>().firstNode = newFirstNode;
                yield return new WaitForSeconds(0.1f);
                //Debug.Log("Holy fuck my guy, did you fucking see that shit spawn in a fucking random location???");
            }
        }
        else
        {
            for (int i = 0; i < waveAmount; i++)        //loop until the amount of minions for this wave have spawned
            {
                GameObject tempSpawnObject = spawnPointList[spawn - 1];
                Vector3 tempSpawnLocation = tempSpawnObject.transform.position;

                switch (tempSpawnObject == spawnPointList[0] ? "1" : tempSpawnObject == spawnPointList[1] ? "2" : tempSpawnObject == spawnPointList[2] ? "3" : "Other")
                {
                    case "1":
                        newFirstNode = firstNodeList[0];
                        break;
                    case "2":
                        newFirstNode = firstNodeList[1];
                        break;
                    case "3":
                        newFirstNode = firstNodeList[2];
                        break;
                    case "Other":
                        Debug.Log("Homeboy, what the fucking shit did you do? This shit ain't supposed to happen");
                        break;
                }

                currEnemyPrefab = Instantiate(minionPrefabTypeOne, tempSpawnLocation, Quaternion.identity);
                currEnemyPrefab.GetComponent<Pathfinding>().firstNode = newFirstNode;
                yield return new WaitForSeconds(0.1f);
                //Debug.Log("Holy fuck my guy, did you fucking see that shit spawn in a totally NOT fucking random location???");
            }
        }


        yield return new WaitForSeconds(1f);
        //Debug.Log("Bool flipped motherfucker!");
        waveInProgress = false;
        waveNumber++;
    }

    void DestroyAllEnemies()
    {

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        for (var i = 0; i < enemyList.Length; i++)
        {
            Destroy(enemyList[i]);
        }
    }

}
