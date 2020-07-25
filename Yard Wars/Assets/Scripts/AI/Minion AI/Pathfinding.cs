using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent navAgent;
    public Vector3[] cornerPoints;
    public Collider[] cols;
    public int PF_Index = 0;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("EnemyTarget");
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.SetDestination(target.transform.position);
        navAgent.radius = Random.Range(0.75f, 1.75f);
    }

    // Update is called once per frame
    void Update()
    {
        cornerPoints = navAgent.path.corners;
    }
}
