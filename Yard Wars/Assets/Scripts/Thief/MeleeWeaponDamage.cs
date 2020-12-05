using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamage : MonoBehaviour
{
    public bool attackSuccessful;
    public float damageAmount;


    // Start is called before the first frame update
    void Start()
    {
        attackSuccessful = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BoxCollider>().enabled == false)
        {
            attackSuccessful = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            //add code here to cause damage to whatever enters the trigger hitbox
            HealthScript M_HealthScript = collider.gameObject.GetComponent<HealthScript>();
            M_HealthScript.DamageHandler();
            M_HealthScript.CurrentHealth -= damageAmount;

            attackSuccessful = true;
            Debug.Log("Sir, an iceburg struck the Trigger collider, I believe the ship is going down.");
        } 


    }
}
