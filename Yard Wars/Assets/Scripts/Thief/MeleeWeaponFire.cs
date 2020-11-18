using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponFire : MonoBehaviour
{
    public GameObject Weapon;
    private float WeaponCooldown;
    public PlaceDefense m_placeDefense;
    public float attackDuration = .25f;
    public float attackSpeed = 1f;

    private void Start()
    {
        m_placeDefense = this.GetComponentInChildren<PlaceDefense>();
    }

    private void Update()
    {
        AttackController();
        WeaponCooldown += Time.deltaTime;
    }

    private void AttackController()
    {
        if (Input.GetButtonDown("Fire1") && WeaponCooldown > .5f && !m_placeDefense.placing)
        {
            //Weapon.GetComponent<MeshRenderer>().enabled = true; //This is just for test purposes, the real script will modify the trigger collider instead
            Weapon.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(AttackDuration());
            WeaponCooldown = 0;
        } 

        //The idea behind this attack is that you activate the hitbox of the melee weapon when pressing down the fire1 button and while the hitbox is out
        //it causes damage to anything that touches it.
    }

    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(attackDuration);
        //Weapon.GetComponent<MeshRenderer>().enabled = false; //This is just for test purposes, the real script will modify the trigger collider instead
        Weapon.GetComponent<BoxCollider>().enabled = false;
        //Debug.Log("Attack Duration Coroutine has is fukcing hapened are yuo having am stroke?");
    }


}
