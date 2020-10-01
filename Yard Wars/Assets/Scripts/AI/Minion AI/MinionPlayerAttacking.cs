using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionPlayerAttacking : MonoBehaviour
{
    public int MinionDamage;
    public Pathfinding m_pathfinding;
    public GameObject Player;
    public Vector3 previousnode;
    private Vector3 target;
    private NavMeshAgent navAgent;
    private float startHealth;
    private float updateHealth;
    private bool test;
    public Coroutine co;
    public Coroutine sho;
    void Start()
    {
        startHealth = gameObject.GetComponent<HealthScript>().CurrentHealth;
        navAgent = GetComponent<NavMeshAgent>();
        //previousnode = m_pathfinding.target;
        InvokeRepeating("UpdateTracker", 0f, .5f);
    }

    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerHolder");

    }

    public void UpdateTracker()
    {
        updateHealth = gameObject.GetComponent<HealthScript>().CurrentHealth;
        if (updateHealth < startHealth)
        {
            Debug.Log("startattack");
            startHealth = gameObject.GetComponent<HealthScript>().CurrentHealth;
            target = Player.transform.position;
            test = true;
            try 
            { 
                StopCoroutine(co);
                StopCoroutine(sho);
            } 
            catch { }
            co = StartCoroutine(TargetTracker());
            sho = StartCoroutine(Attacktime());
        }
    }

    public IEnumerator Attacktime()
    {
        while (test)
        {
            float distance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
            Debug.Log(distance);
            if (distance < 3)
            {
                Debug.Log("hit");
                Player.GetComponent<HealthScript>().CurrentHealth -= MinionDamage;
            }
            yield return new WaitForSeconds(1);
        }
    }
    public IEnumerator TargetTracker()
    {
        while(test)
        {
            target = Player.transform.position;
            m_pathfinding.enabled = false;
            float distance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
            if (navAgent.pathStatus == NavMeshPathStatus.PathPartial || distance > 20 || Player.GetComponent<HealthScript>().CurrentHealth <= 0)
            {
                test = false;
                target = previousnode;
                m_pathfinding.enabled = true;
                StopAllCoroutines();
            }
            navAgent.SetDestination(target);

            yield return null;
        }
    }
    //public IEnumerator DamageTracker()
    //{
    //    startHealth = gameObject.GetComponent<HealthScript>().CurrentHealth;
    //    while(true)
    //    {
    //        yield return new WaitForSeconds(DamageCheckspeed);
    //        updateHealth = gameObject.GetComponent<HealthScript>().CurrentHealth;

    //        if (updateHealth <= startHealth - 1)//a player has done damage to the minion
    //        {
    //            float distance = Vector3.Distance(transform.position, Player.transform.position);
    //            if (distance <= 20)
    //            {

    //                m_pathfinding.currentNode = Player;

    //                if (navAgent.pathStatus == NavMeshPathStatus.PathPartial && m_pathfinding.currentNode == Player)//check if minion can path to player
    //                {
    //                    m_pathfinding.currentNode = previousnode;
    //                }

    //                if (distance <= 3)
    //                {
    //                    Player.GetComponent<HealthScript>().CurrentHealth -= MinionDamage;
    //                }
    //            }
    //            else if(m_pathfinding.currentNode == Player)
    //            {
    //                m_pathfinding.currentNode = previousnode;
    //            }
    //        }
    //        navAgent.SetDestination(m_pathfinding.currentNode.transform.position);
    //        startHealth = gameObject.GetComponent<HealthScript>().CurrentHealth;
    //    }
    //}
}
