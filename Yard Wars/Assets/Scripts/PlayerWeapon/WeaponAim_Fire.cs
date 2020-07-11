using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponAim_Fire : MonoBehaviour
{
    public GameObject Weapon;

    public GameObject projectile;

    private float WeaponCooldown;

    private void Update()
    {
        ShootController();
        WeaponCooldown += Time.deltaTime;
    }

    private void ShootController()
    {
        if (Input.GetButtonDown("Fire1") && WeaponCooldown > .5f)
        {
            CreateBullet();
            WeaponCooldown = 0;
        }
    }

    private void CreateBullet()
    {
        Instantiate(projectile, Weapon.transform.position, transform.rotation);
    }
}
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
}*/
