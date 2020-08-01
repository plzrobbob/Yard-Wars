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

    private void Start()
    {
        target = PlaceHolderTarget;
    }

    void Update()
    {
        UpdateTarget();

        GameObject tempTarget = null;
        if (target == null || target == PlaceHolderTarget)
        {
            FindPossibleTarget();
            tempTarget = FindTarget();
            if (tempTarget != null)
            {
                target = tempTarget;
                tempTarget = null;
            }
        }

        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - TurretHead.transform.position);
        TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, lookOnLook, Time.deltaTime);
    }

    public void UpdateTarget()
    {
        if (!target.activeInHierarchy && target != PlaceHolderTarget || Vector3.Distance(this.gameObject.transform.position, target.transform.position) > range)
        {
            target = PlaceHolderTarget;
        }
    }

    public void FindPossibleTarget()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public GameObject FindTarget()
    {
        float smallestrangeHolder = range;
        GameObject temptarget = null;
        for (int i = 0; i < Enemies.Length; i++)
        {
            float tempdist = Vector3.Distance(this.gameObject.transform.position, Enemies[i].transform.position);
            if (tempdist < range && Enemies[i].activeInHierarchy)
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
}
