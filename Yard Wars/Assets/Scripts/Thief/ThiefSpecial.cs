using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSpecial : MonoBehaviour
{
    bool canUseSpecial;
    bool specialIsActive;
    public float heelysSpeedTM = 25f;
    public float specialDuration = 20f;
    public float damageIncreaseTime = 1.5f;
    public Animator Player_Animator;
    public GameObject Weapon;


    // Start is called before the first frame update
    void Start()
    {
        canUseSpecial = true;
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
            if (Player_Animator.GetBool("IsWalking") == true && Weapon.GetComponent<MeleeWeaponDamage>().attackSuccessful == true)
            {
                Debug.Log("Damage increased motherfucker!");

                //Actually put the code that increases your damage by 25% for 1.5 seconds
                //It detects each of the successful melee attacks while walking per frame so make sure these can't stack kthnx
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
}
