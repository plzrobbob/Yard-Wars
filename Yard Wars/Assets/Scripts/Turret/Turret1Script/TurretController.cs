using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject target;

    public GameObject TurretHead;

    public GameObject[] Enemies;

    [SerializeField] int range;

    public GameObject PlaceHolderTarget;

    [SerializeField] float shootspeed;
    [SerializeField] float turretHeadSpeed;

    private Coroutine co;
    private bool coRunning;

    public GameObject projectile;
    public GameObject bulletstartpos;

    private bool finishedaiming;

    private void Start()
    {
        target = PlaceHolderTarget;
        co = null;
    }

    void Update()
    {
        //if the target is dead or out of range then change target back to palceholder target
        if (target.activeSelf == false ||target == null || Vector3.Distance(this.gameObject.transform.position, target.transform.position) > range || target.GetComponent<HealthScript>().CurrentHealth <= 0)
        {
            target = PlaceHolderTarget;
        }

        //attemp to see if target is null
        if (!target || target == null || target.Equals(null) || target.gameObject.Equals(null) || target.activeInHierarchy == false || target.activeSelf == false || target.gameObject == null)
        {
            Debug.Log("null");
        }

        GameObject tempTarget = null;
        //if the target has not been set or is null then fin an array of gameojbects and see if the enemy is in the turrets range
        if (target.activeSelf == false || target == PlaceHolderTarget)
        {
            //find enemies and add them to the enemy array 
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //set the target ot the nearest enemy in range of the turret
            tempTarget = FindTarget();
            if (tempTarget != null)
            {
                target = tempTarget;
                tempTarget = null;
            }
            else 
            {
                target = PlaceHolderTarget;
            }
        }
        Vector3 temp = target.transform.position;
        temp.y += .5f;
        Quaternion lookOnLook = Quaternion.LookRotation(temp - TurretHead.transform.position);
        //change the turret head rotation to face the target
        if (target.activeSelf != false)
        {
            TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, lookOnLook, turretHeadSpeed * Time.deltaTime);
        }

        Debug.Log(lookOnLook);
        Debug.Log(TurretHead.transform.localRotation);

        if (lookOnLook.y -.05f <= TurretHead.transform.localRotation.y && TurretHead.transform.localRotation.y <= lookOnLook.y+.05f)
        {
            finishedaiming = true;
        }
        else 
        {
            finishedaiming = false;
        }

        //if a target is found begin shooting
        if ((target.activeSelf != false && target != null && target != PlaceHolderTarget) && !coRunning && finishedaiming)
        {
            coRunning = true;
            co = StartCoroutine(TurretFire());
        }

        if (co != null)
        {
            //if no taget is found then dont shoot
            if (((target.activeSelf == false || target == null || target == PlaceHolderTarget) && coRunning) || !finishedaiming)
            {
                coRunning = false;
                StopCoroutine(co);
                co = null;
            }
        }
    }

    public GameObject FindTarget()
    {
        float smallestrangeHolder = range;
        GameObject temptarget = null;
        for (int i = 0; i < Enemies.Length; i++)
        {
            float tempdist = Vector3.Distance(this.gameObject.transform.position, Enemies[i].transform.position);
            if (tempdist < range && Enemies[i].activeInHierarchy && Enemies[i].GetComponent<HealthScript>().CurrentHealth > 0)
            {
                if (tempdist < smallestrangeHolder)
                {
                    smallestrangeHolder = tempdist;
                    temptarget = Enemies[i];
                }
            }
        }
        return temptarget;
    }

    public IEnumerator TurretFire()
    {
        while (target.activeSelf != false || PlaceHolderTarget)
        {
            GameObject go = Instantiate(projectile, bulletstartpos.transform.position, transform.rotation);
            go.GetComponent<TurretBullet1>().Weapon = TurretHead;
            yield return new WaitForSeconds(shootspeed);
        }
    }
}
