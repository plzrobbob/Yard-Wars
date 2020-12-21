using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRidOfBall : MonoBehaviour
{

    /// <summary>
    /// This shit is temporary. I am just trying to make sure stuff doesn't get cluttered
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Add the spawning of some sort of special effect here like a balloon hitting something and spawning;
        Destroy(gameObject);
    }
}
