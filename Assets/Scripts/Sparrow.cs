using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparrow : MonoBehaviour, IDamagable
{
    private float hp;

    private Animator animator;
    public void Damage(GameObject attacker, Sword causer, float power)
    {
        hp = hp - power;

        if(hp<=0)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 2f); return;
        }

        animator.SetTrigger("Damaged");
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        hp = 80;
    }

    
   
}
