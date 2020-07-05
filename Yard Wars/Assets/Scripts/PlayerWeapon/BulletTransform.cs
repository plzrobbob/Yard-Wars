using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTransform : MonoBehaviour
{
    public Vector3 Target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, 1 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
