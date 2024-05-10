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

    private GameObject holsterSword;
    private GameObject handSword;

    Vector3 direction;
    
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Transform holsterSword = GameObject.Find("Holster_Sword").transform;
        if (holsterSword != null)
        {
            sword = Instantiate<GameObject>(swordPrefab, holsterSword, false);  
        }
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
    private void Update()
    {
        UpdateMoving();
    }
}
