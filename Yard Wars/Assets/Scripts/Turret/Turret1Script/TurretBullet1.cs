using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet1 : MonoBehaviour
{
    private GameObject Target;

    [Header("This is set in the turret GO")]
    public float Damages;
    public float speed;

    public void Chase(GameObject _target)
    {
        Target = _target;
    }

    private void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = Target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        Destroy(gameObject);
        HealthScript M_HealthScript = Target.gameObject.GetComponent<HealthScript>();
        M_HealthScript.DamageHandler();
        M_HealthScript.CurrentHealth -= Damages;
    }
}