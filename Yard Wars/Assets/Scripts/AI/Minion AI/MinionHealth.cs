using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealth : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth;
    private bool Isdead;
    public GameObject minionbody;
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        if (CurrentHealth <= 0 && !Isdead)
        {
            Isdead = true;
            StartCoroutine(Dead());
        }
    }
    public IEnumerator Dead()
    {
        //Player_Animator.SetBool("IsDead", true);
        //play dead animation
        yield return new WaitForSeconds(1);
        Destroy(minionbody);
    }
}
