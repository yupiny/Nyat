using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Sparrow : MonoBehaviour, IDamagable
{
    private float hp;
    public bool dead;
    public bool hitted;
    public float radius;
    private Vector3 offset = Vector3.zero;
    private GameObject targetObject;
    private bool hasTarget;

    private Animator animator;
    public void Damage(GameObject attacker, Sword causer, float power)
    {
        hp = hp - power;

        if(hp<=0)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 2f); return;
        }
        hitted = true;

        Vector3 lookPlayer = attacker.transform.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPlayer.normalized, Vector3.up);
        this.transform.rotation = rotation; 
        
        animator.SetTrigger("Damaged");
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1<<6);
        foreach(Collider collider in colliders) 
        {
            targetObject = collider.gameObject;
            hasTarget = true;
        }

        if (targetObject == null)
            return;

        if(Vector3.Distance(targetObject.transform.position, transform.position)> radius*2 && hasTarget)//멀어지면 트루
        {
            moveToPosition = GetRandomPosition();

            Vector3 look = moveToPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(look.normalized, Vector3.up);
            transform.rotation = rotation;

            targetObject =null;
            hasTarget = false;
        }

        if (targetObject != null)
        {
            Vector3 lookPlayer = targetObject.transform.position - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPlayer.normalized, Vector3.up);
            this.transform.rotation = rotation;
        }

    }
    private void End_Hitted()
    {
        hitted = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position + offset;
        Gizmos.DrawWireSphere(position, radius);
    }

}
