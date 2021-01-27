using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAbilityOne : MonoBehaviour
{
    public bool attackSuccessful;
    public float abilityOneDamageAmount;
    public float slowDownDuration;

    private float AbilityOneCooldown = 6f;
    public float maxAbilityOneCooldown = 6f; //maximum seconds per attack

    public Collider[] HitTargets;
    private Vector3 overlapCheck;
    public float area;
    public int layernum;
    private LayerMask layer;

    private float originalEnemySpeed;

    public Transform sphereOne;
    public Transform sphereTwo;

    public float slowPercent; // originally this was set to: 0.5f
    public float slowDuration; // originally this was set to: 5f


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
        AbilityOneController();

        AbilityOneCooldown += Time.deltaTime;

    }

    private void AbilityOneController()
    {
        if (Input.GetButtonDown("Ability One") && AbilityOneCooldown > maxAbilityOneCooldown)
        {
            HitTargets = Physics.OverlapCapsule(sphereOne.position, sphereTwo.position, area, layer);
            DoDamage();

            AbilityOneCooldown = 0;

            Debug.Log("Ability one currently engaged motherfucker!");
        }
        else
        {
            Debug.Log("Ability One is still charging up dude, quit mashin the button!");
        }

    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();
            M_HealthScript.CurrentHealth -= abilityOneDamageAmount;

            Pathfinding pathfindingScript = HitTargets[i].GetComponent<Pathfinding>();
            pathfindingScript.Slowed(slowPercent, slowDuration);

            /*NavMeshAgent navAgent = HitTargets[i].GetComponent<NavMeshAgent>();
            originalEnemySpeed = navAgent.speed;
            Debug.Log("originalEnemySpeed is: " + originalEnemySpeed);
            StartCoroutine(EnemySlowDown(navAgent, originalEnemySpeed));*/

            Debug.Log("Name of thing being damaged is: " + M_HealthScript);
        }
    }

    IEnumerator EnemySlowDown(NavMeshAgent navAgentEnumerator, float originalSpeed)
    {
        navAgentEnumerator.speed = 1f;
        yield return new WaitForSeconds(slowDownDuration);
        navAgentEnumerator.speed = originalSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(sphereOne.position, area);
        //Gizmos.DrawSphere(sphereTwo.position, area);
    }
}
