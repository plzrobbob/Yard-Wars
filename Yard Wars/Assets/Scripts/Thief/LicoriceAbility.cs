using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LicoriceAbility : MonoBehaviour
{
    public GameObject Weapon;

    public GameObject projectile;

    private float WeaponCooldown;

    public PlaceDefense m_placeDefense;

    public Quaternion LicoRotate = Quaternion.Euler(0, 14.75f, 0);


    private void Start()
    {
        m_placeDefense = this.GetComponentInChildren<PlaceDefense>();
    }

    private void Update()
    {
        ShootController();
        WeaponCooldown += Time.deltaTime;
    }

    private void ShootController()
    {
        if (Input.GetButtonDown("Ability2") && WeaponCooldown > .5f && !m_placeDefense.placing)
        {
            CreateBullet();
            WeaponCooldown = 0;
        }
    }

    private void CreateBullet()
    {
        //projectile.transform.rotation = Quaternion.Euler(0, 14.75f, 0);
        Instantiate(projectile, Weapon.transform.position, LicoRotate * transform.rotation);
    }
}

//keep this cause its cool 
/*
    public GameObject m_Camera;
    public GameObject Weapon;
    public GameObject PlayerCapsule;
    public GameObject PlayerHolder;

    public GameObject projectile;

    public LayerMask mask;

    public Vector3 GunTarget;
    void Start()
    {
        m_Camera = GameObject.FindWithTag("MainCamera");
        Weapon = GameObject.FindWithTag("Weapon");
        PlayerCapsule = GameObject.FindWithTag("PlayerCapsule");
        PlayerHolder = GameObject.FindWithTag("PlayerHolder");
    }

    void FixedUpdate()
    {
        ControllLook();
        Debug.Log(GunTarget);
    }

    private void Update()
    {
        ShootController();
    }

    private void ControllLook()
    {

        Debug.DrawRay(m_Camera.transform.position, m_Camera.transform.forward * 100, Color.green);
        bool ray = Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out var hit, 100, mask);
        if (ray)
        {
            Vector3 fromPosition = Weapon.transform.position;
            Vector3 toPosition = hit.point;
            Vector3 direction = toPosition - fromPosition;
            Physics.Raycast(Weapon.transform.position, direction, out var hit2, 100);
            Debug.DrawRay(Weapon.transform.position, direction * 100, Color.red);
            GunTarget = hit2.point;
        }
        else if (!ray)
        {
            Physics.Raycast(Weapon.transform.position, m_Camera.transform.forward, out var hit3, 50);
            Debug.DrawRay(Weapon.transform.position, m_Camera.transform.forward * 100, Color.blue);
            GunTarget = hit3.point;
        }
    }

    private void ShootController()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("fire");
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        var bullet = Instantiate(projectile, Weapon.transform.position, transform.rotation);
        bullet.GetComponent<BulletTransform>().Target = GunTarget;
    }
}

    */