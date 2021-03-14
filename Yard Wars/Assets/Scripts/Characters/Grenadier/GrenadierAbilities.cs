using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.VFX;

public class GrenadierAbilities : MonoBehaviour
{
    /// <summary>
    /// TODO:
    /// --- I need to try and create a spawning over time for ability one. I envision the animation look like they are spinning around and throwing them. Something quick.
    /// I am not quite sure how to go about this. perhaps Coroutines? time to do more research
    /// 
    /// --- Ultimate is going to be a little confusing for me. I know how I want to do it but I might need some help because I am not sure how I could do it. It is going to 
    /// be complicated to me. If I get anyones help I strongly recommend looking at this clip: https://www.youtube.com/watch?v=yU39P75srsA
    /// Look at how the targeting works for sylvanus. That is how I want to do it. My question is how? associate a plane with it and allow the mouse controls to control and 
    /// max the distance it can be to the player? There has to be a different way to do it. Maybe there isn't.
    /// 
    /// --- I am going to get Ability two to work, but I don't think it is actually that good of an idea. I think we should play with it and decide if it is something we want or not
    /// 
    /// </summary>
 


        // All of the variable names are pretty self explanatory in their position, may go back and explain each one but their names speak for themselves
        //NO WEIRD VARIABLE NAMES HERE BAYBEE

    //So this bool right here is to lock down the OnUpdate so that someone cann't be spamming all of the abilities. i will be implementing it later on
    //I am placing this here as a reminder that this will need to be added, or a way that you can't activate multiple abilities at once
    public bool AbilityInUse;

    public Mesh mesh;

    //These are the variables I am using for ability one
    //Damage numbers come from GrenadierStatHandler

    public float AbilityOneRange;
    public float AbilityOneDamage;
    public float AbilityOnecooldown;
    public int AbilityOneBalloonAmount;

    private float spawnRad;
    private Vector3 SpawnDirAbilityOne;
    public Transform SpawnheightAbilityOne;
    public LayerMask layer;
    private Collider[] EnemiesDamaged;

    public GameObject BalloonPrefab;


    //These are the variables I am using for Ability Two
    //Damage numbers come from GrenadierStatHandler

    public float AbilityTwoDamage;
    public float AbilityTwoCooldown;
    public float AbilityTwoDamageRange;
    public GameObject TripWire;
    public Transform TripwireSpawnLocation;
    private bool placing;
    public bool canplace;
    private bool deactivate;
    public float newrot;
    public LayerMask raymask;

    public GameObject AbilitytwoSpawn;
    public GameObject AbilityTwoBola;
    [Range(5.0f, 25.0f)]
    public float AbilityTwoSpeed;
    public float AbilityTwoSlowSpeed;
    public float AbilityTwoDistance;



    //These are the variables I am using for Ultimate
    public float UltimateCooldown;
    public bool UltimatePressed;
    public GameObject PlayerCam;
    public GameObject UltiCam;
    public GameObject reticle;
    public GameObject ReticleController;
    public GameObject Cam;
    public Vector3 DACLAMPA;
    public LayerMask UltiRaymask;
    public VisualEffect effect;
    public bool UltiFired = false;
    public bool inProgress = false;
    public Vector3 UltiAreaDamage;
    private Collider[] damaging;
    public LayerMask Target;
    public float UltiDamageNum;
    public Animator Player_Animator;
    public GrenadierBasicAttack GrenBasic;
    public GameObject UltWeapon;
    bool VisibleUltWeapon;

    public GameObject UltiBalloon;
   // public CinemachineFreeLook brian;

    // Start is called before the first frame update
    void Start()
    {
        effect = reticle.GetComponent<VisualEffect>();
        
        spawnRad = 1f;
        placing = false;
        AbilityOnecooldown = 5f;
        AbilityTwoCooldown = 10f;
        UltimateCooldown = 60f;
        UltimatePressed = false;
       // DasBrain = this.gameObject.GetComponentInChildren<CinemachineFreeLook>();
       // PlayerView = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        //I HAVE A QUESTION FOR THE CREW. HOW DO I TURN IT OFF AND DISABLE IT IF LITERALLY ANY OTHER INPUT THAN THE ONES I AM LOOKING FOR ARE DONE?

        //OnUpdate Section for Ability One
        AbilityOnecooldown += Time.deltaTime;

        //Ability One is an AOE attack. DoDamageAbilityOne() is in charge of dealing the damage, the AbilityOneSpawn will be the actual visual representation
        //of the ability, and at a later date will be linked with animations, meaning this may not be it's permanent home
        if (Input.GetButtonDown("Ability One") && AbilityOnecooldown > 5 && !UltimatePressed)
        {
            EnemiesDamaged = Physics.OverlapSphere(transform.position, AbilityOneRange, layer);
            AbilityOneSpawn();
            DoDamageAbilityOne();
            Debug.Log(EnemiesDamaged.Length);
            Debug.Log("AbilityOneInitiated");
            Player_Animator.SetBool("Ability1", true);
            AbilityOnecooldown = 0f;
            GrenBasic.canattack = false;
        }



        //OnUpdate section for Ability Two
        AbilityTwoCooldown += Time.deltaTime;

        //Ability Two is a trap placement. There will be a trigger on the tripwire trap, which will spawn a small water balloon and drop it on the location of the trap,
        //Dealing damage and slowing enemies. That portion will be handled by AbilityTwoDamage.cs
        //Here we will control the rotating and placing of the ability.
        if (placing)
        {
          //  RotateDefense();
        //    SetDefenseHeight();
        //    DoAbilityTwo();
        }

        if (Input.GetButtonDown("Ability Two") && AbilityTwoCooldown > 10 && !placing && !UltimatePressed)
        {
            Debug.Log("Ability Two initiated");
         //   placing = true;
            
      //      deactivate = true;
       //     TripWire.SetActive(true);
        }

        if (Input.GetButtonDown("Ability Two") && AbilityTwoCooldown > 10 && !UltimatePressed)
        {
            AbilityTwoV2();

            //Invoke("AbilityTwoV2", 1f);
            Player_Animator.SetBool("Ability2", true);
            GrenBasic.canattack = false;
            //   AbilityTwoCooldown = 0f;
        }




        //Onupdate section for the ultimate ability
        // A majority of what the Ultimate ability will need is under DoUltimate(). I will not repeat it here until necessary
        UltimateCooldown += Time.deltaTime;
        if (UltimatePressed)
        {
           // Debug.Log("lOOp");

            DoUltimate();
        }
        if (Input.GetButtonDown("Ultimate") && UltimateCooldown > 60 && !UltimatePressed)
        {
            Debug.Log("Ultimate initiated");
            UltimatePressed = true;
            this.gameObject.GetComponent<PlayerCharacterController>().enabled = false;

            PlayerCam.SetActive(false);
            UltiCam.SetActive(true);
            //UltiCam.transform.rotation = PlayerCam.transform.rotation;

            GameObject obj = Instantiate(reticle, gameObject.transform.position, reticle.transform.rotation);
            ReticleController = obj;
            Player_Animator.SetBool("Ability3", true);
            GrenBasic.canattack = false;
        }

        if (VisibleUltWeapon && !UltWeapon.activeInHierarchy)
        {
            UltWeapon.SetActive(true);
            Player_Animator.SetLayerWeight(1, 0);
        }
        else if (!VisibleUltWeapon && UltWeapon.activeInHierarchy)
        {
            UltWeapon.SetActive(false);
            Invoke("reacLayer", .5f);
        }
    }
    void reacLayer()
    {
        Player_Animator.SetLayerWeight(1, 1);
    }

    //Useful Debug tool I added. remove when Grenadier in final stage.

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(UltiAreaDamage, AbilityOneRange);
        Gizmos.DrawMesh(mesh, -1, transform.position, Quaternion.identity, new Vector3(1, 1, 1));

    }
    
    //FUNCTIONS ASSOCIATED WITH ABILITY 1:

    void DoDamageAbilityOne()
    {

        //This is a super simple script. It takes the enemies detected from the Physics sphere and does damage to them.
        for (int i = 0; i < EnemiesDamaged.Length; i++)
        {
            Debug.Log("running");
                HealthScript M_HealthScript = EnemiesDamaged[i].gameObject.GetComponent<HealthScript>();
                M_HealthScript.CurrentHealth -= AbilityOneDamage;
        }
    }

    void AbilityOneSpawn()
    {
        for (int i = 0; i < AbilityOneBalloonAmount; i++)
        {
            //This is going to get the point around the circle that we need in order to spawn it at that particular section bla bla bla, unit circle shit
            var radian = 2 * Mathf.PI / AbilityOneBalloonAmount * i;

            var vert = Mathf.Sin(radian);
            var Horiz = Mathf.Cos(radian);
            //Since Unity doesn't work in radians, I need to convert into something that does work
            //SpawnDirAbilityOne is a vector3 that holds the converted value from the radian

            SpawnDirAbilityOne = new Vector3(Horiz, 0, vert);

            var spawnPos = this.transform.position + SpawnDirAbilityOne * spawnRad;

            //I am adding the height right here so that it spawns at a height I want it to.
            //TBH there is a better way to do this but this is all monke brain could come up with. Time to return to monke.
            spawnPos.y = SpawnheightAbilityOne.position.y;

            GameObject obj = Instantiate(BalloonPrefab, spawnPos, Quaternion.identity);

            //This rotates the spawned object to look AWAY from the player, this way I can add velocity and fire it in the OPPOSITE direction of the player
            obj.transform.rotation = Quaternion.LookRotation(obj.transform.position - this.transform.position);

            //Current Exit velocity looks weird. it works, but I am going to figure out if it needs to be faster or not
            obj.GetComponent<Rigidbody>().velocity = (obj.transform.forward * 4f);

        }
        Invoke("Ability1False", .2f);
    }


    //FUNCTIONS ASSOCIATED WITH ABILITY 2:

    void DoAbilityTwo()
    {
        if (Input.GetButtonDown("Ability Two"))
        {
            GameObject obj = Instantiate(TripWire, TripWire.transform.position, TripWire.transform.rotation);
            obj.GetComponentInChildren<GrenadierAbilitytwodamage>().damage = AbilityTwoDamage;
            obj.GetComponentInChildren<GrenadierAbilitytwodamage>().range = AbilityTwoDamageRange;
            obj.GetComponentInChildren<GrenadierAbilitytwodamage>().EnemyMask = Target;

            if (gameObject.layer == 20)
            {
                obj.GetComponentInChildren<GrenadierAbilitytwodamage>().numberLETSFUCKINGGOOOOOOBOYYYYSSSSS = 21;

            }
            else if (gameObject.layer == 21)
            {
                obj.GetComponentInChildren<GrenadierAbilitytwodamage>().numberLETSFUCKINGGOOOOOOBOYYYYSSSSS = 20;
            }


            AbilityTwoCooldown = 0f; //This will be removed here and instead initiate when the opject is actually placed
            placing = false;
            TripWire.SetActive(false);
            

        }
    }

    void AbilityTwoV2 ()
    {
        GameObject obj = Instantiate(AbilityTwoBola, AbilitytwoSpawn.transform.position, AbilitytwoSpawn.transform.rotation);
        obj.gameObject.GetComponent<Rigidbody>().velocity = AbilitytwoSpawn.transform.forward * AbilityTwoSpeed;
        obj.gameObject.GetComponent<GrenadierAbilityTwoBola>().damage = AbilityTwoDamage;
        obj.gameObject.GetComponent<GrenadierAbilityTwoBola>().slowTime = AbilityTwoSlowSpeed;
        obj.gameObject.GetComponent<GrenadierAbilityTwoBola>().distanceAllowed = AbilityTwoDistance;

        if (gameObject.layer == 20)
        {
            obj.GetComponentInChildren<GrenadierAbilityTwoBola>().targetNum = 21;

        }
        else if (gameObject.layer == 21)
        {
            obj.GetComponentInChildren<GrenadierAbilityTwoBola>().targetNum = 20;
        }
        Invoke("Ability2False", .2f);

    }

    private void RotateDefense()
    {
        if (Input.GetAxis("RotateDefenseCClockwise") != 0)
        {
            newrot += 70f * Time.deltaTime;
        }
        if (Input.GetAxis("RotateDefenseClockwise") != 0)
        {
            newrot -= 70f * Time.deltaTime;
        }

        if (newrot <= -360 || newrot >= 360)
        {
            Debug.Log("Reset rotate");
            newrot = 0;
        }
        TripWire.transform.rotation = Quaternion.Euler(0, newrot, 0);
    }

    private void SetDefenseHeight()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 5.5f, raymask))//placing against a wall.  defenses will not intersect
        {
            Debug.DrawRay(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up * 20, Color.blue);
            if (Physics.Raycast(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up, out var hit2, 20f, raymask))
            {
                if (hit2.collider.gameObject.layer == 12)
                {
                    canplace = false;
                    return;
                }
                else
                {
                    TripWire.transform.position = hit2.point + (transform.up * 1.8f);
                }
            }
            else
            {
                canplace = false;
                return;
            }
        }
        else//if there is no wall player can just place
        {
            Debug.DrawRay(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up * 20, Color.blue);
            if (Physics.Raycast(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up, out var hit2, 20f, raymask))
            {
                if (hit2.collider.gameObject.layer == 12)
                {
                    canplace = false;
                    return;
                }
                else
                {
                    TripWire.transform.position = hit2.point + (transform.up * 1.8f);
                }
            }
            else
            {
                canplace = false;
            }
        }

        //if hit.point exists and it isnt more than 1.5 meters from the palyer then you can palce a defense
        if (Vector3.Distance(this.transform.position, hit.point) < 1.5f)
        {
            canplace = false;
            return;
        }
        else
        {
            canplace = true;
            return;
        }

    }


    //FUNCTIONS ASSOCIATED WITH ULTIMATE:

    //Ultimate Abilty Try and raytrace to the set distance
    //I don't want to get rid of any comments quite yet but above comment is fucking useless. Look in DoUltimate() or top of script for updated thinking


    void DoUltimate()
    {
       // Debug.DrawRay(Cam.transform.position, Cam.transform.forward *2000000, Color.red);
        Physics.Raycast(Cam.transform.position, Cam.transform.forward, out var hit, 75f, UltiRaymask);

        if (hit.point != new Vector3(0,0,0))
        {
            DACLAMPA = hit.point;
        }
        else
        {

        }

        if (Vector3.Distance(transform.position, ReticleController.transform.position) > 50f)
        {

        }
        else
        {

        }

        ReticleController.transform.position = Vector3.Lerp(ReticleController.transform.position, DACLAMPA, 1f);
        //This section of the code is the part that launches the player out of its current control loop. This will give the player control again and throw it out of its current loop
        if (Input.GetButtonDown("Ultimate") || Input.GetButtonDown("Fire1"))
        {
            Invoke("Ability3False", .1f);

            Player_Animator.SetBool("Ability3Fire", true);
            Debug.Log("Jump out of lOOp");
            gameObject.GetComponent<PlayerCharacterController>().enabled = true;
            UltimateCooldown = 0f;
            UltimatePressed = false;
            UltiCam.SetActive(false);
            PlayerCam.SetActive(true);
            PlayerCam.transform.rotation = UltiCam.transform.rotation;

            

            ReticleController.GetComponent<MeshRenderer>().enabled = false;
            Destroy(ReticleController, 2.5f);
            UltiAreaDamage = ReticleController.gameObject.transform.position;

            fireUlti();
            UltiFired = true;
        }
    }
    void fireUlti()
    {
        GameObject obj = Instantiate(UltiBalloon, transform.position, Quaternion.identity);
        obj.gameObject.GetComponent<Rigidbody>().velocity = HitTargetAtTime(obj.transform.position, ReticleController.transform.position, new Vector3(0f, -9.81f, 0f), 2.5f);
        //HitTargetByAngle(obj.transform.position, ReticleController.transform.position, new Vector3(0f, -9.81f, 0f), 60f)
        Destroy(obj, 2.5f);
        Invoke("UltiDamage", 2.5f);
        Invoke("Ability3False", .2f);
    }
    void UltiDamage()
    {
        damaging = Physics.OverlapSphere(UltiAreaDamage, 3.0f, Target);
        for (int i = 0; i < damaging.Length; i++)
        {
            damaging[i].gameObject.GetComponent<Pathfinding>().Stunned(2.0f);
            HealthScript M_HealthScript = damaging[i].gameObject.GetComponent<HealthScript>();
            M_HealthScript.CurrentHealth -= UltiDamageNum;
            Debug.Log("UltiDamage");
        }
    }
    public static Vector3 HitTargetAtTime(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float timeToTarget)
    {
        Vector3 AtoB = targetPosition - startPosition;
        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase);
        float horizontalDistance = horizontal.magnitude;
        Vector3 vertical = GetVerticalVector(AtoB, gravityBase);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase));

        float horizontalSpeed = horizontalDistance / timeToTarget;
        float verticalSpeed = (verticalDistance + ((0.5f * gravityBase.magnitude) * (timeToTarget * timeToTarget))) / timeToTarget;

        Vector3 launch = (horizontal.normalized * horizontalSpeed) - (gravityBase.normalized * verticalSpeed);
        return launch;
    }


    public static Vector3 HitTargetByAngle(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float limitAngle)
    {
        if (limitAngle == 90 || limitAngle == -90)
        {
            return Vector3.zero;
        }

        Vector3 AtoB = targetPosition - startPosition;
        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase);
        float horizontalDistance = horizontal.magnitude;
        Vector3 vertical = GetVerticalVector(AtoB, gravityBase);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase));

        float angleX = Mathf.Cos(Mathf.Deg2Rad * limitAngle);
        float angleY = Mathf.Sin(Mathf.Deg2Rad * limitAngle);

        float gravityMag = gravityBase.magnitude;

        if (verticalDistance / horizontalDistance > angleY / angleX)
        {
            return Vector3.zero;
        }

        float destSpeed = (1 / Mathf.Cos(Mathf.Deg2Rad * limitAngle)) * Mathf.Sqrt((0.5f * gravityMag * horizontalDistance * horizontalDistance) / ((horizontalDistance * Mathf.Tan(Mathf.Deg2Rad * limitAngle)) - verticalDistance));
        Vector3 launch = ((horizontal.normalized * angleX) - (gravityBase.normalized * angleY)) * destSpeed;
        return launch;
    }



    public static Vector3[] HitTargetBySpeed(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float launchSpeed)
    {
        Vector3 AtoB = targetPosition - startPosition;
        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase);
        float horizontalDistance = horizontal.magnitude;
        Vector3 vertical = GetVerticalVector(AtoB, gravityBase);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravityBase));

        float x2 = horizontalDistance * horizontalDistance;
        float v2 = launchSpeed * launchSpeed;
        float v4 = launchSpeed * launchSpeed * launchSpeed * launchSpeed;

        float gravMag = gravityBase.magnitude;

        float launchTest = v4 - (gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)));

        Vector3[] launch = new Vector3[2];

        if (launchTest < 0)
        {
            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad)) - (gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad));
            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad)) - (gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad));
        }
        else
        {
            float[] tanAngle = new float[2];
            tanAngle[0] = (v2 - Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);
            tanAngle[1] = (v2 + Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);

            float[] finalAngle = new float[2];
            finalAngle[0] = Mathf.Atan(tanAngle[0]);
            finalAngle[1] = Mathf.Atan(tanAngle[1]);
            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[0])) - (gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[0]));
            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[1])) - (gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[1]));
        }

        return launch;
    }

    public static Vector3 GetHorizontalVector(Vector3 AtoB, Vector3 gravityBase)
    {
        Vector3 output;
        Vector3 perpendicular = Vector3.Cross(AtoB, gravityBase);
        perpendicular = Vector3.Cross(gravityBase, perpendicular);
        output = Vector3.Project(AtoB, perpendicular);
        return output;
    }

    public static Vector3 GetVerticalVector(Vector3 AtoB, Vector3 gravityBase)
    {
        Vector3 output;
        output = Vector3.Project(AtoB, gravityBase);
        return output;
    }

    private void Ability1False()
    {
        Player_Animator.SetBool("Ability1", false);
        Invoke("GrenBasicCanFire", .2f);
    }
    private void Ability2False()
    {
        Player_Animator.SetBool("Ability2", false);
        Invoke("GrenBasicCanFire", .2f);
    }
    private void Ability3False()
    {
        Player_Animator.SetBool("Ability3", false);
        Invoke("GrenBasicCanFire", .2f);
    }
    private void GrenBasicCanFire()
    {
        GrenBasic.canattack = true;
    }
    public void enableVisibleUltimate()
    {
        VisibleUltWeapon = true;
    }
    public void DisableVisibleUltimate()
    {
        VisibleUltWeapon = false;
    }
}
