﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderStatBlock : MonoBehaviour
{

    public float BuilderHealth = 120f;
    public float BuilderDamage = 10f;
    public float BuilderSpeed = 10f;
    public float AbilityOneDamage = 50f;
    public float AbilityOneRange = 5f;
    public float AbilityTwoDamage = 40f;
    public float AbilityTwoRange = 2.5f;
    public float AbilityTwoslowTime = 2f;
    public float UltiDamage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<HealthScript>().MaxHealth = BuilderHealth;
        this.GetComponent<HealthScript>().CurrentHealth = BuilderHealth;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = BuilderSpeed;
        this.GetComponent<BuilderBasicAttack>().damage = BuilderDamage;




        if (gameObject.layer == 20)
        {
            this.GetComponent<BuilderBasicAttack>().Target = 21;
            this.GetComponent<BuilderAbilities>().Target = 21;
        }
        else
        {
            this.GetComponent<BuilderBasicAttack>().Target = 20;
            this.GetComponent<BuilderAbilities>().Target = 20;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float BuilderDamageFunction()
    {
        return BuilderDamage;
    }

    public float BuilderHealthFunction()
    {
        return BuilderHealth;
    }

    public float BuilderSpeedFunction()
    {
        return BuilderSpeed;
    }
}
