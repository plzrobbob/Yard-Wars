using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamage : MonoBehaviour
{
    public bool attackSuccessful;
    public float damageAmount;

    //public float range;

    //public LayerMask EnemyMask;
    //public Collider[] enemiesHit;

    private Collider[] HitTargets;
    private Vector3 overlapCheck;
    public float area;
    public int layernum;
    private LayerMask layer;


    // Start is called before the first frame update
    void Start()
    {
        attackSuccessful = false;

        if (layernum == 20)
        {
            layer = LayerMask.GetMask("Team2");
        }
        else if (layernum == 21)
        {
            layer = LayerMask.GetMask("Team1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BoxCollider>().enabled == false)
        {
            attackSuccessful = false;
        }

        OnDrawGizmos();
    }

    private void OnTriggerEnter(Collider collider)
    {
        


        //if (collider.gameObject.tag == "Enemy")
        //{

            HitTargets = Physics.OverlapSphere(transform.position, area, layer);
            DoDamage();

            //add code here to cause damage to whatever enters the trigger hitbox
            //HealthScript M_HealthScript = collider.gameObject.GetComponent<HealthScript>();
            //M_HealthScript.DamageHandler();
            //M_HealthScript.CurrentHealth -= damageAmount;
            attackSuccessful = true;
            Debug.Log("Sir, an iceburg struck the Trigger collider, I believe the ship is going down.");
        //} 


    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();
            M_HealthScript.CurrentHealth -= damageAmount;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, area);
    }
}
