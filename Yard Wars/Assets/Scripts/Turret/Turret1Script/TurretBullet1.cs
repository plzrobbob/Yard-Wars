using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet1 : MonoBehaviour
{
    private GameObject Target;
    private Vector3 targettransform;
    private float timer = 10;
    private float bullettime;
    private Rigidbody rb;

    [Header("This is set in the turret GO")]
    public float Damages;
    public float speed;
    public string EnemyTag;

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
        //M_HealthScript.DamageHandler();
        M_HealthScript.CurrentHealth -= Damages;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if (collision.gameObject.tag != EnemyTag)
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == EnemyTag)
        {
            if (collision.gameObject.GetComponent<HealthScript>().CurrentHealth > 0)
            {
                collision.gameObject.GetComponent<HealthScript>().CurrentHealth -= Damages;
            }
            Destroy(gameObject);
        }
    }
}