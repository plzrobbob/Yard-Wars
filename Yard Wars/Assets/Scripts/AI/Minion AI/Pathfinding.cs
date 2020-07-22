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
        cornerPoints = navAgent.path.corners;
        //navAgent.SetDestination(cornerPoints[0]);
        //cornerPoints = VaryCorners(cornerPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3[] VaryCorners(Vector3[] corners)
    {
        for (int i = 0; i < corners.Length; ++i)
        {
            cols = Physics.OverlapSphere(corners[i], 0.5f, 10);

            Vector3 newPos = Vector3.Lerp(corners[i], cols[0].transform.position, Random.Range(0f, 1f));
            corners[i] = newPos;
            
        }
        return corners;
    }
}
