using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewThiefAbilityOne : MonoBehaviour
{
    public CharacterController CharController;
    public float dashForce = 15f;
    private bool canUseAbility = true;
    public float abilityOneCooldown = 7f;

    // Start is called before the first frame update
    void Start()
    {
        CharController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q") && canUseAbility)
        {
            //Should I disable jumping during this ability?
            Dash();
        }
    }

    void Dash()
    {
        canUseAbility = false;

        Vector3 move = transform.forward;
        move = Vector3.ClampMagnitude(move, 1f);
        CharController.Move(move * dashForce * Time.deltaTime);

        Debug.Log("Activate dash motherfucker");
        Debug.Log(transform.forward);
        Debug.Log(dashForce);

        StartCoroutine(AbilityCooldown());
    }

    IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(abilityOneCooldown);
        canUseAbility = true;
        Debug.Log("Ability Cooldown Coroutine is a gogogo!!");
    }

}
