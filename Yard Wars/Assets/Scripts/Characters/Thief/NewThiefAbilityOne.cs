/* Description of this ability: Dash in whatever direction the thief is facing.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewThiefAbilityOne : MonoBehaviour
{
    [Header("Cooldown")]
    public bool canUseAbility = true; //This bool is true whenever this ability can be used
    public float abilityOneCooldown = 7f; //The length of time (in seconds) the cooldown of this ability lasts
    bool inCooldown = false;

    [Header("Player Variables")]
    public CharacterController CharController; //The character controller for our player character as a variable to reference
    public PlayerCharacterController PlayerController; //The player character controller for our player character as a variable to reference

    [Header("Sliding Variables")]
    public float SlideSpeed = 2500f;//controls slide speed
    public float slidedec = 50f;//this value represents the amount of frictional force to slow down the slide
    private float TempSlideSpeed;//this value represents the slide speed that is being decreased every frame by slidedec

    public bool IsSliding;//this bool tells us if the palyer is sliding.  leave this public in case of bugs for later.
    private bool IsFirstSlide;//this bool lets the program know if we should apply inital slide force or not
    private Vector3 slideDIR; //The direction the character will slide in when using this ability
    private Vector3 SlideForce; //The amount of force applied when the character slides using this ability

    void Start()
    {
        //Declare our outside variables on startup:
        CharController = GetComponent<CharacterController>(); 
        PlayerController = GetComponent<PlayerCharacterController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Ability One") && canUseAbility)
        {
            //We can't use the ability again while we're currently using it
            canUseAbility = false;

            //We are currently in the middle of sliding
            IsSliding = true;

            //Is this the first slide we doin? I don't think we can chain slides like Vanquish so uh, guess the answer is always yes? I think?
            IsFirstSlide = true;
        }

        //If we are done sliding we want to run the coroutine for the cooldown
        if (IsSliding == false && canUseAbility == false && inCooldown == false)
        {
            StartCoroutine(AbilityCooldown());
        }
    }

    void FixedUpdate()
    {
        //Since this has to do with physics movement we run the actual dashing part of the ability in FixedUpdate
        Dash();
    }

    //This dash right here is my favorite dash. Ever! In the history of forever. I think about this dash every day. I think about this dash all 
    //night long. I stay awake at night not sleeping because I'm thinking about this dash!
    void Dash()
    {
        //Because this is Ben's dash I don't feel right commenting it. It just feels... dirty to be doing that to another man's code without
        //his permission.

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


        //Ew a busted old dash that teleports you around. It's fucking unsightly. This isn't a fucking dash, it's a Shunpo, a fucking goddamn
        //Sonido, if you're a fucking dub only pleb you might even call this piece of shit a Flash Step. We're trying to fucking dash here
        //you fucking moron, not Kawarimi no Jutsu around like a bunch of fuckin genin. Christ almighty the sheer fucking hubris of the knuckle
        //scraping mongoloid who programmed this dash, who does he think we're playing as in this children's game, Minato Namikaze? Thinking
        //kids are running around fucking Hiraishin no Jutsuing around like they own the place? Get a fucking grip on reality, goddamn.
        //
        //
        //                                                                                       - Christian (The guy who made this shitty dash)

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
        //We are in a cooldown right now so the bool should reflect that
        inCooldown = true;

        //Wait for the cooldown time
        yield return new WaitForSeconds(abilityOneCooldown);

        //Flip that bool to let us use the ability again
        canUseAbility = true;

        //We are no longer in a cooldown
        inCooldown = false;

        Debug.Log("Ability Cooldown Coroutine is a gogogo!! And by that I mean it is done!!!!!!!");
    }

}
