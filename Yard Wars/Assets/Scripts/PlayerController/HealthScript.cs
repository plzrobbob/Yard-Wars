using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthScript : MonoBehaviour
{
    public GameObject[] RespawnBoundries;

    public float MaxHealth;
    public float CurrentHealth;

    public float regentimer;
    public float regenmult;
    private float curregentime;
    private IEnumerator dotDamage;
    private bool DottyDamage = false;

    public bool damaged;

    public GameObject Playerbody;
    public PlayerCharacterController m_PlayerCharacterController;
    public WeaponAim_Fire m_weaponAim_Fire;
    public PlaceDefense m_placeDefense;
    public GameObject PlayerCineCamera;
    public GameObject DeathCamCineCamera;
    public PlayerResourceSystem player_resources;
    public float death_value;
    public Animator Player_Animator;

    private bool Isdead;

    public int respawnTimer;



    public GameObject TopBody;
    void Start()
    {
        CurrentHealth = MaxHealth;
        if (TopBody.gameObject.CompareTag("Enemy"))
        {
            GameObject tempPlayer = GameObject.FindGameObjectWithTag("PlayerHolder");
            player_resources = tempPlayer.GetComponent<PlayerResourceSystem>();
        }
    }

    public void Update()
    {

        if (CurrentHealth > 0 && TopBody.gameObject.tag == "PlayerHolder")
        {
            RegenHandler();
        }
        if (CurrentHealth <= 0 && !Isdead)
        {
            Isdead = true;
            StartCoroutine(Dead());
        }

        if (Input.GetAxis("Die") != 0 && TopBody.gameObject.tag == "PlayerHolder")
        {
            Debug.Log("dead");
            CurrentHealth = 0;
        }
        else if (DottyDamage)
        {
            StartCoroutine(SpecialDuration());
            DottyDamage = false;
        }
    }

    public void RegenHandler()
    {
        if (damaged && curregentime < regentimer)
        {
            curregentime += Time.deltaTime;
        }

        else if (curregentime >= regentimer)
        {
            damaged = false;
        }

        if (!damaged && curregentime >= regentimer && CurrentHealth < MaxHealth)
        {
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth += regenmult * Time.deltaTime;
            }

            if (CurrentHealth >= MaxHealth)
            {
                CurrentHealth = MaxHealth;
                curregentime = 0;
            }
        }
    }
    public void DamageHandler()
    {
        damaged = true;
        Debug.Log("Current  Health = " + CurrentHealth);
        curregentime = 0;
    }

    public IEnumerator Dead()
    {
        if (!TopBody.gameObject.CompareTag("PlayerHolder"))//destroy ai and defenses
        {
            if (TopBody.gameObject.CompareTag("Enemy"))
            {
                player_resources.Gain(death_value);
            }
            yield return new WaitForSeconds(1);
            Destroy(TopBody);
        }
        if (TopBody.gameObject.CompareTag("PlayerHolder"))//respawn the player
        {

            m_PlayerCharacterController.enabled = false;//player is dead play animation and remove controlls
          //  m_weaponAim_Fire.enabled = false;
            m_placeDefense.enabled = false;
            PlayerCineCamera.SetActive(false);
            Player_Animator.SetBool("IsDead", true);
            Player_Animator.SetLayerWeight(1, 0);
            Player_Animator.SetLayerWeight(2, 0);
            yield return new WaitForSeconds(1);

            yield return new WaitForSeconds(2);//camera transition
            Playerbody.SetActive(false);
            DeathCamCineCamera.SetActive(false);
            //this is where the player should be transformed to his spawn

            yield return new WaitForSeconds(respawnTimer);//reset player values and renable cameras to begin transition after specified respawn timer
            CurrentHealth = MaxHealth;
            Playerbody.SetActive(true);

            Player_Animator.SetBool("IsDead", false);
            Player_Animator.SetLayerWeight(1, 1);
            Player_Animator.SetLayerWeight(2, 1);

            PlayerCineCamera.SetActive(true);
            DeathCamCineCamera.SetActive(true);
            Respawn();

            yield return new WaitForSeconds(1);//give controlls back to player
            m_PlayerCharacterController.enabled = true;
        //    m_weaponAim_Fire.enabled = true;
            m_placeDefense.enabled = true;
            Isdead = false;
        }
    }



    public void Respawn()
    {
        //RespawnBoundries

        Vector3 destination = new Vector3(Random.Range(RespawnBoundries[0].transform.position.x, RespawnBoundries[1].transform.position.x), RespawnBoundries[0].transform.position.y + m_PlayerCharacterController.CharController.height / 2, Random.Range(RespawnBoundries[0].transform.position.z, RespawnBoundries[2].transform.position.z));
        TopBody.transform.position = destination;
    }
    IEnumerator SpecialDuration()
    {
        int i;
        Debug.Log("Hey The Coroutine in the healthscript is turned on");
        for (i = 0; i < 4; i++)   // for three seconds, 0 > 1 > 2 > 3 but not 4
        {
            CurrentHealth -= 1; //take damage BITCH
            Debug.Log("Health of the Minion: " + CurrentHealth); //so I can show off
            //Debug.Log(i);
            yield return new WaitForSeconds(1f); // waits the specified timeframe
        }
    }
}
