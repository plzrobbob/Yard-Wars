using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierAbilityTwoBola : MonoBehaviour
{

    public float damage;
    public float slowTime;
    [Range(0.00f, 1.00f)]
    public float slowSpeed;
    public int targetNum;
    public LayerMask target;
    public LayerMask Team;

    float distanceTravelled;
    public float distanceAllowed;

    Vector3 oldPosition;
    [Range(0.0f, 10.0f)]
    public float rot;
    // Start is called before the first frame update
    void Start()
    {
        if (targetNum == 20)
        {
            target = LayerMask.GetMask("Team1");
           // Team = LayerMask.GetMask("Team1");
            gameObject.layer = LayerMask.NameToLayer("Team2");

        }
        else if (targetNum == 21)
        {
            target = LayerMask.GetMask("Team2");
           // Team = LayerMask.GetMask("Team2");
            gameObject.layer = LayerMask.NameToLayer("Team1");
        }
        else
        {
            Debug.Log("Well something went wrong. I didn't really plan for this to happen in GrenadierAbilityTwoBola.");
        }
        oldPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // gameObject.transform.Rotate(new Vector3(0f, rot, 0f));
        distanceTravelled += Vector3.Distance(gameObject.transform.position, oldPosition);
        oldPosition = gameObject.transform.position;
       // Debug.Log(distanceTravelled);

        if (distanceTravelled > distanceAllowed)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Stop();
        Debug.Log("Hit something");
        Debug.Log(collision.gameObject.layer);
        Debug.Log(targetNum);
        if (collision.gameObject.layer == targetNum)
        {
            Debug.Log("Gets Here");
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //DamagedMinion(collision);
                SlowedMinion(collision);

            }
            else if (collision.gameObject.CompareTag("PlayerHolder"))
            {
              //  DamagedPlayer(collision);
                SlowedPlayer(collision);
            }
            Damaged(collision);

        }
        else
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            Invoke("DestroyObject", 2.0f);
        }
    }

    void SlowedMinion(Collision collision)
    {
        collision.gameObject.GetComponent<Pathfinding>().Slowed(0.65f, slowTime);
    }
    void SlowedPlayer(Collision collision)
    {
        collision.gameObject.GetComponent<PlayerCharacterController>().Slowed(0.65f, slowTime);
    }

    void Damaged(Collision collision)
    {
        Debug.Log("We have done damage to: " + collision.gameObject.name);
        collision.gameObject.GetComponent<HealthScript>().CurrentHealth -= damage;
        Destroy(gameObject);
    }




    //I don't need this.... Yet....
    void DamagedMinion(Collision collision)
    {
    }

    void DamagedPlayer(Collision collision)
    {

    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
