/* Description of this ability: Lunch Money: pushes enemy back, causing them to be stunned for _s
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAbilityTwo : MonoBehaviour
{
    [Header("Cooldown")]
    float AbilityTwoCooldown; //This value continuously increments to properly measure how much time has gone by for the cooldown function
    public float maxAbilityTwoCooldown = 6f; //maximum seconds per attack

    [Header("Ability Variables")]
    public bool attackSuccessful = false; //This bool is true when an attack you've made actually hits something that can be attacked
    public int layernum; //What layer are we on right now?
    public float stunDuration; //How long will the enemy be stunned by this ability?
    public float pushMagnitude; //How hard will the enemy be pushed?


    [Header("Hitbox Variables")]
    public float abilityTwoDamageAmount; //The amount of damage Ability Two deals... This doesn't deal any damage, did we want it to???
    public Collider[] HitTargets; //This array stores all the colliders for things we've hit with our attack
    public float area; //The area of our spherical hitbox
    private LayerMask layer; //The layer of the opposing team


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
        //Let's call the function that actually handles ability two
        AbilityTwoController();

        //This value continuously increments to properly measure how much time has gone by for the cooldown function
        AbilityTwoCooldown += Time.deltaTime;
    }

    private void AbilityTwoController()
    {
        if (Input.GetButtonDown("Ability Two") && AbilityTwoCooldown > maxAbilityTwoCooldown)
        {
            //Here we determine that what qualifies as something to enter our "HitTargets" array is anything that enters our overlap sphere in
            //the enemy layer.
            HitTargets = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.75f), area, 
                layer);

            //Now once we have our targets let's bring it over to the function that acutally handles dealing damage
            DoDamage();

            //Debug.Log("Guard Ability 2 bein used dawg!");

            //Let's reset that Cooldown value to get it back to counting up again
            AbilityTwoCooldown = 0;
        }
        else
        {
            //Debug.Log("Damn dawg be patient! Guard Ability Two is still charging up!");
        }
    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            //Within this for loop we will get the health script of whatever we've hit
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();

            //Once we have the health script, we'll subtract from it's current health component by the amount of damage the attack deals
            M_HealthScript.CurrentHealth -= abilityTwoDamageAmount;

            //Debug.Log("Name of thing being damaged is: " + M_HealthScript);

            //Disable Nav Mesh Agent
            HitTargets[i].GetComponent<NavMeshAgent>().enabled = false;

            //Enable nav mesh obstacle
            HitTargets[i].GetComponent<NavMeshObstacle>().enabled = true;

            //Add Rigidbody
            Rigidbody minionRigidBody = HitTargets[i].gameObject.AddComponent<Rigidbody>();

            //Add force to the rigidbody
            Vector3 force = (HitTargets[i].transform.position) - (transform.position);
            minionRigidBody.AddForce(force * pushMagnitude);

            //once velocity = 0 destroy the rigidbody, disable the navmesh obstacle, Enable Nav Mesh Agent, and stun the minion
            StartCoroutine(RigidBodySpeedTest(HitTargets[i].gameObject));
        }
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
        obj.GetComponent<NavMeshAgent>().enabled = true;

        //Declare the current enemy's pathfinding script as a local variable
        Pathfinding pathfindingScript = obj.GetComponent<Pathfinding>();

        //In the current enemy's pathfinding script, activate their stun function to stun them
        pathfindingScript.Stunned(stunDuration);
    }

    //This draws a visual representation of our hitbox when it's set to active
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.75f), area);
    }
}
