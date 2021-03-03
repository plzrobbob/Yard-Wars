using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public float damage;
    public int target;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        Invoke("DestroyBullet", 2.0f);
        Debug.Log("COLDijougfeeiuwyfgbviwe" + collision.gameObject.layer + "efhibveqwiyfv" + target);
        if (collision.gameObject.layer == target)
        {
            DoDamage(collision);
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
    private void DoDamage(Collision collision)
    {
        HealthScript M_HealthScript = collision.gameObject.GetComponent<HealthScript>();
        M_HealthScript.CurrentHealth -= damage;
    }
}
