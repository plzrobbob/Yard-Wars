using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject target;

    public GameObject TurretHead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - TurretHead.transform.position);

        TurretHead.transform.rotation = Quaternion.Slerp(TurretHead.transform.rotation, lookOnLook, Time.deltaTime);
    }
}
