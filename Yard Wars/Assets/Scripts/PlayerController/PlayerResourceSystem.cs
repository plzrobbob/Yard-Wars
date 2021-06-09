using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceSystem : MonoBehaviour
{

    /*
     * Explanation of the player resource system:
     * 
     * This script only tracks the player's funds,
     * and changes the amount of player funds based
     * on minion kills, waves completed, and defences
     * placed. Other scripts should include a game object
     * of this script, and call the Spend and Gain methods
     * as needed. 
     */
    public float starting_funds;
    public float player_funds;

    // Start is called before the first frame update
    void Start()
    {
        player_funds = starting_funds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spend(float value)
    {
        player_funds -= value;
    }

    public void Gain(float value)
    {
        player_funds += value;
    }
}
