using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletTransform : MonoBehaviour
{
    public GameObject Weapon;

    private Rigidbody rb;
    public float bulletSpeed;
    public float mass;
    public float maxmass;
    public float massMult;
    private Vector3 bullettransfrom;

    private float bulletTime;

    private void Start()
    {
        Weapon = GameObject.FindWithTag("Weapon");
        rb = GetComponent<Rigidbody>();
        bullettransfrom = Weapon.transform.right;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + bullettransfrom * bulletSpeed * Time.fixedDeltaTime);

        if (mass < maxmass)
        {
            mass += Time.deltaTime;
        }

        rb.AddForce(-transform.up * mass * massMult * Time.deltaTime);

        bulletTime += Time.deltaTime;
        if (bulletTime > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        Destroy(gameObject);
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

    }
}
