/* Description: This script holds all the stats of the Guard class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStatHandler : MonoBehaviour
{
    private float GuardHealth = 150f; //Base health
    private float GuardDamage = 30f; //Base damage dealt by weapon
    private float GuardSpeed = 8.5f; //Base speed
    private float GuardMaxMeleeCooldown = 0.75f; //Time between next possible melee attack
    private float AbilityOneDamage = 45f; //How much damage does ability one deal?
    private float AbilityTwoDamage = 25f; //How much damage does ability two deal?


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<HealthScript>().MaxHealth = GuardHealth;
        this.GetComponent<HealthScript>().CurrentHealth = GuardHealth;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = GuardSpeed;
        this.GetComponent<MeleeWeaponFire>().damageAmount = GuardDamage;
        this.GetComponent<MeleeWeaponFire>().maxWeaponCooldown = GuardMaxMeleeCooldown;
        this.GetComponent<GuardAbilityOne>().abilityOneDamageAmount = AbilityOneDamage;
        this.GetComponent<GuardAbilityTwo>().abilityTwoDamageAmount = AbilityTwoDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GuardHealthFunction()
    {
        return GuardHealth;
    }

    public float GuardDamageFunction()
    {
        return GuardDamage;
    }

    public float GuardSpeedFunction()
    {
        return GuardSpeed;
    }
}
