using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderUlt : MonoBehaviour
{
    public float cooldown;
    public bool on;
    public float ultiTime;

    void Start()
    {

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
        ///runs the ulti. sets the radius to 10 and the player is in the center
        ultiTime += Time.deltaTime;
        Vector3 center = GameObject.FindWithTag("PlayerHolder").transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, 10.0f);
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

            ///repeat process for the guard, thief, and builder classes
            ///adjust based on classes (by percentage rather than hard values)
            ///when ability is over, make sure to include the stat handler as a condition or will not work
            ///make return functions for all stat handlers to return original values

            if (hitCollider.GetComponent<GrenadierStatHandler>() && ultiTime > 10f)
            {
                on = false;
                cooldown = 0f;
                ultiTime = 0f;
                hitCollider.GetComponent<GrenadierBasicAttack>().damage = hitCollider.GetComponent<GrenadierStatHandler>().GrenadierDamageCall();
                hitCollider.GetComponent<HealthScript>().CurrentHealth = hitCollider.GetComponent<GrenadierStatHandler>().GrenadierHealthCall();
                hitCollider.GetComponent<GrenadierBasicAttack>().ExitSpeed = 30f;
                
            }
        }
    }
}
