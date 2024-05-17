using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 2.0f;
    [SerializeField]
    private float runSpeed = 4.0f;
    private float speed;
    private float horizontal;
    private float vertical;
    private bool bRun;


    [SerializeField]
    private GameObject swordPrefab;
    private GameObject swordClone;

    private Sword sword;

    private Transform holsterSword;
    private Transform handSword;

    private bool bDrawing = false; //검을 뽑는중인지 판단
    private bool bSheathing = false; //검을 넣는중인지
    private bool bEquipped = false; //무기를 장착했는지 여부
    private bool bAttacking = false;
    //private bool bcanMove = false;

    private bool bComboEnable;
    private bool bComboExist;

    Vector3 direction;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        holsterSword = GameObject.Find("Holster_Sword").transform;
        if (holsterSword != null)
        {
            swordClone = Instantiate<GameObject>(swordPrefab, holsterSword, false);
            sword = swordClone.GetComponent<Sword>();
        }

        handSword = GameObject.Find("Hand_Sword").transform;

    }

    private void Update()
    {
        UpdateMoving();
        UpdateDraw();
        UpdateAttack();
    }

#region Move
    private void UpdateMoving()
    {
        if (bAttacking)
            return;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        bool bRun = Input.GetButton("Run");
        speed = bRun ? runSpeed : walkSpeed;

        direction = (Vector3.forward * vertical) + (Vector3.right * horizontal);
        direction = direction.normalized * speed;
        transform.Translate(direction * Time.deltaTime);
        animator.SetFloat("SpeedX", direction.x);
        animator.SetFloat("SpeedZ", direction.z);
    }
    #endregion

#region Draw
    private void UpdateDraw()
    {
        if (Input.GetButtonDown("Draw") == false)
            return;

        if (bDrawing == true) //뽑는중이면
            return;

        if (bSheathing == true)
            return;


        if (bEquipped == false) //장착중이 아니면 
        {
            bDrawing = true; //검을 뽑는중으로 바꿈
            animator.SetBool("HasSword", bDrawing); //검뽑는 애니메이션 실행

            return;
        }

        bSheathing = true;
        animator.SetBool("UnhasSword", bSheathing);
    }

    private void BeginDraw()
    {
        swordClone.transform.parent.DetachChildren();

        swordClone.transform.position = new Vector3(0, 0, 0); // == Vector3.zero
        swordClone.transform.rotation = Quaternion.identity;
        swordClone.transform.localScale = Vector3.one;

        swordClone.transform.SetParent(handSword, false);
    }

    private void EndDraw()
    {
        bEquipped = true; //장착중으로 변경

        bDrawing = false;
        animator.SetBool("HasSword", false);

    }

    private void BeginUnequip()
    {
        swordClone.transform.parent.DetachChildren();

        swordClone.transform.position = new Vector3(0, 0, 0); // == Vector3.zero
        swordClone.transform.rotation = Quaternion.identity;
        swordClone.transform.localScale = Vector3.one;

        swordClone.transform.SetParent(holsterSword, false);


    }
    private void EndUnequip()
    {
        bEquipped = false;

        bSheathing = false;
        animator.SetBool("UnhasSword", false);
    }
    #endregion

    #region Attack
    private void UpdateAttack()
    {
        if (Input.GetButtonDown("Attack") == false)
            return;

        if (bDrawing == true) 
            return;

        if (bSheathing == true)
            return;

        if (bEquipped == false)
            return;

        if (bComboEnable)
        {
            bComboEnable = false;
            bComboExist = true;

            return;
        }

        if (bAttacking == true)
            return;

        
        bAttacking = true; //공격중
        animator.SetBool("IsAttacking", true); //공격 애니메이션 실행
        
    }

    private void Begin_Combo()
    {
        bComboEnable = true;
    }
    private void End_Combo()
    {
        bComboEnable = false;
    }

    private void Combo_Attack()
    {
        if (bComboExist == false)
            return;

        bComboExist = false;

        animator.SetTrigger("Combo");
    }
    private void End_Attack()
    {
        bAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    private void Begin_Collision()
    {
        sword.Begin_Collision();
    }

    private void End_Collision()
    {
        sword.End_Collision();
    }
    #endregion
}

