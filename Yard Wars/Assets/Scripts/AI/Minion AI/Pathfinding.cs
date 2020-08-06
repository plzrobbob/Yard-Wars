using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent navAgent;
    public int PF_Index = 0;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("EnemyTarget");
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.SetDestination(target.transform.position);
        navAgent.acceleration = Random.Range(2.5f, 4f);
        navAgent.angularSpeed = Random.Range(0.5f, 1.5f);
        navAgent.stoppingDistance = Random.Range(0.5f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
