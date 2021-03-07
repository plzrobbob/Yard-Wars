/* Description of this ability: Massively increase movement speed, as you begin moving on your heelys across the field. When they attack 
 * someone out of this ability, the damage is increased by 25% for 1.5 seconds.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSpecial : MonoBehaviour
{
    [Header("Cooldown")]
    bool canUseSpecial = true; //This bool is true when the player can use this ability
    public float specialCooldown = 6f; //The amount of time (in seconds) of the cooldown between Ultimate usages

    [Header("Buff Variables")]
    bool stackPrevention; //This bool prevents buffs from stacking
    public float damageIncreaseTime = 1.5f; //How long (in seconds) does the buff last?
    float originalDamageValue; //What was the original value of the attack damage? We'll need that to undo the buff

    [Header("Ult Variables")]
    bool specialIsActive = false; //This bool is true while the Ultimate is going on
    public float heelysSpeedTM = 25f; //The speed value of your legally distinct wheeled shoes
    public float specialDuration = 20f; //How long does the speed boost last?

    [Header("Outside Referenced Variables")]
    public Animator Player_Animator; //The animator component of the player character
    public GameObject Weapon; //The player character's melee weapon game object


    // Start is called before the first frame update
    void Start()
    {
        //As soon as the character is spawned we store the amount of damage their weapon deals without buffs
        originalDamageValue = Weapon.GetComponent<MeleeWeaponDamage>().damageAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Ultimate") && canUseSpecial)
        {
            Debug.Log("Thief Ult is fuckin active boi");

            //We can't use the special again until after the cooldown
            canUseSpecial = false;

            //The special is currently going on
            specialIsActive = true;

            //Add the speed buff
            this.GetComponent<PlayerCharacterController>().MoveSpeed = heelysSpeedTM;

            //Start the coroutine that ends when the Ultimate ends
            StartCoroutine(SpecialDuration());
        }

        if (specialIsActive)
        {
            //Once the special is active we add the damage buff
            DamageBuff();
        }
    }

    void DamageBuff()
    {
        if (Player_Animator.GetBool("IsWalking") == true && Weapon.GetComponent<MeleeWeaponDamage>().attackSuccessful == true
                && stackPrevention == false)
        {
            Debug.Log("Damage increased motherfucker!");

            //Increases your damage by 25% for 1.5 seconds
            //It detects each of the successful melee attacks while walking per frame so make sure these can't stack kthnx
            stackPrevention = true;
            StartCoroutine(DamageMultiplier());
        }
    }

    IEnumerator SpecialDuration()
    {
        //Wait the amount of seconds the Ulitmate remains active
        yield return new WaitForSeconds(specialDuration);

        //The special is no longer active
        specialIsActive = false;

        //Return the player's speed to normal
        this.GetComponent<PlayerCharacterController>().MoveSpeed = 12f;


        Debug.Log("Special Duration Coroutine has been deployed, God help us all.");

        //The Ultimate is done so now start the cooldown
        StartCoroutine(SpecialCooldown());
    }

    IEnumerator DamageMultiplier()
    {
        //The damage buff itself
        Weapon.GetComponent<MeleeWeaponDamage>().damageAmount = Weapon.GetComponent<MeleeWeaponDamage>().damageAmount + 
            (Weapon.GetComponent<MeleeWeaponDamage>().damageAmount * 0.25f);

        //Wait the amount of time the buff lasts
        yield return new WaitForSeconds(damageIncreaseTime);

        //After the buff is done, return the damage amount to it's original state
        Weapon.GetComponent<MeleeWeaponDamage>().damageAmount = originalDamageValue;

        //Buff is done we don't need this Stack prevention anymore
        stackPrevention = false;
    }

    IEnumerator SpecialCooldown()
    {
        //Wait for the cooldown amount of time
        yield return new WaitForSeconds(specialCooldown);

        //We can now use the Ultimate again
        canUseSpecial = true;

        Debug.Log("You have regained Thief Ult Priveleges once more");
    }
}
