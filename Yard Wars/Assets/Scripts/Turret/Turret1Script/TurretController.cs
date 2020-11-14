using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("SetupFields")]
    public GameObject target;
    public Transform PartToRotate;
    public GameObject PlaceHoldertarget;
    public string EnemyTag;

    public GameObject BulletPrefab;
    public Transform FirePoint;

    [Header("Attributes")]
    public float TurnSpeed = 5f;
    public float fireRate = 1f;
    public float range;
    public float bulletDamages;
    public float bulletSpeed;
    private float canshootTimer;
    public float timerToShoot;
    private float fireCountDown = 0f;

    public GameObject[] enemies;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, .5f );
    }

    void UpdateTarget()
    {
        Debug.Log("Update");
        enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        float shortestDitance = Mathf.Infinity;
        GameObject nearestEnemy = null;


        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDitance && enemy.GetComponent<HealthScript>().CurrentHealth > 0)
            {
                shortestDitance = distanceToEnemy;
                nearestEnemy = enemy;
            }
            //else if (distanceToEnemy < shortestDitance && enemy.GetComponent<HealthScript>().CurrentHealth > 0 && enemy.tag=="EnemyPlayer")
            //{//use this if turret can target enemy players
            //    shortestDitance = distanceToEnemy;
            //    nearestEnemy = enemy;
            //}
        }

        if (target != null && Vector3.Distance(transform.position, target.transform.position) > range)
        {
            target = null;
        }
        else if (nearestEnemy != null && shortestDitance <= range)
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
            canshootTimer = 0;
        }

        if (fireCountDown <= 0f && canshootTimer >= timerToShoot)
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }
        canshootTimer += Time.deltaTime;
        fireCountDown -= Time.deltaTime; 
    }
    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        TurretBullet1 bullet = bulletGO.GetComponent<TurretBullet1>();

        if (bullet != null)
        {
            bullet.Chase(target);
            bullet.Damages = bulletDamages;
            bullet.speed = bulletSpeed;
            bullet.EnemyTag = EnemyTag;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
