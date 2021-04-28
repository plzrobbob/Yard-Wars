using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionPathingRandomization_old : MonoBehaviour
{
    public Vector3 position;
    public GameObject nextNode;
    public float circleRadius = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 AddPathVariance()
    {
        position = new Vector3(gameObject.transform.position.x + Random.Range(-circleRadius, circleRadius),
                               gameObject.transform.position.y,
                               gameObject.transform.position.z + Random.Range(-circleRadius, circleRadius));
        return position;
    }
}
