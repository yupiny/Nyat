using JetBrains.Annotations;
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

    private float attackCooldown = 2f;
    private float lastAttackTime;

    private GameObject playerobj;
    Player player;

    private Animator animator;
#region Damage
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
#endregion
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    
    private void Update()
    {
        Debug.Log(attacking);
        //Attack();
        PlayerTracker();// 플레이어 따라가는 기능
        TargetDeath();
    }
    private void TargetDeath()
    {
        if (targetObject != null)
        {
            Attack();
        }
    }
    private void Attack()
    {
        if (attacking == true)
            return;

        if (targetObject == null)
            return;

        if (player.dead)
            return;

        if (Vector3.Distance(targetObject.transform.position, transform.position) <= 0.7f)
        {
            if (IsAttackDelayOver())
            {
                attacking = true;
                lastAttackTime = Time.time; // 마지막 공격 시간 업데이트
                animator.SetTrigger("Attack");
                return;
            }
        }
    }
    public bool IsAttackDelayOver()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
            return true;
        else
            return false;
    }
#region   PlayerTracker
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
            MoveToRandomPositionAndResetTarget();
        }

        if (targetObject != null)
        {
            Vector3 lookPlayer = targetObject.transform.position - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPlayer.normalized, Vector3.up);
            this.transform.rotation = rotation;
        }
    }
    #endregion
    private void MoveToRandomPositionAndResetTarget() //랜덤위치 재설정
    {
        moveToPosition = GetRandomPosition();

        Vector3 look = moveToPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(look.normalized, Vector3.up);
        transform.rotation = rotation;

        targetObject = null;
        hasTarget = false;
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

    // 객체가 파괴될때 자동 1회 호출
    private void OnDestroy()
    {
        GameManager.Instance.SetSparrow(-1);
    }

}
