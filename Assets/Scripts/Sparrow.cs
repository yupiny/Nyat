using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparrow : MonoBehaviour, IDamagable
{
    private Animator animator;
    public void Damage(GameObject attacker, Sword causer, float power)
    {
        animator.SetTrigger("Damaged");
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
   
}
