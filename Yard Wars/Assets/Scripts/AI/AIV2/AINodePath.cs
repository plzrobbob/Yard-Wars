using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



//need to change algorithm that tells if the minion is closer to node 1 vs node 2 vs node 3 ...
//          Potential fix: use a ditance check to see the distance between two ndoes.  then check this distance with referecne to the minion.  This can give the exact spot the minion is currently at on the lane

//need to change algorithm that will reset path until a valid path is found.  
//          potential fix: It should slowly increase the random range circleradius until it is possible to navigate to the point

//need to fix the agrotarget function.  The targets are not picked in the correct order.  If the path is blocked then minions will prioritze this target.  This could be difficult to fix.  Will have to brain storm.




public class AINodePath : MonoBehaviour
{
    private NavMeshAgent Minionagent;
    private NavMeshObstacle MinionObstacle;

    [Header("Minion Info")]
    public int CurNode;//the current node the minion is pathing to
    public float circleRadius = 1.0f;//float to plot a random point around the node to path to
    public bool PartialPath;//is the minion path blocked?
    public bool onpath;//is the minion on a path?
    private bool nodeIsEmpty;//this bool will reset the struct if(path is not empty, and it is not currently pathfinding)
    private bool recalcpath;//the minion is ready to reclaculate the path
    public GameObject LastKnownLanePos;
    public float AttackRate = 1f;
    public float StoppingDistance = 1f;

    [Header("FSM bools")]
    public bool nodepathing;//the minion is performing pathfinding
    public bool GoalReached;//the minion is has reached the goal and is attacking
    public bool AgroTarget;//the minion is moving to the target and attacking

    [Serializable]
    public struct PathArr//the struct that tells us if the node has been reached
    {
        public GameObject Node;
        public bool ReachedNode;
        public Vector3 position;
    }

    [Header("PathArrayss")]
    public GameObject[] path;//the path the minion should be navigating around

    [SerializeField]
    private List<PathArr> data2;//the path that is set by the waveManager Script on instantiation

    //these are used for agro target and the agro brain
    [Header("AgroTargets")]
    public GetAgroTarget getAgroTarget;//used to get the agro target
    public GameObject Target;//the current target of the AI - this is given by getAgroTarget
    private float targetdestcount;
    private float targetdesttimer = .5f;
    private Vector3 TargetPreviousPos;
    private Vector3 TargetRandPos;
    private Vector3 firstpos;
    private bool CheckStuck = false;
    [SerializeField]
    private GameObject WallTarget = null;
    [SerializeField]
    private GameObject TurretTarget = null;
    [SerializeField]
    private bool PathToTurretTargetIsPartial;
    [SerializeField]
    private GameObject PlayerTarget = null;
    [SerializeField]
    private bool PathToPlayerTargetIsPartial;    
    [SerializeField]
    private bool PathToNextNodeIsPartial;

    [SerializeField]
    public GameObject TheOrb = null;
    public float MinionDamage = 5f;

    //used for sliding
    [Header("Sliding")]
    public bool isSliding;
    public Vector3 SlidingDirection;
    public LayerMask defeatedsigh;

    //This is used for Slowing enemies for the AI
    [Header("Slowing")]
    public float BaseSpeed;
    public float EditedSpeed;

    //This is used for stunning the enemy
    [Header("Stunning")]
    public GameObject StunVFX;
    public bool IsStunned;

    //public float speedHolder;
    public GameObject Builder2TurnOff;

    void Start()
    {
        Minionagent = GetComponent<NavMeshAgent>();
        MinionObstacle = GetComponent<NavMeshObstacle>();
        CurNode = 0;
        nodeIsEmpty = true;
        onpath = true;
        PartialPath = false;
        InvokeRepeating("GetTarget", 0f, .5f);
        InvokeRepeating("AttackTarget", 0f, AttackRate);
    }

    //if player or turret gets too close - attack player until he is a specific distance away from initial agro position
    //else if path blocked by wall - attack wall with other minions that have a distance to the next node that is greater than the minion who annouced the charge  (this must be modular so if a new wall is placed it will change agro)
    //else if minion is in range of core then attack the core
    //if none of the above are true then continue with normal pathfinding
    //      if dist to node 1 >= dist to node 2, make sure node 2 is the curent target node.

    private void Update()
    {
        //Debug.Log(PartialPath);
        //Debug.Log(Minionagent.pathStatus);
        //Debug.Log(Minionagent.isPathStale);
        //Debug.Log("turret " + PathToTurretTargetIsPartial);


        var path2 = Minionagent.path;
        for (int i = 0; i < path2.corners.Length - 1; i++)
            Debug.DrawLine(path2.corners[i], path2.corners[i + 1], Color.red);

        if (!Minionagent.hasPath && Minionagent.enabled && !nodeIsEmpty && nodepathing)//this will recalculate a path if the path is blocked by an obstacle
        {
            if (Target != null)
            {
                TargetRandPos = new Vector3(Target.transform.position.x + UnityEngine.Random.Range(-circleRadius, circleRadius), Target.transform.position.y, Target.transform.position.z + UnityEngine.Random.Range(-circleRadius, circleRadius));
            }
            else
            {
                var temp = new PathArr();
                temp.Node = path[CurNode];
                temp.ReachedNode = false;
                temp.position = new Vector3(path[CurNode].transform.position.x + UnityEngine.Random.Range(-circleRadius, circleRadius), path[CurNode].transform.position.y, path[CurNode].transform.position.z + UnityEngine.Random.Range(-circleRadius, circleRadius));
                data2[CurNode] = temp;
            }
        }

        if (!nodepathing && nodeIsEmpty && path.Length > 0)//on the first frame get the path and make a set of nodes for minions to follow.
        {
            try
            {
                if (data2.Count <= 0)//only run this once.  the else statement should never run
                {
                    for (int i = 0; i < path.Length; i++)
                    {
                        var temp = new PathArr();
                        temp.Node = path[i];
                        temp.ReachedNode = false;
                        temp.position = new Vector3(path[i].transform.position.x + UnityEngine.Random.Range(-circleRadius, circleRadius), path[i].transform.position.y, path[i].transform.position.z + UnityEngine.Random.Range(-circleRadius, circleRadius));
                        data2.Add(temp);
                    }
                    nodepathing = true;
                    nodeIsEmpty = false;
                    GetNextDestination();
                }
                else
                {
                    Debug.LogError("This should only be running once.  If you see this error, then more nodes are being added to the minion pathing.  THIS IS NO BUENO");
                }
            }
            catch
            {
                Debug.LogError("error in minion spawn");
            }
        }

        if (data2[data2.Count - 1].ReachedNode)//the last node has been reached and the minion should no longer be pathing or agroing anything
        {
            GoalReached = true;
        }

        if (Minionagent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            PartialPath = true;
        }
        else
        {
            PartialPath = false;
        }

        if (isSliding)
        {
            Sliding();
        }

        //call the state machine to determine the AI's next state if the minion is not stunned
        if (!IsStunned)
        {
            FSM();
        }
    }

    void GetTarget()
    {
        if (!GoalReached)//maybe put this on a timer
        {
            //call getagrotarget() here.
            //getagrotarget() will find any gameobject in range that the minion can attack.  
            //This acts as an additional brain that will decide if something has met the criteria to be attacked or not.
            //if getagroTarget does not return with a valid target then Agrotarget == false
            (WallTarget, TurretTarget, PlayerTarget) = getAgroTarget.GetTargetsInRange();

            if (PlayerTarget != null)//check player target
            {
                if (!PathToPlayerTargetIsPartial)//if player is on path then minion should follow them if there is no wall in the way.
                {
                    Target = PlayerTarget;
                }
            }
            else if (TurretTarget != null && !PathToTurretTargetIsPartial)
            {
                Target = TurretTarget;
            }
            else if (WallTarget != null && !CheckStuck && PartialPath && (PathToNextNodeIsPartial || PathToPlayerTargetIsPartial || PathToTurretTargetIsPartial))
            {
                CheckStuck = true;
                firstpos = transform.position;
                Invoke("CheckIfStuck", .5f);
            }

            if (Target != null)
            {
                AgroTarget = true;
                if (Target.transform.position != TargetPreviousPos)
                {
                    TargetPreviousPos = Target.transform.position;
                    TargetRandPos = new Vector3(Target.transform.position.x + UnityEngine.Random.Range(-circleRadius, circleRadius), Target.transform.position.y, Target.transform.position.z + UnityEngine.Random.Range(-circleRadius, circleRadius));
                    AgroTarget = true;
                }
            }

            if (PathToNextNodeIsPartial || PathToPlayerTargetIsPartial || PathToTurretTargetIsPartial)
            {
                int mask = 0;
                mask += 0 << NavMesh.GetAreaFromName("Walkable");
                mask += 0 << NavMesh.GetAreaFromName("Not walkable");
                mask += 0 << NavMesh.GetAreaFromName("Jump");
                mask += 0 << NavMesh.GetAreaFromName("offPath");
                mask += 1 << NavMesh.GetAreaFromName("Lane");

                var pathOnlyLane = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, data2[CurNode].position, mask, pathOnlyLane);//calculate the path using an area mask.  This makes it so minions wont leave the lane surface
                if (pathOnlyLane.status == NavMeshPathStatus.PathComplete)
                {
                    PathToNextNodeIsPartial = false;
                }

                if (PlayerTarget != null )
                {
                    bool Playeronpath = PlayerTarget.GetComponent<TargetProperty>().onpath;
                    if (Playeronpath)
                    {
                        NavMesh.CalculatePath(transform.position, PlayerTarget.transform.position, mask, pathOnlyLane);//calculate the path using an area mask.  This makes it so minions wont leave the lane surface
                        if (pathOnlyLane.status == NavMeshPathStatus.PathComplete)
                        {
                            PathToPlayerTargetIsPartial = false;

                        }
                    }
                }
                if (TurretTarget != null)
                {
                    NavMesh.CalculatePath(transform.position, TurretTarget.transform.position, mask, pathOnlyLane);//calculate the path using an area mask.  This makes it so minions wont leave the lane surface
                    if (pathOnlyLane.status == NavMeshPathStatus.PathComplete)
                    {
                        PathToTurretTargetIsPartial = false;
                    }
                }
            }

            if (Target == PlayerTarget && PlayerTarget != null && LastKnownLanePos != null) //if minion has followed the palyer too far off the lane then reset palyer target to null
            {
                float tempdist = Vector3.Distance(LastKnownLanePos.transform.position, transform.position);
                if (tempdist > 15f)
                {
                    Target = null;
                    PlayerTarget = null;
                    getAgroTarget.PlayerTarget = null;
                }
            }
        }
    }
    void CheckIfStuck()//if minion hasnt moved more than .3f in the ;ast second then attack the nearest wall
    {
        float distcheck = Vector3.Distance(firstpos, transform.position);
        if (distcheck < .3f && WallTarget != null)
        {
            Target = WallTarget;
        }
        CheckStuck = false;
    }

    void FSM()
    {
        //this is basically a state machine.  It controls what the minion will be doing.  Stephen And Ben will set up this aprt towards the end of AI script creation
        if (GoalReached)//if minion is nearing the goal then agro the goal
        {
            if (Vector3.Distance(data2[data2.Count - 1].Node.transform.position, transform.position) > StoppingDistance)//if minion is pushed away from the goal then pathfind back to the goal
            {
                GoalReached = false;//goal is no longer reached

                var temp = new PathArr();
                temp.Node = data2[data2.Count - 1].Node;
                temp.ReachedNode = false;
                data2[data2.Count - 1] = temp;//the node is no longer reached


                nodepathing = true;//reactivate pathfinding
                MinionObstacle.enabled = false;//diable the obstacle
                Invoke("EnAgent", .1f);//enable the agent
                if (Target == TheOrb)
                {
                    Target = null;
                }
            }
            else if (MinionObstacle.enabled == false)//else disable the agent
            {
                Minionagent.enabled = false;
                Invoke("EnObst", .1f);
                Target = TheOrb;
            }
        }
        else if (AgroTarget)//if minion has a target based on agro script
        {
            if (Target != null)
            {
                nodepathing = false;
            }
            if (Target == null || Target.activeInHierarchy == false)
            {
                AgroTarget = false;
                Target = null;
                nodepathing = true;//reactivate pathfinding
                MinionObstacle.enabled = false;//diable the obstacle
                Invoke("EnAgent", .1f);//enable the agent
                return;
            }

            if (Vector3.Distance(Target.transform.position, transform.position) > 5)//if minion is pushed away from the goal then pathfind back to the goal
            {
                nodepathing = true;//reactivate pathfinding
                MinionObstacle.enabled = false;//diable the obstacle
                Invoke("EnAgent", .1f);//enable the agent
            }
            else if (MinionObstacle.enabled == false)//else disable the agent
            {
                Minionagent.enabled = false;
                Invoke("EnObst", .1f);
            }

            if (Minionagent.enabled == true && targetdestcount > targetdesttimer)
            {
                Invoke("BeginMoveTarget", .1f);
                targetdestcount = 0;
            }
            targetdestcount += Time.deltaTime;
        }
        else if (nodepathing)//if none of the above are true then continue with normal pathfinding
        {
            Pathfinding();
        }
    }

    void AttackTarget()
    {
        if (Target != null)
        {
            float tempdist = Vector3.Distance(Target.transform.position, transform.position);
            if (Target.GetComponent<HealthScript>().CurrentHealth > 0 && tempdist < 5)
            {
                Target.GetComponent<HealthScript>().CurrentHealth -= MinionDamage;
            }
        }
    }

    void BeginMoveTarget()
    {
        if (Target == null || Target.activeInHierarchy == false)
        {
            AgroTarget = false;
            Target = null;
            nodepathing = true;//reactivate pathfinding
            MinionObstacle.enabled = false;//diable the obstacle
            Invoke("EnAgent", .1f);//enable the agent
            return;
        }
        if (Minionagent.enabled == false)
        {
            return;
        }

        if (Target == PlayerTarget)
        {
            bool Playeronpath = PlayerTarget.GetComponent<TargetProperty>().onpath;
            if (Playeronpath)
            {
                MinionObstacle.enabled = false;
                Minionagent.enabled = true;
                //this vector 3 is a random desitnation in a radius around the node. Thank you stephen
                //get the area mask so minions can only path on a surface that has the path of lane
                int mask = 0;//this is annoying but its the only way.  you cant make a layer mask for areas.  i wish you could
                mask += 0 << NavMesh.GetAreaFromName("Walkable");
                mask += 0 << NavMesh.GetAreaFromName("Not walkable");
                mask += 0 << NavMesh.GetAreaFromName("Jump");
                mask += 0 << NavMesh.GetAreaFromName("offPath");
                mask += 1 << NavMesh.GetAreaFromName("Lane");

                var pathOnlyLane = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, PlayerTarget.transform.position, mask, pathOnlyLane);//calculate the path using an area mask.  This makes it so minions wont leave the lane surface
                Minionagent.path = pathOnlyLane;
            }
            else
            {
                MinionObstacle.enabled = false;
                Minionagent.enabled = true;
                Minionagent.SetDestination(PlayerTarget.transform.position);
            }
        }
        else
        {
            if (onpath)
            {
                MinionObstacle.enabled = false;
                Minionagent.enabled = true;
                //this vector 3 is a random desitnation in a radius around the node. Thank you stephen
                //get the area mask so minions can only path on a surface that has the path of lane
                int mask = 0;//this is annoying but its the only way.  you cant make a layer mask for areas.  i wish you could
                mask += 0 << NavMesh.GetAreaFromName("Walkable");
                mask += 0 << NavMesh.GetAreaFromName("Not walkable");
                mask += 0 << NavMesh.GetAreaFromName("Jump");
                mask += 0 << NavMesh.GetAreaFromName("offPath");
                mask += 1 << NavMesh.GetAreaFromName("Lane");

                var pathOnlyLane = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, TargetRandPos, mask, pathOnlyLane);//calculate the path using an area mask.  This makes it so minions wont leave the lane surface
                Minionagent.path = pathOnlyLane;
            }
            else if (!onpath)//if the minion is off the path then he should path back to last known path position.
            {
                if (LastKnownLanePos != null)
                {
                    MinionObstacle.enabled = false;
                    Minionagent.enabled = true;
                    Minionagent.SetDestination(LastKnownLanePos.transform.position);
                }
            }
        }

        if (Minionagent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            if (Target = TurretTarget)
            {
                PathToTurretTargetIsPartial = true;
                Target = null;
            }
            else if (Target = PlayerTarget)
            {
                PathToPlayerTargetIsPartial = true;
                Target = null;
            }
        }
        else
        {
            PathToPlayerTargetIsPartial = false;
            PathToTurretTargetIsPartial = false;
        }
    }

    void Pathfinding()
    {
        if (CurNode < data2.Count && onpath) //if dist to node 1 >= dist to node 2, make sure node 2 is the curent target node.
        {
            for (int i = CurNode + 1; i < data2.Count; i++)
            {
                float tmp1 = Vector3.Distance(transform.position, data2[CurNode].Node.transform.position);
                float tmp2 = Vector3.Distance(transform.position, data2[i].Node.transform.position);
                if (tmp1 > tmp2)
                {
                    var temp = new PathArr();
                    temp.Node = data2[CurNode].Node;
                    temp.ReachedNode = true;
                    data2[CurNode] = temp;//the node is no longer reached
                }
            }
        }

        if (Minionagent.enabled == true && MinionObstacle.enabled == true)//this case should never happen .  If it does it needs to eb imemdiately resolved.
        {
            MinionObstacle.enabled = false;
            Debug.LogError("obstacle error has occured ON FP");
        }

        if (Vector3.Distance(data2[CurNode].Node.transform.position, transform.position) < StoppingDistance)//if the minion is close enough to the node, then set reached node to true
        {
            var temp = new PathArr();
            temp.Node = data2[CurNode].Node;
            temp.ReachedNode = true;
            data2[CurNode] = temp;//the node is no longer reached
        }

        if (data2.Count > 0 && data2[CurNode].ReachedNode == true && data2[data2.Count - 1].ReachedNode == false)//get the next destination
        {
            GetNextDestination();
        }
        else if (data2.Count > 0 && data2[data2.Count - 1].ReachedNode == false && recalcpath == false)//recalc the path every .5 seconds
        {
            recalcpath = true;
            Invoke("GetNextDestination", .5f);
        }
    }

    void GetNextDestination()//calculate the new destination based off of available nodes that the minion can path to.
    {
        if (data2[data2.Count - 1].ReachedNode == false)
        {
            for (int i = 0; i < data2.Count; i++)
            {
                if (data2[i].ReachedNode == false)
                {
                    CurNode = i;
                    break;
                }
            }
            MinionObstacle.enabled = false;
            Invoke("BeginMoveLane", .1f);
        }

        if (recalcpath)
        {
            recalcpath = false;
        }
    }

    void BeginMoveLane()//wait for a small window and then set nav agent destination.  This function will only navigate using Lane area type
    {
        if (Minionagent.enabled == true)
        {

            Minionagent.ResetPath();
            if (onpath)
            {
                //Debug.Log("setting path V1");
                MinionObstacle.enabled = false;
                Minionagent.enabled = true;
                //this vector 3 is a random desitnation in a radius around the node. Thank you stephen
                //get the area mask so minions can only path on a surface that has the path of lane
                int mask = 0;//this is annoying but its the only way.  you cant make a layer mask for areas.  i wish you could
                mask += 0 << NavMesh.GetAreaFromName("Walkable");
                mask += 0 << NavMesh.GetAreaFromName("Not walkable");
                mask += 0 << NavMesh.GetAreaFromName("Jump");
                mask += 0 << NavMesh.GetAreaFromName("offPath");
                mask += 1 << NavMesh.GetAreaFromName("Lane");

                var pathOnlyLane = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, data2[CurNode].position, mask, pathOnlyLane);//calculate the path using an area mask.  This makes it so minions wont leave the lane surface
                Minionagent.path = pathOnlyLane;
            }
            else if (!onpath)//if the minion is off the path then he should path back to last known path position.
            {
                if (LastKnownLanePos != null)
                {
                    MinionObstacle.enabled = false;
                    Minionagent.enabled = true;
                    Vector3 position = new Vector3(LastKnownLanePos.transform.position.x, 0, LastKnownLanePos.transform.position.z);
                    Minionagent.SetDestination(position);
                }
            }

            if (Minionagent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                PathToNextNodeIsPartial = true;
            }
            else
            {
                PathToNextNodeIsPartial = false;
            }
        }
    }

    void EnObst()//wait for a small window and then set nav agent destination.
    {
        Minionagent.enabled = false;
        MinionObstacle.enabled = true;
    }

    void EnAgent()//wait for a small window and then set nav agent destination.
    {
        MinionObstacle.enabled = false;
        Minionagent.enabled = true;
    }

    public void Slowed(float percentage, float time)
    {
        EditedSpeed = BaseSpeed * percentage;
        Debug.Log("Base speed" + BaseSpeed + "percentage" + percentage + "EditedSpeed" + EditedSpeed);

        Minionagent.speed = EditedSpeed;
        Debug.Log(Minionagent.speed);

        Invoke("resetSpeed", time);
    }

    public void Faster(float percentage, float time)
    {
        EditedSpeed = BaseSpeed * percentage;
        Minionagent.speed = EditedSpeed;

        Invoke("resetSpeed", time);
    }


    void resetSpeed()
    {
        Minionagent.speed = BaseSpeed;
        Debug.Log("SpeedReset");
        Debug.Log(Minionagent.speed);

    }

    public void Stunned(float time)
    {
        IsStunned = true;
        Debug.Log(gameObject.name + " is currently stunned for " + time + " seconds!");
        Minionagent.speed = 0f;
        StunVFX.SetActive(true);
        Invoke("Unstunned", time);
    }

    void Unstunned()
    {
        IsStunned = false;
        StunVFX.SetActive(false);
        resetSpeed();
    }

    void Sliding()
    {
        Minionagent.velocity = SlidingDirection;
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