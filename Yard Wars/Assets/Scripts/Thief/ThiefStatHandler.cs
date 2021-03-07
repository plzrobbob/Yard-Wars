/* Description: This script holds all the stats of the Thief class
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefStatHandler : MonoBehaviour
{
    private float ThiefHealth = 75f; //Max Health
    private float ThiefDamage = 10f; //Base damage dealt by weapon
    private float ThiefSpeed = 12f; //Base speed
    private float JumpCount = 1f; //How many jumps this boy got in him?
    private float ThiefMaxMeleeCooldown = 0.5f; //Time between next possible melee attack
    private float AbilityTwoDamage = 50f; //How much damage does Ability two deal?
    private float AbilityTwoRange = 10f; //How far does ability two's range extend?
    private bool CanThiefJump = true; //Can this boy jump at all??????

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<HealthScript>().MaxHealth = ThiefHealth;
        this.GetComponent<HealthScript>().CurrentHealth = ThiefHealth;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = ThiefSpeed;
        this.GetComponent<PlayerCharacterController>().MaxJmpCount = JumpCount;
        this.GetComponent<PlayerCharacterController>().CanThiefJump = CanThiefJump;
        this.GetComponent<MeleeWeaponFire>().damageAmount = ThiefDamage;
        this.GetComponent<MeleeWeaponFire>().maxWeaponCooldown = ThiefMaxMeleeCooldown;
        //this.GetComponent<ThiefAbilityTwo>().AbilityDamage = AbilityTwoDamage;
        //this.GetComponent<ThiefAbilityTwo>().AbilityRange = AbilityTwoRange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
