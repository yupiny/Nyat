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
        PlayerTracker();// �÷��̾� ���󰡴� ���
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
                lastAttackTime = Time.time; // ������ ���� �ð� ������Ʈ
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
        Debug.Log("colliders Ž��" + colliders.Length);
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

        // �÷��̾ ������ �� ��ġ�� �����̰�, ������ �־����鼭 ���� ��ġ�� ���Ϸ��� ������
        // �÷��̾ �׾ targetObject�� null�̵Ǿ� transform.position ��ü�� �������� ���ϰ�
        // ������ return�ؼ� �ƿ� �������� �������� �ʴ´�.
        // �÷��̾��� ������� �� ��ġ������ ��� ������Ʈ �ϴٰ�
        // �÷��̾ ������ ��ġ���� ������Ʈ ���� ������ �÷��̾��� ������ ��ġ������ ����
        // ������ �� ��ġ������ ���� �־������� �ؿ��� ���ǰ˻��ϴٰ� �־����� 
        // ��ġ�� ���� ���ϸ� �ȴ�.
        if (Vector3.Distance(targetObject.transform.position, transform.position) > radius + 2 && hasTarget)//�־����� Ʈ��
        {
            MoveToRandomPositionAndResetTarget();
        }
            Vector3 lookPlayer = targetObject.transform.position - this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPlayer.normalized, Vector3.up);
            this.transform.rotation = rotation;
    }
    #endregion
    private void MoveToRandomPositionAndResetTarget() //������ġ �缳��
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

    // ��ü�� �ı��ɶ� �ڵ� 1ȸ ȣ��
    private void OnDestroy()
    {
        GameManager.Instance.SetSparrow(-1);
    }

}
