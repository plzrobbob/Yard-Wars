using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAbilityOne : MonoBehaviour
{
    public bool attackSuccessful;
    public float damageAmount;
    public float slowDownDuration;

    bool canUseAbility = true;
    public float abilityOneDuration = 5f;
    public float abilityOneCooldown = 7f;

    public Collider[] HitTargets;
    private Vector3 overlapCheck;
    public float area;
    public int layernum;
    private LayerMask layer;

    private float originalEnemySpeed;

    public Transform sphereOne;
    public Transform sphereTwo;


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

        if (Input.GetKeyDown("q") && canUseAbility)
        {
            canUseAbility = false;
            AttackController();

            StartCoroutine(AbilityCooldown());
            //StartCoroutine(AbilityDuration());
        }

    }

    private void AttackController()
    {
            //HitTargets = Physics.OverlapSphere(transform.position, area, layer);
            HitTargets = Physics.OverlapCapsule(sphereOne.position, sphereTwo.position, area, layer);
            DoDamage();

            Debug.Log("Attack underway!");
    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();
            M_HealthScript.CurrentHealth -= damageAmount;

            NavMeshAgent navAgent = HitTargets[i].GetComponent<NavMeshAgent>();
            originalEnemySpeed = navAgent.speed;
            Debug.Log("originalEnemySpeed is: " + originalEnemySpeed);
            StartCoroutine(EnemySlowDown(navAgent, originalEnemySpeed));

            Debug.Log("Name of thing being damaged is: " + M_HealthScript);
        }
    }

    IEnumerator EnemySlowDown(NavMeshAgent navAgentEnumerator, float originalSpeed)
    {
        navAgentEnumerator.speed = 1f;
        yield return new WaitForSeconds(slowDownDuration);
        navAgentEnumerator.speed = originalSpeed;
    }

    IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(abilityOneCooldown);
        canUseAbility = true;
        Debug.Log("Ability Cooldown Coroutine is a gogogo!!");
    }

    IEnumerator AbilityDuration()
    {
        yield return new WaitForSeconds(abilityOneDuration);
        Debug.Log("Ability Duration Coroutine is a gogo");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(sphereOne.position, area);
        Gizmos.DrawSphere(sphereTwo.position, area);
    }
}
