using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class Sparrow : MonoBehaviour, IDamagable
{
    private float hp;
    public bool dead;
    public bool hitted;
    public bool attacking= false;
    public float radius;
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private GameObject targetObject;
    private bool hasTarget;

    private float attackCooldown = 2f;
    private float lastAttackTime;

    private GameObject playerobj;
    Player player;
    private Rigidbody rigidbody;

    private Animator animator;
#region Damage
    public void Damage(GameObject attacker, float power, DoActionData doAction)
    {
        hp = hp - doAction.power;
        PushBack(doAction.distance);

        if (doAction.hitParticle != null)
        {
            GameObject obj = Instantiate(doAction.hitParticle, transform, false);
            obj.transform.localPosition = doAction.hitParticlePositionOffset;
            obj.transform.localScale = doAction.hitParticleScaleOffset;
        }

        if (hp<=0)
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
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update

    private void Update()
    {
        //Debug.Log(attacking);
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
#region Attack
    private void Attack()
    {
        if (attacking == true)
            return;

        if (targetObject == null)
            return;

        if (hitted == true)
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
    #endregion


 #region   PlayerTracker
    private Transform playerLastPosition;
    private void PlayerTracker()
    {
        if (dead)
            return;

        if (attacking == true)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 6);
        Debug.Log("colliders 탐색" + colliders.Length);
        if (colliders.Length == 0)
        {
            hasTarget = false;
            targetObject = null;
        }
        else
        { 
            foreach (Collider collider in colliders)
            {
                targetObject = collider.gameObject;
                hasTarget = true;
            }
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

        // 플레이어가 죽으면 그 위치는 고정이고, 참새는 멀어지면서 새로 위치를 구하려고 했으나
        // 플레이어가 죽어서 targetObject가 null이되어 transform.position 자체를 구하지도 못하고
        // 위에서 return해서 아예 이쪽으로 들어오지도 않는다.
        // 플레이어의 살아있을 때 위치정보를 계속 업데이트 하다가
        // 플레이어가 죽으면 위치정보 업데이트 하지 않으면 플레이어의 마지막 위치정보만 남고
        // 참새는 그 위치정보를 토대로 멀어지는지 밑에서 조건검사하다가 멀어지면 
        // 위치를 새로 구하면 된다.
        if (Vector3.Distance(targetObject.transform.position, transform.position) > radius + 2 && hasTarget)//멀어지면 트루
        {
            MoveToRandomPositionAndResetTarget();
        }
            Vector3 lookPlayer = targetObject.transform.position - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPlayer.normalized, Vector3.up);
            this.transform.rotation = rotation;
    }
    #endregion
    private void MoveToRandomPositionAndResetTarget() //랜덤위치 재설정
    {
        moveToPosition = GetRandomPosition();
        Debug.Log(moveToPosition);
        Vector3 look = moveToPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(look.normalized, Vector3.up);
        transform.rotation = rotation;

        targetObject = null;
        hasTarget = false;
    }

    public void End_Attack()
    {
        attacking = false;
    }

    private void End_Hitted()
    {
        hitted = false;
    }
    private void PushBack(float distance)
    {
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.forward * -1f * distance, ForceMode.Force);
        CancelInvoke("IsKinematicTrue");
        Invoke("IsKinematicTrue", 0.3f);
    }
    private void IsKinematicTrue()
    {
        rigidbody.isKinematic = true;
    }

    private void OnDisable()
    {
        player.OnDie -= MoveToRandomPositionAndResetTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position + offset;
        Gizmos.DrawWireSphere(position, radius);
        Gizmos.DrawWireSphere(moveToPosition, radius);
    }

    // 객체가 파괴될때 자동 1회 호출
    private void OnDestroy()
    {
        GameManager.Instance.SetSparrow(-1);
    }

}
