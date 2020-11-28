using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierStatHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private float GrenadierHealth = 120f;
    private float GrenadierDamage = 10f;
    private float GrenadierSpeed = 10f;
    private float AbilityOneDamage = 50f;
    private float AbilityOneRange = 5f;
    private float AbilityTwoDamage = 40f;
    private float AbilityTwoDamageRange = 2.5f;

    void Start()
    {
        this.GetComponent<HealthScript>().MaxHealth = GrenadierHealth;
        this.GetComponent<HealthScript>().CurrentHealth = GrenadierHealth;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = GrenadierSpeed;
        this.GetComponent<GrenadierAbilities>().AbilityOneDamage = AbilityOneDamage;
        this.GetComponent<GrenadierAbilities>().AbilityOneRange = AbilityOneRange;
        this.GetComponent<GrenadierAbilities>().AbilityTwoDamage = AbilityTwoDamage;
        this.GetComponent<GrenadierAbilities>().AbilityTwoDamageRange = AbilityTwoDamageRange;
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public float GrenadierDamageCall()
    {
        return GrenadierDamage;
    }
}
