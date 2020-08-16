using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet1 : MonoBehaviour
{
    private GameObject Target;
    public Vector3 targettransform;
    private float timer = 10;
    private float bullettime;
    private Rigidbody rb;

    [Header("This is set in the turret GO")]
    public float Damages;
    public float speed;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Chase(GameObject _target)
    {
        Target = _target;
        targettransform = Target.transform.position;
    }

    private void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, Target.transform.position) < 1)
        {
            HitTarget();
            return;
        }

        rb.AddForce(transform.forward * speed);

        if (bullettime >= timer)
        {
            Destroy(gameObject);
        }
        bullettime += Time.deltaTime;
    }

    void HitTarget()
    {
        Destroy(gameObject);
        HealthScript M_HealthScript = Target.gameObject.GetComponent<HealthScript>();
        M_HealthScript.DamageHandler();
        M_HealthScript.CurrentHealth -= Damages;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if (collision.gameObject != Target)
        {
            Destroy(gameObject);
        }
    }
}