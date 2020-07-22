using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthScript : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth;

    public float regentimer;
    public float regenmult;
    private float curregentime;

    public bool damaged;


    public void Update()
    {
        RegenHandler();
        if (CurrentHealth <= 0)
        {
            Dead();
        }
    }

    public void RegenHandler()
    {
        if (damaged && curregentime < regentimer)
        {
            curregentime += Time.deltaTime;
        }

        else if (curregentime >= regentimer)
        {
            damaged = false;
        }

        if (!damaged && curregentime >= regentimer && CurrentHealth < MaxHealth)
        {
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth += regenmult * Time.deltaTime;
            }

            if (CurrentHealth >= MaxHealth)
            {
                CurrentHealth = MaxHealth;
                curregentime = 0;
            }
        }
    }
    public void DamageHandler()
    {
        damaged = true;
        curregentime = 0;
    }

    public void Dead()
    {
        //die time
        Destroy(gameObject);
    }
}
