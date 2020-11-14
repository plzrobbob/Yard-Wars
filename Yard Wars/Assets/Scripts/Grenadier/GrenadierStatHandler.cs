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
    private float AbilityOneRange = 10f;

    void Start()
    {
        this.GetComponent<HealthScript>().MaxHealth = GrenadierHealth;
        this.GetComponent<HealthScript>().CurrentHealth = GrenadierHealth;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = GrenadierSpeed;
        this.GetComponent<GrenadierAbilityOne>().AbilityDamage = AbilityOneDamage;
        this.GetComponent<GrenadierAbilityOne>().AbilityRange = AbilityOneRange;

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
