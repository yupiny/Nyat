using System.Collections;
using System.Collections.Generic;
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
    private GameObject sword;

    private Transform holsterSword;
    private Transform handSword;

    private bool bDrawing = false;
    private bool bSheathing = false;
    private bool bEquipped = false;

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
            sword = Instantiate<GameObject>(swordPrefab, holsterSword, false);  
        }

        handSword = GameObject.Find("Hand_Sword").transform;

    }
    
    private void Update()
    {
        UpdateMoving();
        UpdateDraw();
    }
    private void UpdateMoving()
    {
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

    private void UpdateDraw()
    {
        if (Input.GetButtonDown("Draw") == false)
            return;

        animator.SetTrigger("IsDraw");
    }

    private void BeginDraw()
    {
        sword.transform.parent.DetachChildren();

        sword.transform.position = new Vector3(0, 0, 0);
        sword.transform.rotation = Quaternion.identity;

        sword.transform.SetParent(handSword, false);
    }

    private void EndDraw()
    {
        animator.SetBool("HasSword", true);
    }
    
}
