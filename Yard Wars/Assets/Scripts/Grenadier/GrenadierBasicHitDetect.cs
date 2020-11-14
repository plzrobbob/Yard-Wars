using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierBasicHitDetect : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider[] HitTargets;
    private Vector3 overlapCheck;
    public float damage;
    public float area;

    void Start()
    {
        damage = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<GrenadierStatHandler>().GrenadierDamageCall();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        HitTargets = Physics.OverlapSphere(collision.contacts[0].point, area);
        DoDamage();
        Destroy(gameObject);
    }
    private void DoDamage()
    {
        for (int  i = 0; i < HitTargets.Length; i++)
        {
            if ( HitTargets[i].gameObject.tag == "Enemy")
            {
                HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();
                M_HealthScript.CurrentHealth -= damage;
            }
        }
    }
}
