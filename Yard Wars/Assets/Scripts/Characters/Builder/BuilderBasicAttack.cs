using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBasicAttack : MonoBehaviour
{
    public float damage;
    public GameObject Weapon;
    public GameObject Projectile;
    private float FiringCooldown;
    public GameObject RotationSetter;
    public float FireCooldown;
    public float ExitSpeed;

    public float Rounds_In_Mag; //This is for later :)

    public PlaceDefense m_placeDefense;

    // Start is called before the first frame update
    void Start()
    {
        m_placeDefense = this.GetComponentInChildren<PlaceDefense>();
        FireCooldown = 2f;
        ExitSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && FireCooldown > 1f)
        {
            CreateBullet();
            FireCooldown = 0f;
        }
        FireCooldown += Time.deltaTime;
    }


    private void CreateBullet()
    {
       GameObject obj = Instantiate(Projectile, Weapon.transform.position, RotationSetter.transform.rotation);
        obj.GetComponent<Rigidbody>().velocity = (Camera.main.transform.forward * ExitSpeed);
        obj.GetComponent<BulletHit>().damage = damage;
    }
}
