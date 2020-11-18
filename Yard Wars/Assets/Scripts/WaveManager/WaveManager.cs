/* Description: The point of this script is to spawn enemies on each wave. This also means it needs to determine when a wave is
 * going on or not.
 */


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

    public int waveNumber; //This int corrolates to the number of waves that have gone by
    int waveAmount; //The amount of minions spawned during the current wave

    public GameObject minionPrefabTypeOne; //The prefab for enemy type 1 which is: TBD
    public GameObject minionPrefabTypeTwo; //The prefab for enemy type 2 which is: TBD
    public GameObject minionPrefabTypeThree; //The prefab for enemy type 3 which is: TBD
    public GameObject minionPrefabTypeFour; //The prefab for enemy type 4 which is: TBD
    public GameObject minionPrefabTypeFive; //The prefab for enemy type 5 which is: TBD

    private int spawn; //This value correlates to the spawn location used in the current wave. If the number goes above the amount
                       //of spawn locations then enemies spawn in random locations.

    // Start is called before the first frame update
    void Start()
    {
        //Setting up all of our values how we'd like them on startup:
        ready4Wave = true;
        waveInProgress = false;
        spawn = 0;
        waveNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player has pressed the ready key. If they have, call the wave function
        if (Input.GetKeyDown("r"))
        {
            //Check to make sure wave is not in progress
            WaveCheck();

            if (ready4Wave == true && waveInProgress == false)
            {
                //flipping the bools once a wave is in progress
                ready4Wave = false;
                waveInProgress = true;

                Wave(); //The function that actually runs a wave
            }

        }

        //This function is strictly for debug purposes. It kills all enemies currently spawned
        if (Input.GetKeyDown("k"))
        {
            DestroyAllEnemies();
        }

    }

    void WaveCheck() //This function checks how many enemies are on screen. If there are none then it flips the ready4wave bool.
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            ready4Wave = true;
        }

    }

    void Wave()
    {

        //If wave is ready to start, check the wave number and get the amount of minions for that wave using the minion count
        //algorithm:
        waveAmount = waveNumber * Random.Range(5, 10);

        spawn++;
        StartCoroutine(Spawner()); //Actually spawning the enemies is handled by this Coroutine
    }

    IEnumerator Spawner() //Coroutine that actually handles spawning enemies
    {
        //Debug.Log("Coroutine started, bitch!");

        if (spawn > spawnPointList.Count) //This condition is for when enemies spawn from random Spawn Points
        {
            for (int i = 0; i < waveAmount; i++) //loop until the amount of minions for this wave have spawned
            {
                GameObject tempSpawnObject = spawnPointList[Random.Range(0, spawnPointList.Count)]; //Creating a temporary GameObject
                                                                                            //for the purpose of holding the Spawn
                                                                                            //location info of the most recently 
                                                                                            //spawned enemy
                                                                                                    
                Vector3 tempSpawnLocation = tempSpawnObject.transform.position; //The spawn location of the most recently spawned
                                                                                //enemy.

                //The purpose of this switch case is to tell the enemy's AI which pathing nodes to follow depending on the Spawn Point
                //they've been assigned. This switch case was built for a map with 3 spawn points, if a map with more than 3 is used
                //then modify this code to add more cases.
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

                currEnemyPrefab = Instantiate(minionPrefabTypeOne, tempSpawnLocation, Quaternion.identity); //Spawns an enemy
                currEnemyPrefab.GetComponent<Pathfinding>().firstNode = newFirstNode; //Sets the currently spawned enemy's first 
                                                                                      //pathfinding node
                yield return new WaitForSeconds(0.1f); //This is the time between enemy spawns
                //Debug.Log("Holy fuck my guy, did you fucking see that shit spawn in a fucking random location???");
            }
        }
        else
        {
            for (int i = 0; i < waveAmount; i++) //loop until the amount of minions for this wave have spawned
            {
                GameObject tempSpawnObject = spawnPointList[spawn - 1]; //Spawn location in this loop is determined wave number
                Vector3 tempSpawnLocation = tempSpawnObject.transform.position; //The spawn location of the most recently spawned
                                                                                //enemy.

                //The purpose of this switch case is to tell the enemy's AI which pathing nodes to follow depending on the Spawn Point
                //they've been assigned. This switch case was built for a map with 3 spawn points, if a map with more than 3 is used
                //then modify this code to add more cases.
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

                currEnemyPrefab = Instantiate(minionPrefabTypeOne, tempSpawnLocation, Quaternion.identity); //Spawns an enemy
                currEnemyPrefab.GetComponent<Pathfinding>().firstNode = newFirstNode;//Sets the currently spawned enemy's first 
                                                                                     //pathfinding node
                yield return new WaitForSeconds(0.1f); //This is the time between enemy spawns
                //Debug.Log("Holy fuck my guy, did you fucking see that shit spawn in a totally NOT fucking random location???");
            }
        }


        yield return new WaitForSeconds(1f); //why did I put this here?
        //Debug.Log("Bool flipped motherfucker!");
        waveInProgress = false; //If we've reached this part of the code then the spawning is over so we should flip this bool
        waveNumber++; //Increment the wave number now that the wave has ended
    }

    void DestroyAllEnemies() //A debug function to kill off all enemies on the map
    {

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy"); //Create a list of every GameObject tagged "Enemy"

        for (var i = 0; i < enemyList.Length; i++)
        {
            Destroy(enemyList[i]); //DESTROY EVERY FUCKER ON THAT LIST
        }
    }

}
