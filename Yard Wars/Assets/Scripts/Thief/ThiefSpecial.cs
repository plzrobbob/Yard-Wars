using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSpecial : MonoBehaviour
{
    bool canUseSpecial;
    bool specialIsActive;
    bool stackPrevention;
    public float heelysSpeedTM = 25f;
    public float specialDuration = 20f;
    public float damageIncreaseTime = 1.5f;
    float originalDamageValue;
    public Animator Player_Animator;
    public GameObject Weapon; 


    // Start is called before the first frame update
    void Start()
    {
        canUseSpecial = true;
        stackPrevention = false;

        originalDamageValue = Weapon.GetComponent<MeleeWeaponDamage>().damageAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && canUseSpecial)
        {
            canUseSpecial = false;
            specialIsActive = true;
            this.GetComponent<PlayerCharacterController>().MoveSpeed = heelysSpeedTM;
            StartCoroutine(SpecialDuration());
        }

        if (specialIsActive)
        {
            if (Player_Animator.GetBool("IsWalking") == true && Weapon.GetComponent<MeleeWeaponDamage>().attackSuccessful == true && stackPrevention == false)
            {
                Debug.Log("Damage increased motherfucker!");

                //Actually put the code that increases your damage by 25% for 1.5 seconds
                //It detects each of the successful melee attacks while walking per frame so make sure these can't stack kthnx

                stackPrevention = true;
                StartCoroutine(DamageMultiplier());
            }


        }
    }

    IEnumerator SpecialDuration()
    {
        yield return new WaitForSeconds(specialDuration);
        specialIsActive = false;
        this.GetComponent<PlayerCharacterController>().MoveSpeed = 12f;
        //Debug.Log("Special Duration Coroutine has been deployed, God help us all.");
    }

    IEnumerator DamageMultiplier()
    {
        Weapon.GetComponent<MeleeWeaponDamage>().damageAmount = Weapon.GetComponent<MeleeWeaponDamage>().damageAmount + (Weapon.GetComponent<MeleeWeaponDamage>().damageAmount * 0.25f);
        yield return new WaitForSeconds(damageIncreaseTime);
        Weapon.GetComponent<MeleeWeaponDamage>().damageAmount = originalDamageValue;
        stackPrevention = false;
    }
}
