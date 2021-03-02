using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject AbilityTwoObject;
    public float AbilityTwoCooldown;
    public bool Ability2Targeting;
    public Camera Cam;
    public GameObject ReticleController;
    public Vector3 DACLAMPA;
    public GameObject reticle;


    void Start()
    {
        Ability2Targeting = false;
        AbilityTwoCooldown = 12;
    }

    // Update is called once per frame
    void Update()
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
            Debug.Log("46");

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
        Destroy(obj, 2.5f);
    }


}
