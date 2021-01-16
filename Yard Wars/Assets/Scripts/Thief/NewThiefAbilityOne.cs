using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewThiefAbilityOne : MonoBehaviour
{
    public CharacterController CharController;
    public PlayerCharacterController PlayerController;
    public bool canUseAbility = true;
    public float abilityOneCooldown = 7f;

    public float SlideSpeed = 2500f;//controls slide speed
    public float slidedec = 50f;//this value represents the amount of frictional force to slow down the slide
    private float TempSlideSpeed;//this value represents the slide speed that is being decreased every frame by slidedec

    public bool IsSliding;//this bool tells us if the palyer is sliding.  leave this public in case of bugs for later.
    private bool IsFirstSlide;//this bool lets the program know if we should apply inital slide force or not
    private Vector3 slideDIR;
    private Vector3 SlideForce;

    void Start()
    {
        CharController = GetComponent<CharacterController>();
        PlayerController = GetComponent<PlayerCharacterController>();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown("q") && canUseAbility)
        {
            //Should I disable jumping during this ability?  nah fuck it dud
            IsSliding = true;
            IsFirstSlide = true;
            canUseAbility = false;
            StartCoroutine(AbilityCooldown());
        }
        Dash();
    }

    void Dash()
    {
        //Ben's Dash
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (IsSliding)//want to add behavior where slide will keep going and increase if going down an incline
        {
            if (IsFirstSlide)//initial slide speed
            {
                slideDIR = transform.right * x + transform.forward * z;
                TempSlideSpeed = SlideSpeed;
                SlideForce = slideDIR * SlideSpeed * Time.deltaTime;
                IsFirstSlide = false;
            }
            if (TempSlideSpeed > 0)//slowly decreases sliding speed: fake friction
            {
                TempSlideSpeed = TempSlideSpeed - slidedec;
                SlideForce = slideDIR * TempSlideSpeed * Time.deltaTime;
            }
            else if (PlayerController.Grounded)
            {
                SlideForce.y = 0;
            }
            CharController.Move(SlideForce * Time.deltaTime);

            if (TempSlideSpeed <= 0)
            {
                IsSliding = false;
            }
        }


        /* 
        //Christian's dash
        canUseAbility = false;

        Vector3 move = transform.forward;
        move = Vector3.ClampMagnitude(move, 1f);
        CharController.Move(move * dashForce * Time.deltaTime);

        Debug.Log("Activate dash motherfucker");
        Debug.Log(transform.forward);
        Debug.Log(dashForce);

        StartCoroutine(AbilityCooldown());
        */
    }

    IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(abilityOneCooldown);
        canUseAbility = true;
        Debug.Log("Ability Cooldown Coroutine is a gogogo!!");
    }

}
