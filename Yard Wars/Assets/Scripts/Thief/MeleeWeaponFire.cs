/* Description: The purpose of this script is for character classses that use melee weapons instead of guns.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponFire : MonoBehaviour
{
    [Header("General Weapon Function")]
    public GameObject Weapon; //The actual GameObject for our melee Weapon
    public PlaceDefense m_placeDefense; //This is used to place walls and I basically ripped it from the gun fire script
    public bool attackSuccessful = false; //This bool is true when an attack you've made actually hits something that can be attacked
    public float damageAmount; //How much damage melee attacks do

    [Header("Cooldown")]
    private float WeaponCooldown; //This value continuously increments to properly measure how much time has gone by for the cooldown function
    public float maxWeaponCooldown = 0.5f; //Maximum seconds per attack

    [Header("Hitbox Variables")]
    public Collider[] HitTargets; //This array stores all the colliders for things we've hit with our attack
    public float area; //The area of our spherical hitbox
    public int layernum; //What layer are we on right now?
    private LayerMask layer; //The layer of the opposing team

    private void Start()
    {
        //This variable here references the script for placing defenses. With this here our character can put down walls when not attacking
        m_placeDefense = this.GetComponentInChildren<PlaceDefense>();

        //Debug.Log("layernum equals " + layernum);

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

    private void Update()
    {
        //This is the function that controls attacking (Fuckin, DUH)
        AttackController();

        //This value continuously increments to properly measure how much time has gone by for the cooldown function
        WeaponCooldown += Time.deltaTime; 
    }

    private void AttackController()
    {
        //Gotta make sure we aren't in the middle of defense placing or still waiting for the cooldown to end
        if (Input.GetButtonDown("Fire1") && WeaponCooldown > maxWeaponCooldown && !m_placeDefense.placing)
        {
            //Here we determine that what qualifies as something to enter our "HitTargets" array is anything that enters our overlap sphere in
            //the enemy layer.
            HitTargets = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.75f), area, 
                layer);

            //Now once we have our targets let's bring it over to the function that acutally handles dealing damage
            DoDamage();

            //Debug.Log("Attack underway!");

            //Set our weapon cooldown back down to zero so it can start counting up again
            WeaponCooldown = 0;
        } 

    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            //Within this for loop we will get the health script of whatever we've hit
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();

            //Once we have the health script, we'll subtract from it's current health component by the amount of damage the attack deals
            M_HealthScript.CurrentHealth -= damageAmount;

            //Debug.Log("Name of thing being damaged is: " + M_HealthScript);
        }
    }

    //This draws a visual representation of our hitbox when it's set to active
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.75f), area);
    } 


}
