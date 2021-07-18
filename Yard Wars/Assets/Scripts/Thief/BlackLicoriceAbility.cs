using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackLicoriceAbility : MonoBehaviour
{
    private GameObject Target;
    private Vector3 targettransform;
    private float timer = 10;
    private float bullettime;
    private Rigidbody rb;
    public float TimeFrame = 1;
    public bool DottyTime = false;
    

    
    [Header("This is set in the turret GO")]
    public float Damages;
    public float speed;
    public string EnemyTag = "Enemy";
    
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
        rb.AddForce(transform.forward * speed);

        if (bullettime >= timer)
        {
            Destroy(gameObject);
        }
        bullettime += Time.deltaTime;
    }

    
    private void OnCollisionEnter(Collision collision) // If the bullet collides
    {
        Debug.Log("hit"); 
        if (collision.gameObject.tag == "Enemy")  //and the collider is an enemy
        {
            collision.gameObject.SendMessage("SpecialDuration"); // Activate DOT Damage
        }
        Destroy(gameObject);
    }
    

    // BELOW IS A TEST FUNCTION, THE REAL FUNCTION IS ON THE HEALTHSCRIPT
    IEnumerator SpecialDuration(Collision collision) // This code is where the Dot Damage works.
    {
        int i = 4;
        for (i = 0; i < 4; i++)   // for three seconds, 0 > 1 > 2 > 3 but not 4
        {
            collision.gameObject.GetComponent<HealthScript>().CurrentHealth -= 1; // take away one damage
            Debug.Log("Health of the Minion: " + collision.gameObject.GetComponent<HealthScript>().CurrentHealth);
            Debug.Log(i);
            yield return new WaitForSeconds(1f); // waits the specified timeframe
        }
    }
}
 