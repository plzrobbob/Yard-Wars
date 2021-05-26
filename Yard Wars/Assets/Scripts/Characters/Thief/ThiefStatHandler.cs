using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefStatHandler : MonoBehaviour
{
    private float ThiefHealth = 75f;
    private float ThiefDamage = 10f;
    private float ThiefSpeed = 12f;
    private float AbilityTwoDamage = 50f;
    private float AbilityTwoRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<HealthScript>().MaxHealth = ThiefHealth;
        this.GetComponent<HealthScript>().CurrentHealth = ThiefHealth;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = ThiefSpeed;
        //this.GetComponent<ThiefAbilityTwo>().AbilityDamage = AbilityTwoDamage;
        //this.GetComponent<ThiefAbilityTwo>().AbilityRange = AbilityTwoRange;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float ThiefHealthFunction()
    {
        return ThiefHealth;
    }

    public float ThiefDamageFunction()
    {
        return ThiefDamage;
    }

    public float ThiefSpeedFunction()
    {
        return ThiefSpeed;
    }
}

