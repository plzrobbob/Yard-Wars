using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierAbilityOne : MonoBehaviour
{
    public float AbilityRange;
    public float AbilityDamage;
    private Collider[] EnemiesDamaged;
    public LayerMask layer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            EnemiesDamaged = Physics.OverlapSphere(transform.position, AbilityRange, layer);
            DoDamage();
            Debug.Log(EnemiesDamaged.Length);
        }
    }

    void DoDamage()
    {
        for (int i = 0; i < EnemiesDamaged.Length; i++)
        {
            Debug.Log("running");
                HealthScript M_HealthScript = EnemiesDamaged[i].gameObject.GetComponent<HealthScript>();
                M_HealthScript.CurrentHealth -= AbilityDamage;
        }
    }
}
