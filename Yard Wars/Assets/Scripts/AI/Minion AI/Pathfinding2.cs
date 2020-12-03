using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding2 : MonoBehaviour
{
    private NavMeshAgent navAgent;
    public GameObject target;
    public float varianceCircleRadius;
    [SerializeField]
    private NavMeshPath path_copy;
    public Vector3 steeringTarget_copy;
    public Vector3 steeringTarget_actual;
    public Vector3[] actualPath;
    public Vector3[] comparedPath;
    public Vector3[] new_path;
    public int frame_delay;

    // Start is called before the first frame update
    void Start()
    {
        frame_delay = 4;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.SetDestination(target.transform.position);
        path_copy = navAgent.path;
        actualPath = navAgent.path.corners;
        comparedPath = path_copy.corners;
        steeringTarget_copy = navAgent.steeringTarget;
        steeringTarget_actual = navAgent.steeringTarget;
    }

    // Update is called once per frame
    void Update()
    {
        steeringTarget_actual = navAgent.steeringTarget;
        actualPath = navAgent.path.corners;
        comparedPath = path_copy.corners;
        if (frame_delay > 0)
        {
            frame_delay--;
        }
        /*
         * ComparePaths function is used instead of checking the path status. 
         * When the NavMesh updates in runtime, like when the player places
         * a wall set to carve the NavMesh, ALL agents on that NavMesh auto-
         * repath even if you've told them not to. So, ComparePaths can let 
         * us know if the agents have had to repath due to an obstacle.
         * ComparePaths() = true when the agent has recalculated their path,
         * false otherwise.
         * CompareSteeringTargets is used to know when we've arrived at a
         * waypoint and are ready to path to the next waypoint in the path,
         * which is when we vary the next waypoint's position a little.
        */
        if ((ComparePaths() || CompareSteeringTargets()) && frame_delay == 0) 
        {
            Debug.Log("ComparePaths = " + ComparePaths() + ", CompareSteeringTarget = " + CompareSteeringTargets());
            AddVariance();
            steeringTarget_copy = navAgent.steeringTarget;
        }
    }
    bool ComparePaths() 
    {
        if (path_copy.corners.Length <= 1)
        {
            path_copy = navAgent.path;
            //frame_delay = 4;
            return false;
        }
        if (path_copy.corners[1] != navAgent.path.corners[1] || (navAgent.path.corners.Length == 1 && path_copy.corners.Length == 1))
        {
            path_copy = navAgent.path;
            frame_delay = 4;
            return true;
        }
        else return false;
    }
    bool CompareSteeringTargets()
    {
        if (Vector3.Distance(steeringTarget_copy, navAgent.steeringTarget) > 0.01f)
        {
            return true;
        }
        else return false;
    }
    void AddVariance()
    {
        int i;
        for (i = 0; ; i++) // finds how far along we are on the path
            if (navAgent.steeringTarget == navAgent.path.corners[i])
                break;

        Vector3 v1 = navAgent.steeringTarget - transform.position;
        Vector3 v2 = navAgent.steeringTarget - navAgent.path.corners[i];
        Vector3 v3;
        Vector3 circleCenter;
        Vector3 nextWaypoint;

        float angle = Vector3.SignedAngle(v1, v2, Vector3.up);
        v1 = transform.position - navAgent.steeringTarget; // reverse the direction of the v1 for cross product

        if (angle < 0) // vary point to the right
        {
            v3 = Vector3.Cross(v1, Vector3.up); // v3 is a vector3 starting at steeringTarget and pointing right
        }
        else // vary point to the left
        {
            v3 = Vector3.Cross(v1, Vector3.down); // v3 is a vector3 starting at steeringTarget and pointing left
        }
        circleCenter = navAgent.steeringTarget + (v3.normalized * varianceCircleRadius);
        nextWaypoint = new Vector3(circleCenter.x + Random.Range(-varianceCircleRadius, varianceCircleRadius),
                                       circleCenter.y + Random.Range(-varianceCircleRadius, varianceCircleRadius),
                                       circleCenter.z + Random.Range(-varianceCircleRadius, varianceCircleRadius));
        NavMeshPath newPath = navAgent.path;
        NavMesh.CalculatePath(nextWaypoint, target.transform.position, navAgent.areaMask, newPath);
        new_path = newPath.corners;
        bool test = navAgent.SetPath(newPath);
        path_copy = newPath;
        Debug.Log("Set Path Success: " + test);
        frame_delay = 4;
    }
}
