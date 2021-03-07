/* Description of this ability: Pillow Talk: brings forth pillow and uses it for a shield. Shield health = 201. Do we want to disable
 * attacking while the shield is active?
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        //If you're able to do the ultimate then we call the function for it
        if (Input.GetButtonDown("Ultimate") && canUlt)
        {
            UltimateActivated();
        }

        //If the current instantiated pillow is destroyed prematurely, then the cooldown for the next use of this ability will begin
        if (pillowIsActive == true && thisPillow == null)
        {
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
        
        //Keep pillow active for however many seconds we decide. We make sure to keep this coroutine stored as a variable to prevent duplicates
        currentDuration = StartCoroutine(GuardUltimateDuration(thisPillow));
        

    }

    //This coroutine handles how long the pillow stays active
    IEnumerator GuardUltimateDuration(GameObject currentPillow)
    {
        Debug.Log("Oh fuck my guy, in " + ultimateDuration + " seconds this fucking pillow will cease to be");

        //The duration of the pillow being active
        yield return new WaitForSeconds(ultimateDuration);

        //Destroy the current pillow once the duration is up
        Destroy(currentPillow);

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

}
