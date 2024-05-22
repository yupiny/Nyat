using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class Sparrow : MonoBehaviour, IDamagable
{
    private float hp;
    public bool dead;
    public bool hitted;
    public bool attacking= false;
    public float radius;
    private Vector3 offset = Vector3.zero;
    private GameObject targetObject;
    private bool hasTarget;
    Player player;

    private Animator animator;
    public void Damage(GameObject attacker, float power)
    {
        hp = hp - power;

        if(hp<=0)
        {
            dead = true;
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
        //Attack();
        PlayerTracker();// 플레이어 따라가는 기능

    }
    private void FixedUpdate()
    {
        TargetDeath();
    }
    private void Attack()
    {
        if (attacking == true)
            return;

        if (targetObject == null)
            return;

        if (player.dead)
            return;

            if (Vector3.Distance(targetObject.transform.position, transform.position) <= 0.7f  )
        {
            attacking = true;
            animator.SetTrigger("Attack");

            return;
        }

        if(Vector3.Distance(targetObject.transform.position, transform.position) > 1f)
        {
            attacking = false;
        }
        // 만약에 거리가 가깝다면.
        // attacking을 true로
        // 공격 애니
        // SparrowAttack 스크립트에 구현되어 있는 콜라이더 키는 함수 실행하기(애니메이션 event)
        // SparrowAttack 스크립트에 구현되어 있는 콜라이더 끄는 함수 실행하기(애니메이션 event)
        // 
    }

    private void TargetDeath()
    {
        if (targetObject != null)
        {
            Attack();
        }

    }


    private void PlayerTracker()
    {
        if (dead)
            return;

        if (attacking == true)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 6);
        foreach (Collider collider in colliders)
        {
            targetObject = collider.gameObject;
            hasTarget = true;
        }

        if (targetObject == null)
            return;

        if (dead == true)
        {
            targetObject = null;
            hasTarget = false;
            animator.SetFloat("SpeedX", 0);
            animator.SetFloat("SpeedY", 0);
            return;
        }

        if (Vector3.Distance(targetObject.transform.position, transform.position) > radius + 2 && hasTarget)//멀어지면 트루
        {
            moveToPosition = GetRandomPosition();

            Vector3 look = moveToPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(look.normalized, Vector3.up);
            transform.rotation = rotation;

            targetObject = null;
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
