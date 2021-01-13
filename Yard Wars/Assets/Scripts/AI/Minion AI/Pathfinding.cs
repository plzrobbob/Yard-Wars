﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    public Vector3 target;
    private NavMeshAgent navAgent;
    public GameObject firstNode;
    public GameObject currentNode;
    public IntersectionPathingRandomization prcpy;
    public float distanceForDebug;
    public float distanceToNode;
    public MinionPlayerAttacking MinionPlayerAttacking;




    //This is used for Slowing enemies for the AI
    public float BaseSpeed;
    public float EditedSpeed;



    //This is used for stunning the enemy
    public Pathfinding pathfinding;
    public GameObject StunVFX;
    public MinionTowerAttacking MTA;
    public MinionPlayerAttacking MPA;

    //public float speedHolder;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentNode = firstNode;
        prcpy = currentNode.GetComponent<IntersectionPathingRandomization>();
        target = prcpy.AddPathVariance();
        navAgent.SetDestination(target);
        BaseSpeed = navAgent.speed;

    }

    // Update is called once per frame
    void Update()
    {
        distanceForDebug = Vector3.Distance(gameObject.transform.position, target);
        if (Vector3.Distance(gameObject.transform.position, target) <= distanceToNode)
        {
            currentNode = prcpy.nextNode;
            prcpy = currentNode.GetComponent<IntersectionPathingRandomization>();
            target = prcpy.AddPathVariance();
            navAgent.SetDestination(target);

            MinionPlayerAttacking.previousnode = target;
        }
    }

    void PathToNextNode()
    {
        Vector3 nextcorner = navAgent.path.corners[0];
        Collider[] tempNodeCollider = new Collider[5];
        if (Physics.OverlapSphereNonAlloc(nextcorner, 10f, tempNodeCollider) > 0)
        {

        }
    }








    //I AM CLAIMING THIS SECTION OF THE SCRIPT. I UNDERSTAND THIS IS STEPHENS BUT I HAVE ALTERED THE DEAL. PRAY I DO NOT ALTER IT FURTHER.
    //    
    //    Signed,
    //          The muffled yelling from someone sounding similar to cameron from the safety of his Compile Bear Bunker.
    //  


    public void Slowed(float percentage, float time)
    {
        EditedSpeed = BaseSpeed * percentage;
        Debug.Log("Base speed" + BaseSpeed + "percentage" + percentage + "EditedSpeed" + EditedSpeed);

        navAgent.speed = EditedSpeed;
        Debug.Log(navAgent.speed);

        Invoke("resetSpeed", time);
    }

    public void Faster(float percentage, float time)
    {
        EditedSpeed = BaseSpeed * percentage;
        navAgent.speed = EditedSpeed;

        Invoke("resetSpeed", time);
    }


    void resetSpeed()
    {
        navAgent.speed = BaseSpeed;
        Debug.Log("SpeedReset");
        Debug.Log(navAgent.speed);

    }

    public void Stunned(float time)
    {
        Debug.Log(gameObject.name + " is currently stunned for " + time + " seconds!");
        navAgent.speed = 0f;
        StunVFX.SetActive(true);
        DisableAll();
        Invoke("Unstunned", time);
    }

    void Unstunned()
    {
        StunVFX.SetActive(false);
        resetSpeed();
        EnableAll();
    }

    void DisableAll()
    {
        MTA.enabled = false;
        MPA.enabled = false;
    }

    void EnableAll()
    {
        MTA.enabled = true;
        MPA.enabled = true;
    }

}
