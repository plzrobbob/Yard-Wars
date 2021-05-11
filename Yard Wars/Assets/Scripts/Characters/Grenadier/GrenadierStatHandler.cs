using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierStatHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public float GrenadierHealth = 120f;
    public float GrenadierDamage = 10f;
    public float GrenadierSpeed = 10f;
    public float AbilityOneDamage = 50f;
    public float AbilityOneRange = 5f;
    public float AbilityTwoDamage = 40f;
    public float AbilityTwoRange = 2.5f;
    public float AbilityTwoslowTime = 2f;
    public float UltiDamage = 10f;


    void Start()
    {
        this.GetComponent<HealthScript>().MaxHealth = GrenadierHealth;
        this.GetComponent<HealthScript>().CurrentHealth = GrenadierHealth;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = GrenadierSpeed;
        this.GetComponent<GrenadierAbilities>().AbilityOneDamage = AbilityOneDamage;
        this.GetComponent<GrenadierAbilities>().AbilityOneRange = AbilityOneRange;
        this.GetComponent<GrenadierAbilities>().AbilityTwoDamage = AbilityTwoDamage;
        this.GetComponent<GrenadierAbilities>().AbilityTwoDamageRange = AbilityTwoRange;
        this.GetComponent<GrenadierBasicAttack>().damage = GrenadierDamage;
        this.GetComponent<GrenadierAbilities>().UltiDamageNum = UltiDamage;
        this.GetComponent<GrenadierAbilities>().AbilityTwoSlowSpeed = AbilityTwoslowTime;

        //Currently they only way I know how to get the layermask to work is this way. Effectively what you are doing is comparing the players own layer, and if it is 
        //team 1, set it's target to Team2 
        //Please for the love of Zeus help me find a better way -Cameron
        //This portion is designed to work for grenadier basic attacks. In the future I may reapproach this and get it
        //set up for other abilities like ability 2
        if (gameObject.layer == 20)
        {
            this.GetComponent<GrenadierBasicAttack>().TeamLayer = 20;
            this.GetComponent<GrenadierAbilities>().Target = LayerMask.GetMask("Team2");
        }
        else
        {
            this.GetComponent<GrenadierBasicAttack>().TeamLayer = 21;
            this.GetComponent<GrenadierAbilities>().Target = LayerMask.GetMask("Team1");

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public float GrenadierDamageCall()
    {
        return GrenadierDamage;
    }
    public float GrenadierHealthCall()
    {
        return GrenadierHealth;
    }
}
