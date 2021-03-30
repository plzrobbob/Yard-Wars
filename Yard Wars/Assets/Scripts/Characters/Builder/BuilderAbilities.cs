using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderAbilities : MonoBehaviour
{

    //So what do i need? 
    //I need some sort of targeter for the ground effect. Seems easy
    /// <summary>
    /// so it needs to link with the existing character targeting
    /// Lock left right and link it to camera, allow it to go out to certain distance?
    /// </summary>
    //spawn the area where this will happen
    //Do damage every second
    //Code done
    public GameObject AbilityOneObject;
    public GameObject AbilityOneWall;
    private GameObject slider;
    private GameObject wall;
    public GameObject SliderOrigin;
    private Terrain terrainCopy;
    public float AbilityOneCooldown;
    public float AbilityOneMaxCooldown;
    public float AbilityOneWallDuration;
    public bool AbilityOneIsSliding;
    public float AbilityOneMaxRange;
    public float AbilityOneSlidingSpeed;
    private Vector3 slidingStartPos;
    private Vector3 projectilePos;

    public GameObject AbilityTwoObject;
    public float AbilityTwoCooldown;
    public float AbilityTwoMaxCooldown;
    public bool Ability2Targeting;
    public Camera Cam;
    public GameObject ReticleController;
    public Vector3 DACLAMPA;
    public GameObject reticle;
    public int Target;


    void Start()
    {
        AbilityOneIsSliding = false;
        AbilityOneCooldown = AbilityOneMaxCooldown;
        Ability2Targeting = false;
        AbilityTwoCooldown = AbilityTwoMaxCooldown;
        terrainCopy = Terrain.FindObjectOfType<Terrain>();
    }

    // Update is called once per frame
    void Update()
    {
        AbilityOneHandler();
        AbilityTwoHandler();
    }

    void AbilityOneHandler()
    {
        // Ability one is cosidered "rolling" if the Ability One key has been pressed once. 
        // The player sends a projectile sliding across the ground, which is when AbilityOneIsSliding = true.
        // If the player presses the Ability One key a second time, the projectile turns into a wall.
        if (Input.GetButtonDown("Ability One") && AbilityOneCooldown >= AbilityOneMaxCooldown && !AbilityOneIsSliding)
        {
            // shoot out sliding projectile
            slidingStartPos = SliderOrigin.transform.position;

            slider = Instantiate(AbilityOneObject, new Vector3(SliderOrigin.transform.position.x, terrainCopy.SampleHeight(SliderOrigin.transform.position), SliderOrigin.transform.position.z), SliderOrigin.transform.rotation);

            Rigidbody rbcopy = slider.GetComponent<Rigidbody>();

            // if this breaks everything in the future, I'm sorry.
            // - Stephen
            rbcopy.velocity = Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up)) * AbilityOneSlidingSpeed;
            rbcopy.detectCollisions = false;

            // sticks the projectile to the ground
            slider.transform.position = new Vector3(rbcopy.position.x, terrainCopy.SampleHeight(rbcopy.position), rbcopy.position.z);
            rbcopy.constraints = RigidbodyConstraints.FreezePositionY;

            AbilityOneIsSliding = true;
        }

        // if the player presses the key again, OR if the projectile hits the max range, turn into a wall
        else if ((Input.GetButtonDown("Ability One") && AbilityOneCooldown >= AbilityOneMaxCooldown && AbilityOneIsSliding) ||
                 (AbilityOneIsSliding && Vector3.Distance(slidingStartPos, slider.transform.position) >= AbilityOneMaxRange))
        {
            // create wall

            wall = Instantiate(AbilityOneWall, slider.transform.position, slider.transform.rotation);
            GameObject.Destroy(wall, AbilityOneWallDuration);
            wall.transform.position = new Vector3(wall.transform.position.x, slider.transform.position.y + wall.GetComponent<BoxCollider>().size.y, wall.transform.position.z);
            Debug.Log("Does the wall exist? The answer may surprise you: " + wall.gameObject);
            GameObject.Destroy(slider);

            AbilityOneCooldown = 0;
            AbilityOneIsSliding = false;
        }
        AbilityOneCooldown += Time.deltaTime;

    }
    void AbilityTwoHandler()
    {
        if (Ability2Targeting)
        {
            AbilityTwo();
        }
        if (Input.GetButtonDown("Ability Two") && AbilityTwoCooldown > 12 && !Ability2Targeting)
        {
            Ability2Targeting = true;

            GameObject obj = Instantiate(reticle, gameObject.transform.position, reticle.transform.rotation);
            ReticleController = obj;
            // Debug.Log("46");
        }
        AbilityTwoCooldown += Time.deltaTime;
    }

    void AbilityTwo()
    {
        Physics.Raycast(Cam.transform.position, Cam.transform.forward, out var hit, 75f);

        if (hit.point != new Vector3(0, 0, 0))
        {
            DACLAMPA = hit.point;
        }
        Debug.Log(hit.collider.name);
     //  Debug.Log("59");
        ReticleController.transform.position = hit.point;
        //ReticleController.transform.position = Vector3.Lerp(ReticleController.transform.position, DACLAMPA, 1f);
        Debug.Log(ReticleController.transform.position);
        if (Input.GetButtonDown("Ability Two") || Input.GetButtonDown("Fire1"))
        {
            Ability2Targeting = false;
            AbilityTwoCooldown = 0;
            AbilityTwoSpawn();
            Destroy(ReticleController);
            Debug.Log("test");

        }

    }
    void AbilityTwoSpawn()
    {
        GameObject obj = Instantiate(AbilityTwoObject, ReticleController.transform.position+new Vector3(0,0.05f,0), Quaternion.identity);
        obj.GetComponent<BuilderAbilityTwo>().Target = Target;
        Destroy(obj, 4f);
    }


}
