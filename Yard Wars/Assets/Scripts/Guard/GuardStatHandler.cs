using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStatHandler : MonoBehaviour
{
    private float GuardHealth = 150f;
    private float GuardDamage = 30f;
    private float GuardSpeed = 8.5f;
    private float GuardMaxMeleeCooldown = 0.75f; //time between next possible melee attack
    private float AbilityOneDamage = 45f;
    private float AbilityTwoDamage = 25f;


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
}
