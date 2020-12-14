using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierAbilitytwodamage : MonoBehaviour
{
    /// <summary>
    /// So this script is going to run the same exact issue I have with the ability one. I need it to differentiate from the player layermask and enemy layermask
    /// I really need to solve this problem. I will work it as is for now, but the window will soon close on being allowed to do this.
    /// </summary>

    public LayerMask EnemyMask;
   
    public GameObject FallingBalloon;
    public Transform FBalloonSpawn;
    public float damage;
    public float range;
    public Collider[] enemiesHit;
    public int numberLETSFUCKINGGOOOOOOBOYYYYSSSSS;
    public GameObject Self;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider collision)
    {
        Debug.Log("Detected");
        Debug.Log(collision.gameObject.layer);
        Debug.Log(EnemyMask);


        var EnemyMask1 = EnemyMask >> collision.gameObject.layer;
        string string1 = EnemyMask1.ToString();
        var EnemyMask2 = EnemyMask << collision.gameObject.layer;
       string  string2 = EnemyMask2.ToString();

         string string3 = string1 + string2;
        Debug.Log("String3 = " + string3);
        float value = float.Parse(string3);
        Debug.Log(value);
        Debug.Log(collision.gameObject.layer == numberLETSFUCKINGGOOOOOOBOYYYYSSSSS);


        if (collision.gameObject.layer == numberLETSFUCKINGGOOOOOOBOYYYYSSSSS)
        {
            enemiesHit = Physics.OverlapSphere(transform.position, range, EnemyMask);
            Debug.Log("enemiesHit = " + EnemyMask);
            DoDamage();
            SlowEnemy();
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false; //This should disable all children right?
            collision.gameObject.GetComponent<HealthScript>().CurrentHealth -= damage;


            GameObject obj = Instantiate(FallingBalloon, FBalloonSpawn.position, FBalloonSpawn.rotation);
            Destroy(Self);
            //I am adding this to speed up it hitting the ground
            obj.GetComponent<Rigidbody>().velocity = (obj.transform.forward * 2f);
        }

    }

   void DoDamage()
    {
        for (int i = 0; i < enemiesHit.Length; i++)
        {
            Debug.Log("running");
            HealthScript M_HealthScript = enemiesHit[i].gameObject.GetComponent<HealthScript>();
            M_HealthScript.CurrentHealth -= damage;
        }
    }


    void SlowEnemy()
    {
        //This portion is here as a personal note to self. This is relatively easy I think, but I think I have to make a script to call to in the respective movement controllers
        //I don't think I can effectively do it here
        //The reason I say that is I kind of want to delete this object , and when I do that, this script will disappear, so I need the slow to stay. This is the way I think of it

        for (int i = 0; i < enemiesHit.Length; i++)
        {
            if (enemiesHit[i].gameObject.tag == "PlayerHolder")
            {
                enemiesHit[i].gameObject.GetComponent<PlayerCharacterController>().Slowed(0.65f, 3f);
                //Call to that function in PlayerCharacterController to slow them
                //It would be something like: enemiesHit[i].gameObject.getComponent<PlayerCharacterController>().*functionName*
            }
            else if (enemiesHit[i].gameObject.tag == "Enemy")
            {
                enemiesHit[i].gameObject.GetComponent<Pathfinding>().Slowed(0.65f, 3f);
                //Actually not sure what to call in here. Maybe add a function to pathfinding script that calls to NavMeshAgent and temporarily changes speed.
                //It would be something like: enemiesHit[i].gameObject.getComponent<Pathfinding>().*functionName*
            }
        }

    }
}
