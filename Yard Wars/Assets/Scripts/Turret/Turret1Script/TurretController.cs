using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject target;

    public int range;

    public string EnemyTag;

    public Transform PartToRotate;

    public float TurnSpeed = 5f;

    public GameObject PlaceHoldertarget;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, .5f );
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        float shortestDitance = Mathf.Infinity;
        GameObject nearestEnemy = null;


        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDitance)
            {
                shortestDitance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (target != null && Vector3.Distance(transform.position, target.transform.position) > range)
        {
            target = null;
        }

        if (nearestEnemy != null && shortestDitance <= range)
        {
            if (target == null)
            {
                target = nearestEnemy;
            } 
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        //if (target == null)
        //{
        //    return;
        //}
        try
        {
            if (target.GetComponent<HealthScript>().CurrentHealth <= 0)
            {
                target = null;
            }
        }
        catch { }

        if (target != null)
        {
            Vector3 dir = (target.transform.position - transform.position);
            Quaternion LookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, LookRotation, Time.deltaTime * TurnSpeed).eulerAngles;
            PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        else
        {
            Vector3 dir = (PlaceHoldertarget.transform.position - transform.position);
            Quaternion LookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, LookRotation, Time.deltaTime * TurnSpeed).eulerAngles;
            PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
