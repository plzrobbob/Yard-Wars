using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding_old : MonoBehaviour
{
    public Vector3 target;
    private NavMeshAgent navAgent;
    public GameObject firstNode;
    public GameObject currentNode;
    public IntersectionPathingRandomization_old prcpy;
    public float distanceForDebug;
    public float distanceToNode;
    public MinionPlayerAttacking_old MinionPlayerAttacking;

    public bool isSliding;
    public Vector3 SlidingDirection;
    public LayerMask defeatedsigh;



    //This is used for Slowing enemies for the AI
    public float BaseSpeed;
    public float EditedSpeed;



    //This is used for stunning the enemy
    public Pathfinding_old pathfinding;
    public GameObject StunVFX;
    public MinionTowerAttacking_old MTA;
    public MinionPlayerAttacking_old MPA;

    //public float speedHolder;
    public GameObject Builder2TurnOff;


    // Start is called before the first frame update
    void Start()
    {
        isSliding = false;
        navAgent = GetComponent<NavMeshAgent>();
        currentNode = firstNode;
        prcpy = currentNode.GetComponent<IntersectionPathingRandomization_old>();
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
            prcpy = currentNode.GetComponent<IntersectionPathingRandomization_old>();
            target = prcpy.AddPathVariance();
            navAgent.SetDestination(target);

            MinionPlayerAttacking.previousnode = target;
        }
        if (isSliding)
        {
            Sliding();
        }
       // Debug.Log(navAgent.velocity + "Art thou slowing down?");
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

    void Sliding()
    {
        navAgent.velocity = SlidingDirection;
        //navAgent.Move(SlidingDirection *navAgent.speed * Time.deltaTime);
        // CharController.Move(Velocity * Time.deltaTime);
        // CharController.Move(tempSlidingDirection * MoveSpeed * Time.deltaTime);
        if (!Builder2TurnOff)
        {
            isSliding = false;
            Debug.Log("Hey so like Builder Ability 2 Marbles is totally gone. It should be gone.");
            NavMeshAgent m_navmeshagent = gameObject.GetComponent<NavMeshAgent>();

            m_navmeshagent.isStopped = false;
        }
        Ray ray;
        RaycastHit hit;
        ray = new Ray(gameObject.transform.position, SlidingDirection);
        
        if (Physics.Raycast(gameObject.transform.position, SlidingDirection, out hit, 1, defeatedsigh))//cast the ray 1 unit at the specified direction
        {
            //This lets them bounce off whatever they hit, as it should be
            {
                Debug.Log(hit.collider.gameObject.name);
                print("Raycast hits wall");
                //find new ray direction
               Vector3 inDirection = Vector3.Reflect(ray.direction, hit.normal);
               Debug.DrawRay(hit.point, inDirection * 8, Color.magenta);
                Debug.Log(inDirection + "In Direction and then temp sliding direction" + SlidingDirection);

                SlidingDirection = inDirection.normalized * SlidingDirection.magnitude;
            }
        }
    }


}
