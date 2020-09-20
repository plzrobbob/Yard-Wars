using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthScript : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth;

    public float regentimer;
    public float regenmult;
    private float curregentime;

    public bool damaged;

    public GameObject Playerbody;
    public PlayerCharacterController m_PlayerCharacterController;
    public WeaponAim_Fire m_weaponAim_Fire;
    public PlaceDefense m_placeDefense;
    public GameObject PlayerCineCamera;
    public GameObject DeathCamCineCamera;


    public void Update()
    {
        if (CurrentHealth > 0)
        {
            RegenHandler();
        }
        if (CurrentHealth <= 0)
        {
            StartCoroutine(Dead());
        }

        if (Input.GetAxis("Die") != 0)
        {
            CurrentHealth = 0;
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
        curregentime = 0;
    }

    public IEnumerator Dead()
    {
        //play death aniamtion
        Playerbody.SetActive(false);
        m_PlayerCharacterController.enabled = false;
        m_weaponAim_Fire.enabled = false;
        m_placeDefense.enabled = false;
        PlayerCineCamera.SetActive(false);


        yield return new WaitForSeconds(2);
        DeathCamCineCamera.SetActive(false);
        //this is where the player should be transformed to his spawn

        yield return new WaitForSeconds(6);
        //respawn the palyer
        CurrentHealth = 100;
        Playerbody.SetActive(true);
        m_PlayerCharacterController.enabled = true;
        m_weaponAim_Fire.enabled = true;
        m_placeDefense.enabled = true;
        PlayerCineCamera.SetActive(true);
        DeathCamCineCamera.SetActive(true);
    }
}
