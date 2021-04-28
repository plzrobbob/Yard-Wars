using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderAbilityTwo : MonoBehaviour
{

    /// <summary>
    /// So i have two ways to do this. One is the easy way. The easy way is to use Vector.distance to calculate
    /// the other side of the surface, and have the player be slid over there no matter what their controls say they want. That seems relatively simple but I am going interesting
    /// I am thinking of going with a more icy style where they will have some controls but add a noise parameter into the input, simulating them losing their controls, but still having them
    /// 
    /// 
    /// ALRIGHT, so this ability is done. Currently it:
    /// 1. has a ground targeter for when you initially press the button PLACEHOLDER 
    /// 2. When you press the ability again or press the fire button, you then place the ability
    /// 3. It is spawned, which then has the enemies slide across it and do damage every second they are on it
    /// 4. It goes away after 4 seconds, sending the enemy back to normal
    /// 
    /// Current issue is minion related. They are targeting themselves? We might be able to fix this by making only enemy layer things targetable. Look into this.
    /// </summary>

    public bool SlidingDamage;
    public float Damagetimer;
    public float Builder2Damage = 1f;
    public bool ShouldSlide;
    public int Target;

    //public Vector3 SlidingDirection;

    // Start is called before the first frame update
    void Start()
    {
        ShouldSlide = false;
        SlidingDamage = false;
        Damagetimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //  Debug.Log(ShouldSlide);
        //  Debug.Log(SlidingDirection);
        Damagetimer += Time.deltaTime;
    }



    private void OnTriggerEnter(Collider collision)
    {
        //Player section
        ShouldSlide = true;
        if (collision.gameObject.CompareTag("PlayerHolder") && collision.gameObject.layer == Target)
        {
            PlayerCharacterController m_playercharactercontroller = collision.gameObject.GetComponent<PlayerCharacterController>();
            m_playercharactercontroller.SlidingBool = true;
            m_playercharactercontroller.Builder2TurnOff = gameObject;
            HealthScript m_healthscript = collision.gameObject.GetComponent<HealthScript>();
            m_healthscript.CurrentHealth -= 5;
     //       m_healthscript.CurrentHealth -= 1;
        }
        else if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == Target)
        {
            AINodePath m_pathfinding = collision.gameObject.GetComponent<AINodePath>();
            NavMeshAgent m_navmeshagent = collision.gameObject.GetComponent<NavMeshAgent>();
            m_pathfinding.SlidingDirection = m_navmeshagent.velocity;
            m_pathfinding.Builder2TurnOff = gameObject;

            HealthScript m_healthscript = collision.gameObject.GetComponent<HealthScript>();
            m_healthscript.CurrentHealth -=5;
            // m_navmeshagent.velocity = Vector3.zero;
            m_navmeshagent.isStopped = true;
            m_pathfinding.isSliding = true;

        }
        //SlidingDirection = m_playercharactercontroller.tempSlidingDirection;
        //

        //Minion Section... Maybe

        //
    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("Yo we in a damn trigger over heah");
        if (Damagetimer > 1f)
        {
            Damagetimer = 0f;
            if (collision.gameObject.CompareTag("PlayerHolder") && collision.gameObject.layer == Target)
            {
                HealthScript m_healthscript = collision.gameObject.GetComponent<HealthScript>();
                m_healthscript.CurrentHealth -= 1;
                //       m_healthscript.CurrentHealth -= 1;
            }
            else if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer == Target)
            {
                HealthScript m_healthscript = collision.gameObject.GetComponent<HealthScript>();
                m_healthscript.CurrentHealth -= 1;


            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        //Player Section
        ShouldSlide = false;
        if (collision.gameObject.CompareTag("PlayerHolder"))
        {
            PlayerCharacterController m_playercharactercontroller = collision.gameObject.GetComponent<PlayerCharacterController>();
            m_playercharactercontroller.SlidingBool = false;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            AINodePath m_pathfinding = collision.gameObject.GetComponent<AINodePath>();
            NavMeshAgent m_navmeshagent = collision.gameObject.GetComponent<NavMeshAgent>();

            m_navmeshagent.isStopped = false;

            m_pathfinding.isSliding = false;

        }

        //

        //Minion Section... Maybe

        //
    }
}
