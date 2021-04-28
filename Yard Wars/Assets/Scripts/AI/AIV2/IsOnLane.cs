using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// will set objects to onpath if they are in the path trigger collider
/// </summary>
public class IsOnLane : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<TargetProperty>() != null)
        {
            if (other.GetComponent<TargetProperty>().onpath == false)
            {
                other.GetComponent<TargetProperty>().onpath = true;
            }
        }

        if (other.GetComponent<AINodePath>() != null)
        {
            if (other.GetComponent<AINodePath>().onpath == false)
            {
                other.GetComponent<AINodePath>().onpath = true;
                other.GetComponent<AINodePath>().LastKnownLanePos = null;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TargetProperty>() != null)
        {
            other.GetComponent<TargetProperty>().onpath = false;
        }

        if (other.GetComponent<AINodePath>() != null)
        {
            other.GetComponent<AINodePath>().onpath = false;
            other.GetComponent<AINodePath>().LastKnownLanePos = this.gameObject;
        }
    }
}
