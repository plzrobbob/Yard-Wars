using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierBasicHitDetect : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioManager audio_manager;
    private Collider[] HitTargets;
    private Vector3 overlapCheck;
   // public float mainDamage;  
    public float damage;
    public float area;
    public int layernum;
    private LayerMask layer;
    public GameObject PlayerThatFired;

    void Start()
    {


        /// <summary>
        /// Somewhere in another script attached to this there is a cry for help over this situation. This is dumb. Why is this like this? this should be easier.
        /// Please answer the call for help - Cameron
        /// 
        /// So this is a little convoluted, but what I do is on GrenadierStatHandler set an int named 'TeamLayer' in GrenadierBasicAttack to the integer representation
        /// of the LayerMask I want it to be. Example: If player is on Team 1, then 'TeamLayer' will be set to '20' and if it is Team 2, then it is '21'.
        /// 
        /// When the Water Balloon is spawned I call to it and set 'public int layernum' equal to 'TeamLayer'. Now below this, I check what 'layernum' is. If it is 20,
        /// I set 'layer', the LayerMask number we are looking to do damage against, equal to the enemy layer using the .GetMask function in LayerMask. 
        /// 
        /// I tried to make this work where it was in as little different scripts as possible and it.... didn't work.....
        /// *sigh*
        /// </summary>
        Debug.Log(layernum);

        ///<summary>
        ///if players layer is 20, or Team 1, then their target for doing damage is Team 2 or 21
        ///else, if their layer is 21, or Team 2, then their target for doing damage is Team 1 or 20
        /// 
        /// </summary>
        if (layernum == 20)
        {
            layer = LayerMask.GetMask("Team2");
        }
        else if (layernum == 21)
        {
            layer = LayerMask.GetMask("Team1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 

    private void OnCollisionEnter(Collision collision)
    {
        //The reason I have it spawning on collision.contacts[0].point is to try and replicate how a balloon would work
        //When it pops, the water will splash off of the first thign it, so the aoe damage is based on that location

        //FindObjectOfType<AudioManager>().Play("WaterBalloonSplash1", transform.position);

        HitTargets = Physics.OverlapSphere(collision.contacts[0].point, area, layer);
        DoDamage();
        Destroy(gameObject);

    }
    private void DoDamage()
    {

        //This should work against enemy players. Hopefully.
        //Debug.Log("work?");
        for (int  i = 0; i < HitTargets.Length; i++)
        {

            HealthScript M_HealthScript = HitTargets[i].gameObject.GetComponent<HealthScript>();
            if (M_HealthScript != null)
            {
                M_HealthScript.CurrentHealth -= damage;
            }

            GetAgroTarget getAgroTarget = HitTargets[i].gameObject.GetComponent<GetAgroTarget>();
            if (getAgroTarget != null)
            {
                getAgroTarget.PlayerTarget = PlayerThatFired;
            }
        }
    }
}
