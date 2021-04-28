using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MinionTowerAttacking_old : MonoBehaviour
{

    private NavMeshAgent navAgent;
    public Collider[] TargetsInRange;
    private Pathfinding_old pathfinding;
    public float detectionRadius;
    public LayerMask mask;
    public int MinionDamage;
    private bool cooldown;
    private Collider NearestTarget;
    private GameObject ultiTarget;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        cooldown = true;
        pathfinding = GetComponent<Pathfinding_old>();
        ultiTarget = GameObject.FindGameObjectWithTag("MinionGoal");
    }

    void Update()
    {
        if (navAgent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            TargetsInRange = Physics.OverlapSphere(transform.position, detectionRadius, mask);
            float shortestDitance = Mathf.Infinity;
            NearestTarget = null;

            foreach (Collider Target in TargetsInRange)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, Target.transform.position);
                if (distanceToEnemy < shortestDitance && Target.GetComponent<HealthScript>().CurrentHealth > 0)
                {
                    shortestDitance = distanceToEnemy;
                    NearestTarget = Target;
                }
                Debug.Log(NearestTarget);
            }
           if (NearestTarget != null && cooldown == true)    
           {
                cooldown = false;
                Invoke("damagewall", 1.0f);
           }
        }

        if(pathfinding.currentNode.name == "THE ORB")
        {
            if (Vector3.Distance(transform.position, ultiTarget.transform.position) <= 2.0f)
            {
                cooldown = false;
                Invoke("damageorb", 1.0f);
            }
        }
    }

    void damagewall()
    {
        cooldown = true;
        if (NearestTarget != null && NearestTarget.GetComponent<HealthScript>().CurrentHealth > 0)
        {
            NearestTarget.GetComponent<HealthScript>().CurrentHealth -= MinionDamage;
        }
    }

    void damageorb()
    {
        cooldown = true;
        ultiTarget.GetComponent<HealthScript>().CurrentHealth -= MinionDamage;
    }
}
