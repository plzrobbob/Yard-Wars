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
    /// </summary>


    public bool ShouldSlide;
    //public Vector3 SlidingDirection;

    // Start is called before the first frame update
    void Start()
    {
        ShouldSlide = false;
    }

    // Update is called once per frame
    void Update()
    {
      //  Debug.Log(ShouldSlide);
      //  Debug.Log(SlidingDirection);

    }
    
    private void OnTriggerEnter(Collider collision)
    {
        //Player section
        ShouldSlide = true;
        if (collision.gameObject.CompareTag("PlayerHolder"))
        {
            PlayerCharacterController m_playercharactercontroller = collision.gameObject.GetComponent<PlayerCharacterController>();
            m_playercharactercontroller.SlidingBool = true;
     //       HealthScript m_healthscript = collision.gameObject.GetComponent<HealthScript>();
     //       m_healthscript.CurrentHealth -= 1;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Pathfinding m_pathfinding = collision.gameObject.GetComponent<Pathfinding>();
            NavMeshAgent m_navmeshagent = collision.gameObject.GetComponent<NavMeshAgent>();
            m_pathfinding.SlidingDirection = m_navmeshagent.velocity;
           // m_navmeshagent.velocity = Vector3.zero;
            m_navmeshagent.isStopped = true;
            m_pathfinding.isSliding = true;

        }
        //SlidingDirection = m_playercharactercontroller.tempSlidingDirection;
        //

        //Minion Section... Maybe

        //
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
            Pathfinding m_pathfinding = collision.gameObject.GetComponent<Pathfinding>();
            NavMeshAgent m_navmeshagent = collision.gameObject.GetComponent<NavMeshAgent>();

            m_navmeshagent.isStopped = false;

            m_pathfinding.isSliding = false;

        }

        //

        //Minion Section... Maybe

        //
    }
}
