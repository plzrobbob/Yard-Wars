/*
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefAbilityOne : MonoBehaviour
{
    bool canUseAbility;
    public float abilityOneJumpHeight = 6f;
    public float abilityOneDuration = 5f;
    public float abilityOneCooldown = 7f;


    // Start is called before the first frame update
    void Start()
    {
        canUseAbility = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("Ability One") && canUseAbility)
        {
            canUseAbility = false;
            this.GetComponent<PlayerCharacterController>().JumpHeight = abilityOneJumpHeight;

            StartCoroutine(AbilityDuration());
            StartCoroutine(AbilityCooldown());
        }
    }

    IEnumerator AbilityDuration()
    {

        yield return new WaitForSeconds(abilityOneDuration);
        this.GetComponent<PlayerCharacterController>().JumpHeight = 3f;
        //Debug.Log("Ability Duration Coroutine is a gogo");
    }

    IEnumerator AbilityCooldown()
    {

        yield return new WaitForSeconds(abilityOneCooldown);
        canUseAbility = true;
        //Debug.Log("Ability Cooldown Coroutine is a gogogo!!");
    }
}
