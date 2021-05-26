using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderUlt : MonoBehaviour
{
    public float cooldown;
    public bool on;
    public float ultiTime;
    public GameObject builder;
    public int layer;
    private LayerMask mask;

    void Start()
    {
        if (layer == 20){
            mask = LayerMask.GetMask("Team1");
        }
        else if (layer == 21)
        {
            mask = LayerMask.GetMask("Team2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        if (on)
        {
            ulti();
        }

        if (Input.GetKey(KeyCode.N) && cooldown > 30 && !on)
        {
            on = true;
            Debug.Log("Ulti Starts");
        }
    }

    public void ulti()
    {
        int layerMask = 0;
        ///runs the ulti. sets the radius to 10 and the player is in the center
        ultiTime += Time.deltaTime;
        if (mask == 20)
        {
            layerMask = mask << 20;
        }

        else if (mask == 21)
        {
            layerMask = mask << 21;
        }
        Vector3 center = GameObject.FindWithTag("PlayerHolder").transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, 10.0f, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            //if the hitcollider has the same tag as the builder (Team1 or Team2), and contains the stathandler, and ult time is less than 10 seconds
            //stats change
            if (GameObject.FindWithTag("PlayerHolder").tag == hitCollider.tag && hitCollider.GetComponent<GrenadierStatHandler>() && ultiTime < 10.0f)
            {
                hitCollider.GetComponent<GrenadierBasicAttack>().damage = 20.0f;
                hitCollider.GetComponent<GrenadierBasicAttack>().ExitSpeed = 100.0f;
                hitCollider.GetComponent<HealthScript>().CurrentHealth = 200.0f;
            }

            if(GameObject.FindWithTag("PlayerHolder").tag == hitCollider.tag && hitCollider.GetComponent<BuilderStatBlock>() && ultiTime < 10.0f)
            {
                hitCollider.GetComponent<BuilderBasicAttack>().damage = 20.0f;
                hitCollider.GetComponent<PlayerCharacterController>().MoveSpeed = 20.0f;
                hitCollider.GetComponent<HealthScript>().CurrentHealth = 200.0f;
            }

            if (GameObject.FindWithTag("PlayerHolder").tag == hitCollider.tag && hitCollider.GetComponent<GuardStatHandler>() && ultiTime < 10.0f)
            {
                hitCollider.GetComponent<MeleeWeaponFire>().damageAmount = 50.0f;
                hitCollider.GetComponent<PlayerCharacterController>().MoveSpeed = 20.0f;
                hitCollider.GetComponent<HealthScript>().CurrentHealth = 200.0f;
            }

            if (GameObject.FindWithTag("PlayerHolder").tag == hitCollider.tag && hitCollider.GetComponent<ThiefStatHandler>() && ultiTime < 10.0f)
            {
                hitCollider.GetComponent<MeleeWeaponDamage>().damageAmount = 20.0f;
                hitCollider.GetComponent<PlayerCharacterController>().MoveSpeed = 20.0f;
                hitCollider.GetComponent<HealthScript>().CurrentHealth = 200.0f;
            }

            ///repeat process for the guard, thief, and builder classes
            ///adjust based on classes (by percentage rather than hard values)
            ///when ability is over, make sure to include the stat handler as a condition or will not work
            ///make return functions for all stat handlers to return original values

            if (ultiTime > 10f)
            {
                on = false;
                cooldown = 0f;
                ultiTime = 0f;
                if (hitCollider.GetComponent<GrenadierStatHandler>() && ultiTime > 10f)
                {
                    hitCollider.GetComponent<GrenadierBasicAttack>().damage = hitCollider.GetComponent<GrenadierStatHandler>().GrenadierDamageCall();
                    hitCollider.GetComponent<HealthScript>().CurrentHealth = hitCollider.GetComponent<GrenadierStatHandler>().GrenadierMaxHealth();
                    hitCollider.GetComponent<GrenadierBasicAttack>().ExitSpeed = hitCollider.GetComponent<GrenadierStatHandler>().GrenadierSpeedFunction();
                }

                if (hitCollider.GetComponent<BuilderStatBlock>() && ultiTime > 10f)
                {
                    hitCollider.GetComponent<BuilderStatBlock>().BuilderDamage = hitCollider.GetComponent<BuilderStatBlock>().BuilderDamageFunction();
                    hitCollider.GetComponent<HealthScript>().CurrentHealth = hitCollider.GetComponent<BuilderStatBlock>().BuilderHealthFunction();
                    hitCollider.GetComponent<PlayerCharacterController>().MoveSpeed = hitCollider.GetComponent<BuilderStatBlock>().BuilderSpeedFunction();
                }

                if (hitCollider.GetComponent<BuilderStatBlock>() && ultiTime > 10f)
                {
                    hitCollider.GetComponent<BuilderStatBlock>().BuilderDamage = hitCollider.GetComponent<GuardStatHandler>().GuardDamageFunction();
                    hitCollider.GetComponent<HealthScript>().CurrentHealth = hitCollider.GetComponent<GuardStatHandler>().GuardHealthFunction();
                    hitCollider.GetComponent<PlayerCharacterController>().MoveSpeed = hitCollider.GetComponent<GuardStatHandler>().GuardSpeedFunction();
                }

                if (hitCollider.GetComponent<BuilderStatBlock>() && ultiTime > 10f)
                {
                    hitCollider.GetComponent<BuilderStatBlock>().BuilderDamage = hitCollider.GetComponent<ThiefStatHandler>().ThiefDamageFunction();
                    hitCollider.GetComponent<HealthScript>().CurrentHealth = hitCollider.GetComponent<ThiefStatHandler>().ThiefHealthFunction();
                    hitCollider.GetComponent<PlayerCharacterController>().MoveSpeed = hitCollider.GetComponent<ThiefStatHandler>().ThiefSpeedFunction();
                }
            }
        }
    }
}
