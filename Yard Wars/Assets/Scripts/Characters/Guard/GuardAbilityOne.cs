/* Description of this ability: Swings his justice claymore in  wide arc in front of him cleaving enemies and slowing them for 1 second.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAbilityOne : MonoBehaviour
{
    [Header("Cooldown")]
    float AbilityOneCooldown; //This value continuously increments to properly measure how much time has gone by for the cooldown function
    public float maxAbilityOneCooldown = 6f; //maximum seconds per attack

    [Header("Ability Variables")]
    public bool attackSuccessful = false; //This bool is true when an attack you've made actually hits something that can be attacked
    public float abilityOneDamageAmount; //How much damage this ability will deal
    public float slowPercent; //Percentage value of how much an enemy will be slowed by this ability. originally this was set to: 0.5f
    public float slowDuration; //How long the enemy will be slowed down. originally this was set to: 5f

    [Header("Hitbox Variables")]
    public Collider[] HitTargets; //This array stores all the colliders for things we've hit with our attack
    //private Vector3 overlapCheck;
    public float area; //The area of each of our spherical hitboxes
    public int layernum; //What layer are we on right now?
    private LayerMask layer; //The layer of the opposing team
    public Transform sphereOne; //This sphere makes up the left half of the capsule hitbox for this attack
    public Transform sphereTwo; //This sphere makes up the right half of the capsule hitbox for this attack

    // Start is called before the first frame update
    void Start()
    {
        //This tells us which layer the opposing team is on so that the script knows which people to attack
        if (layernum == 20)
        {
            layer = LayerMask.GetMask("Team2");
        }
        else if (layernum == 21)
        {
            layer = LayerMask.GetMask("Team1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //The function that actually controls the ability
        AbilityOneController();

        //This value continuously increments to properly measure how much time has gone by for the cooldown function
        AbilityOneCooldown += Time.deltaTime;
    }

    private void AbilityOneController()
    {
        if (Input.GetButtonDown("Ability One") && AbilityOneCooldown > maxAbilityOneCooldown)
        {
            //Here we determine that what qualifies as something to enter our "HitTargets" array is anything that enters our overlap capsule in
            //the enemy layer.
            HitTargets = Physics.OverlapCapsule(sphereOne.position, sphereTwo.position, area, layer);

            //We call the function that handles damage
            DoDamage();

            //We reset the cooldown variable to start counting again
            AbilityOneCooldown = 0;

            //Debug.Log("Ability one currently engaged motherfucker!");
        }
        else
        {
            //Debug.Log("Ability One is still charging up dude, quit mashin the button!");
        }

    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            //Within this for loop we will get the health script of whatever we've hit
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();

            //Once we have the health script, we'll subtract from it's current health component by the amount of damage the attack deals
            M_HealthScript.CurrentHealth -= abilityOneDamageAmount;

            //Within this for loop we will get the pathfinding script of whatever we've hit
            AINodePath pathfindingScript = HitTargets[i].GetComponent<AINodePath>();

            //Once we have the pathfinding script, we'll use it to slow down the enemy hit by the amount and duration specified
            pathfindingScript.Slowed(slowPercent, slowDuration);

            //Debug.Log("Name of thing being damaged is: " + M_HealthScript);
        }
    }

    //This draws a visual representation of our hitbox when it's set to active
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(sphereOne.position, area);
        //Gizmos.DrawSphere(sphereTwo.position, area);
    }
}
