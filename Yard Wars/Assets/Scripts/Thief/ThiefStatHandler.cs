using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefStatHandler : MonoBehaviour
{
    private float ThiefHealth = 75f;
    private float ThiefDamage = 10f;
    private float ThiefSpeed = 12f;
    private float JumpCount = 1f;
    private float ThiefMaxMeleeCooldown = 0.5f; //time between next possible melee attack
    private float AbilityTwoDamage = 50f;
    private float AbilityTwoRange = 10f;
    private bool CanThiefJump = true;

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
