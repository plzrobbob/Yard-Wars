using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackLicoriceAbility : MonoBehaviour
{
    private IEnumerator whatever;
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
        /*
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }
        */
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
    /*

    private void OnCollision(Collision collision)
    {
        
            Debug.Log("hit");
            whatever = SpecialDuration(collision);
            StartCoroutine(whatever);
            Debug.Log("Coroutine start");
            
        if (collision.gameObject.tag == EnemyTag)
        {
            Debug.Log("Hey an enemy got hit");
            collision.gameObject.SendMessage("SpecialDuration", 4);
        }
    }
*/
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("SpecialDuration");
        }
        Destroy(gameObject);
    }
    


    IEnumerator SpecialDuration(Collision collision)
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
 