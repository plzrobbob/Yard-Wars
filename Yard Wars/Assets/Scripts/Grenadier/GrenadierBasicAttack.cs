using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierBasicAttack : MonoBehaviour
{

    // This script is the Grenadiers Basic attack. It is a ranged basic attack with an aoe
    //It will looks like tossing a water balloon so it will have an arc and certain range.
    // Start is called before the first frame update

    public GameObject WaterBalloon;
    public float ExitSpeed;
    private float weaponCooldown;
    public GameObject BalloonOrigin;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        weaponCooldown += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && weaponCooldown > 1)
        {
            CreateBalloon();
            weaponCooldown = 0;
        }
    }

    private void CreateBalloon()
    {
        //This is temporarily positioned in front of the player. When player is created, have it spawn from the top of their hand and lets hope it looks good
        //Maybe also have it where when you throw it, the balloon that it looks like the player is carrying is turned off until they "reload"
        GameObject obj = Instantiate(WaterBalloon, BalloonOrigin.transform.position, BalloonOrigin.transform.rotation);
        obj.GetComponent<Rigidbody>().velocity = (Camera.main.transform.forward * ExitSpeed) ;
        //BalloonOrigin.transform.forward
    }
}
