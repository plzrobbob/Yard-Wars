using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAbilityTwo : MonoBehaviour
{
    public GameObject Weapon;
    private float AbilityTwoCooldown = 6f;
    public float maxAbilityTwoCooldown = 6f; //maximum seconds per attack
    public PlaceDefense m_placeDefense;

    //bool canUseAbility = true;
    //public float abilityTwoCooldown = 7f;

    public bool attackSuccessful;
    public float abilityTwoDamageAmount;
    public Collider[] HitTargets;
    public float area;
    public int layernum;
    private LayerMask layer;
    public float stunDuration;

    public float pushMagnitude;

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
        AbilityTwoController();
        AbilityTwoCooldown += Time.deltaTime;

    }

    private void AbilityTwoController()
    {
        if (Input.GetButtonDown("Ability Two") && AbilityTwoCooldown > maxAbilityTwoCooldown)
        {
            HitTargets = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.75f), area, layer);
            DoDamage();

            //StartCoroutine(AttackDuration());

            Debug.Log("Ability 2 bein used dawg!");

            AbilityTwoCooldown = 0;
        }
        else
        {
            Debug.Log("Damn dawg be patient! Ability Two is still charging up!");
        }
    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();
            M_HealthScript.CurrentHealth -= abilityTwoDamageAmount;
            Debug.Log("Name of thing being damaged is: " + M_HealthScript);

            //Disable Nav Mesh Agent
            HitTargets[i].GetComponent<NavMeshAgent>().enabled = false;

            //Enable nav mesh obstacle
            HitTargets[i].GetComponent<NavMeshObstacle>().enabled = true;

            //Add Rigidbody
            Rigidbody minionRigidBody = HitTargets[i].gameObject.AddComponent<Rigidbody>();

            //Add force to the rigidbody
            Vector3 force = (HitTargets[i].transform.position) - (transform.position);
            minionRigidBody.AddForce(force * pushMagnitude);

            //once velocity = 0 destroy the rigidbody, disable the navmesh obstacle, Enable Nav Mesh Agent, and stun the minion


            /*if (minionRigidBody.velocity == new Vector3 (0,0,0))
            {
                Destroy(minionRigidBody);
                HitTargets[i].GetComponent<NavMeshObstacle>().enabled = false;
                HitTargets[i].GetComponent<NavMeshAgent>().enabled = true;

                Pathfinding pathfindingScript = HitTargets[i].GetComponent<Pathfinding>();
                pathfindingScript.Stunned(stunDuration);
            }*/

            StartCoroutine(RigidBodySpeedTest(HitTargets[i].gameObject));
        }
    }

    IEnumerator RigidBodySpeedTest(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Rigidbody minionRB = obj.GetComponent<Rigidbody>();
        Destroy(minionRB);
        obj.GetComponent<NavMeshObstacle>().enabled = false;
        obj.GetComponent<NavMeshAgent>().enabled = true;

        Pathfinding pathfindingScript = obj.GetComponent<Pathfinding>();
        pathfindingScript.Stunned(stunDuration);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.75f), area);
    }
}
