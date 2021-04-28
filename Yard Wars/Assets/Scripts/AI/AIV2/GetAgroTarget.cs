using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAgroTarget : MonoBehaviour
{
    public float AgroRange = 20f;
    public LayerMask OverlapMask;
    //public AINodePath aiNodePath;

    [Serializable]
    public struct TargetStruct//the struct that tells us if the node has been reached
    {
        public GameObject PossibleTarget;
        public float targetAgroNum;
        public float dist;
    }

    [SerializeField]
    private List<TargetStruct> data2;//the path that is set by the waveManager Script on instantiation

    public GameObject TurretTarget = null;
    public GameObject WallTarget = null;
    public GameObject PlayerTarget = null;


    //need a case statement where if the player has recently damaged a minion, then it will consider the player as an attackintg target
    //Additonally, minions can follow palyers a little away from the lane.
    public (GameObject, GameObject, GameObject) GetTargetsInRange()
    {
        Collider[] TargetableObjects = Physics.OverlapSphere(transform.position, AgroRange, OverlapMask);//get all objects in a radius around the minion

        data2.Clear();//reset the values for targets and the struct
        TurretTarget = null;
        WallTarget = null;

        foreach (var TargetObj in TargetableObjects)//for each object check if the object has a target script
        {
            //Debug.Log("checked");
            var Scripttarprop = TargetObj.GetComponent<TargetProperty>();
            if (Scripttarprop != null)//if if has a target script and it is on the path then push that object onto our struct.  otherwise set value to -1;
            {
                if (Scripttarprop.agronum == 10 || Scripttarprop.agronum == 20)//if the object is a wall or a turret
                {
                    if (Scripttarprop.onpath)
                    {
                        var temp = new TargetStruct();
                        temp.PossibleTarget = TargetObj.gameObject;
                        temp.targetAgroNum = Scripttarprop.agronum;
                        temp.dist = Vector3.Distance(transform.position, TargetObj.transform.position);
                        data2.Add(temp);
                    }
                }
            }
        }

        //get the closest turret and wall, and set them as the target
        float WallDist = 1000;
        float TurretDist = 1000;
        for (int i = 0; i < data2.Count; i++)
        {
            if (data2[i].targetAgroNum == 10 && data2[i].dist < WallDist)
            {
                WallDist = data2[i].dist;
                WallTarget = data2[i].PossibleTarget;
            }
            if (data2[i].targetAgroNum == 20 && data2[i].dist < TurretDist)
            {
                TurretDist = data2[i].dist;
                TurretTarget = data2[i].PossibleTarget;
            }
        }
        return (WallTarget, TurretTarget, PlayerTarget);
    }
}
