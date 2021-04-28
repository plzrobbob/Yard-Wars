using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used for setting the agro num if the object is targetable by AI.
/// it is fairly straight forward.  In the inspector set what type of target the object is.
/// The object will now be targetable by AI.
/// One object should have at most only one of these scripts
/// </summary>
public class TargetProperty : MonoBehaviour
{
    [Header("Set Agronum")]
    public bool isTurret;
    public bool isPlayer;
    public bool isWall;
    //public bool RecentlyAttacked;

    [Header("Do Not Change Manually Unless Testing")]
    public int agronum;//if you change the base values of agro number, then you will have to change the values in get agro target
    public bool onpath;


    //private float timer = 5f;
    //private float timercount;
    private void Update()
    {
        //the following three if statements are just so agro can be easily changed from inspector
        if (isPlayer && agronum != 30)
        {
            agronum = 30;
            isWall = false;
            isTurret = false;
        }
        else if (isTurret && agronum != 20)
        {
            agronum = 20;
            isWall = false;
            isPlayer = false;
        }
        else if (isWall && agronum != 10)
        {
            agronum = 10;
            isWall = false;
            isTurret = false;
        }

        if (gameObject.tag == "Wall")
        {
            isWall = true;
        }        
        
        if (gameObject.tag == "Turret")
        {
            isTurret = true;
        }        
        
        if (gameObject.tag == "PlayerHolder")
        {
            isPlayer = true;
        }

        //if (RecentlyAttacked)
        //{
        //    if (timercount >= timer)
        //    {
        //        timercount = 0;
        //        RecentlyAttacked = false;
        //    }
        //    timercount += Time.deltaTime;
        //}

    }
}
