/* Description of this ability: Pillow Talk: brings forth pillow and uses it for a shield. Shield health = 201. The pillow takes damage for you
 * and every second while the ability is active you push back every enemy around you
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardUltimateAbility : MonoBehaviour
{
    [Header("Cooldown")]
    public float ultimateCooldown = 6f; //The amount of time between uses of this ability
    public float ultimateDuration = 10f; //The amount of time this ability stays active
    bool inCooldown = false; //This bool is true if we are currently in the cooldown state between Ultimates
    bool canUlt = true; //This bool is true when the player is able to do their Ultimate

    [Header("Ability Variables")]
    public GameObject pillowObject; //The prefab for the pillow the player will use as a shield
    bool pillowIsActive = false; //This bool is true if there is currently a pillow instantiated
    GameObject thisPillow; //The current instantiated pillow
    Coroutine currentDuration; //The current Duration coroutine going on. We'll use this to stop a specific coroutine if the pillow is destroyed
                               //prematurely
    float tempHealth = 201; //The amount of health the pillow has. We add this to the player's current health
    float originalHealth;
    HealthScript P_HealthScript; //The health script of the player
    bool inPushBack = false; //Is there currently a pushback coroutine going on?
    public float pushMagnitude; //How hard will the enemy be pushed?
    public float pushCooldown; //How many seconds between pushes

    [Header("Hitbox Variables")]
    public Collider[] HitTargets; //This array stores all the colliders for things we've hit with our attack
    public float area; //The area of our spherical hitbox
    public int layernum; //What layer are we on right now?
    private LayerMask layer; //The layer of the opposing team

    private void Start()
    {
        P_HealthScript = this.gameObject.GetComponent<HealthScript>(); //Declare the health script of the player

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
        //If you're able to do the ultimate then we call the function for it
        if (Input.GetButtonDown("Ultimate") && canUlt)
        {
            UltimateActivated();
        }

        if (pillowIsActive == true)
        {
            //RedirectDamage();

            if (inPushBack == false)
            {
                StartCoroutine(PillowPushBack());
            }
        }

        //If the current instantiated pillow is destroyed prematurely, then the cooldown for the next use of this ability will begin
        if (pillowIsActive == true && P_HealthScript.CurrentHealth <= originalHealth)
        {
            Debug.Log("Premature pillow destruction is a problem that a lot of men deal with it really isn't so bad.");

            Destroy(thisPillow);

            pillowIsActive = false;
        }

        //After Pillow deactivates or is destroyed, start cooldown coroutine
        if (pillowIsActive == false && canUlt == false && inCooldown == false)
        {
            //If a duration coroutine is going on then this will stop it dead in it's tracks
            StopCoroutine(currentDuration);

            StartCoroutine(GuardUltimateCooldown());
        }
    }

    void UltimateActivated()
    {
        Debug.Log("By golly gee whiz this Guard sure is doing a fucking Ultimate right now ain't he?");

        //The Ultimate is currently going on so we don't want the player to be able to Ult again
        canUlt = false;

        //Instantiate Pillow Gameobject
        thisPillow = Instantiate(pillowObject, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2),
            Quaternion.identity, transform);

        //Now that a pillow is instantiated we flip the bool to let the rest of the script know that
        pillowIsActive = true;

        //How much health does the player have the MOMENT a pillow is spawned?
        //tempHealth = P_HealthScript.CurrentHealth; 

        //Before we add the temp health, how much health did we have before that?
        originalHealth = P_HealthScript.CurrentHealth;

        //Add the temp health
        P_HealthScript.CurrentHealth += tempHealth;

        //Keep pillow active for however many seconds we decide. We make sure to keep this coroutine stored as a variable to prevent duplicates
        currentDuration = StartCoroutine(GuardUltimateDuration(thisPillow));
        

    }

    //The purpose of this function is that any time damage is dealt while a pillow is active it will be redirected to the pillow
    /*void RedirectDamage()
    {
        if (P_HealthScript.CurrentHealth != tempHealth) //this tells us if the player has taken damage
        {
            float tempDamage = tempHealth - P_HealthScript.CurrentHealth; //How much damage was dealt?

            P_HealthScript.CurrentHealth = tempHealth; //Undo the damage done

            HealthScript Pillow_HealthScript = thisPillow.gameObject.GetComponent<HealthScript>(); //Declare temp variable to hold the pillow's 
                                                                                                   //health script

            Pillow_HealthScript.CurrentHealth -= tempDamage; //Deal that damage to the pillow
        }

        tempHealth = P_HealthScript.CurrentHealth; //Reset the tempHealth value
    }*/

    //This coroutine handles how long the pillow stays active
    IEnumerator GuardUltimateDuration(GameObject currentPillow)
    {
        Debug.Log("Oh fuck my guy, in " + ultimateDuration + " seconds this fucking pillow will cease to be");

        //The duration of the pillow being active
        yield return new WaitForSeconds(ultimateDuration);

        //Destroy the current pillow once the duration is up
        Destroy(currentPillow);

        //How much damage did we take?
        float damageTaken = originalHealth + tempHealth - P_HealthScript.CurrentHealth;
        Debug.Log(damageTaken + " is the amount of the damageTaken Value");

        //We lost less damage than we gained so we need to take away the rest of the temp HP
        if (damageTaken < tempHealth)
        {
            Debug.Log(tempHealth + " is the amount of the tempHealth Value");
            Debug.Log(damageTaken + " is the amount of the damageTaken Value");
            Debug.Log(originalHealth + " is the amount of the originalHealth Value");

            //Amount of temp HP to take away
            float hpToLose = tempHealth - damageTaken;

            Debug.Log("Fuck dawg the player is about to lose " + hpToLose + " health. What a fuckin' bummer.");

            P_HealthScript.CurrentHealth -= hpToLose;
        }

        //If the pillow is destroyed then uh yeah the fuckin pillow ain't active no more
        pillowIsActive = false;

        Debug.Log("Golly gee FUCKING willickers motherfucker the Guard Ultimate Duration coroutine has fucking ended for fuck's sake");
    }

    //This coroutine handles the cooldown between ultimates
    IEnumerator GuardUltimateCooldown()
    {
        //We are in fact, currently in a cooldown
        inCooldown = true;

        Debug.Log("Well shit my dude, in " + ultimateCooldown + " seconds you'll be able to spawn another fucking pillow");

        //Wait for the duration we specified would be the cooldown
        yield return new WaitForSeconds(ultimateCooldown);

        //We can now use the Ultimate again
        canUlt = true;

        //We are no longer in a cooldown phase
        inCooldown = false;

        Debug.Log("Well I'll be a motherfucking scumsucking bottom feeder son of a bitch you can spawn another pillow. Good job!");
    }

    //This coroutine handles the pillow push back
    IEnumerator PillowPushBack()
    {
        Debug.Log("Motherfuckin' pillow pushback is happening RIGHT NOW");

        //We are currently in the process of pushing enemies back
        inPushBack = true; 

        //Here we determine that what qualifies as something to enter our "HitTargets" array is anything that enters our overlap sphere in
        //the enemy layer.
        HitTargets = Physics.OverlapSphere(transform.position, area, layer);

        for (int i = 0; i < HitTargets.Length; i++)
        {
            //Within this for loop we will get the health script of whatever we've hit
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();

            //Debug.Log("Name of thing being pushed back is: " + M_HealthScript);

            //Disable Nav Mesh Agent
            //HitTargets[i].GetComponent<NavMeshAgent>().enabled = false;

            //Enable nav mesh obstacle
            HitTargets[i].GetComponent<NavMeshObstacle>().enabled = true;

            //Cameron told me to add this for some fuckin reason it makes it all work idk
            HitTargets[i].GetComponent<NavMeshAgent>().isStopped = true;

            //Add Rigidbody
            Rigidbody minionRigidBody = HitTargets[i].gameObject.AddComponent<Rigidbody>();

            //Add force to the rigidbody
            Vector3 force = (HitTargets[i].transform.position) - (transform.position);
            minionRigidBody.AddForce(force * pushMagnitude);

            //once velocity = 0 destroy the rigidbody, disable the navmesh obstacle, Enable Nav Mesh Agent, and stun the minion
            StartCoroutine(RigidBodySpeedTest(HitTargets[i].gameObject));
        }

        yield return new WaitForSeconds(pushCooldown);

        //We are NOT currently in the process of pushing enemies back
        inPushBack = false;
    }

    IEnumerator RigidBodySpeedTest(GameObject obj)
    {
        //Wait about half a second to allow time for the enemy to be pushed back
        yield return new WaitForSeconds(0.5f);

        //Declare the current enemy's rigidbody as a local variable
        Rigidbody minionRB = obj.GetComponent<Rigidbody>();

        //Destroy said local variable
        Destroy(minionRB);

        //The enemy is no longer a Nav Mesh Obstacle
        obj.GetComponent<NavMeshObstacle>().enabled = false;

        //The enemy once again has it's Nav Mesh Agent so it can continue to pathfind
        //obj.GetComponent<NavMeshAgent>().enabled = true;

        //Cameron told me to add this for some fuckin reason it makes it all work idk
        obj.GetComponent<NavMeshAgent>().isStopped = false;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, area);
    }

}
