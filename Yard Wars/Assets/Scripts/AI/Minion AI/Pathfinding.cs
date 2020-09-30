using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    private Vector3 target;
    private NavMeshAgent navAgent;
    public GameObject firstNode;
    public GameObject currentNode;
    public IntersectionPathingRandomization prcpy;
    public float distanceForDebug;
    public float distanceToNode;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentNode = firstNode;
        prcpy = currentNode.GetComponent<IntersectionPathingRandomization>();
        target = prcpy.AddPathVariance();
        navAgent.SetDestination(target);
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
}
