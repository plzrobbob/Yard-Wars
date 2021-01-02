using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponFire : MonoBehaviour
{
    public GameObject Weapon;
    private float WeaponCooldown;
    public float maxWeaponCooldown = 0.5f; //maximum seconds per attack
    public PlaceDefense m_placeDefense;
    public float attackDuration = .25f;
    public float attackSpeed = 1f;

    public bool attackSuccessful;
    public float damageAmount;
    public Collider[] HitTargets;
    public float area;
    public int layernum;
    private LayerMask layer;

    private void Start()
    {
        m_placeDefense = this.GetComponentInChildren<PlaceDefense>();

        //attackSuccessful = false;

        //Debug.Log("layernum equals " + layernum);

        if (layernum == 20)
        {
            layer = LayerMask.GetMask("Team2");
        }
        else if (layernum == 21)
        {
            layer = LayerMask.GetMask("Team1");
        }
    }

    private void Update()
    {
        AttackController();
        WeaponCooldown += Time.deltaTime;
    }

    private void AttackController()
    {
        if (Input.GetButtonDown("Fire1") && WeaponCooldown > maxWeaponCooldown && !m_placeDefense.placing)
        {
            HitTargets = Physics.OverlapSphere(transform.position, area, layer);
            DoDamage();

            //StartCoroutine(AttackDuration());

            Debug.Log("Attack underway!");

            WeaponCooldown = 0;
        } 

        //The idea behind this attack is that you activate the hitbox of the melee weapon when pressing down the fire1 button and while the hitbox is out
        //it causes damage to anything that touches it.
    }

    IEnumerator AttackDuration()
    {

        yield return new WaitForSeconds(attackDuration);
        //Debug.Log("Attack Duration Coroutine has is fukcing hapened are yuo having am stroke?");
    }

    void DoDamage()
    {
        for (int i = 0; i < HitTargets.Length; i++)
        {
            //attackSuccessful = true;
            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();
            M_HealthScript.CurrentHealth -= damageAmount;
            Debug.Log("Name of thing being damaged is: " + M_HealthScript);
        }
    }

   /* void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, area);
    } */


}
