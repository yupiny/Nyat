using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Sparrow
{
    private Vector3 moveToPosition;
    void Start()
    {
        playerobj = GameObject.Find("Player");
        player = GameObject.Find("Player").GetComponent<Player>();
        player.OnDie += MoveToRandomPositionAndResetTarget;
        hp = 80;
        StartCoroutine(MoveTo());
        GameManager.Instance.SetSparrow(1);
    }
    private IEnumerator MoveTo()
    {
        float speed = 0.0f;
        float move = 0.0f;
        bool bFirst = false;
        moveToPosition = transform.position;

        while (true)
        {
            // 맞은 상태가 아니면서 , 공격중이 아니고 , 공격 딜레이 중이 아니라면
            if (hitted == false && attacking == false && IsAttackDelayOver())
            {
                if (Vector3.Distance(moveToPosition, transform.position) < 0.1f) //도달
                {
                    animator.SetFloat("SpeedX", 0);
                    speed = Random.Range(0.04f, 0.02f);
                    float time = Random.Range(1.0f, 2.0f);
                    move = Random.Range(1, 5);

                    if (bFirst == false)
                    {
                        bFirst = true;
                        time = 0.0f;
                    }
                    animator.SetFloat("SpeedY", 0);
                    yield return new WaitForSeconds(time);


                    moveToPosition = GetRandomPosition();
                    Vector3 look = moveToPosition - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(look.normalized, Vector3.up);
                    transform.rotation = rotation;
                }

                if (dead)
                    break;
                    

                transform.position += transform.forward * speed;
                animator.SetFloat("SpeedX", 2);
                animator.SetFloat("SpeedY", move);

                yield return new WaitForFixedUpdate();
            }
            else
            {
                animator.SetFloat("SpeedX", 0);
                animator.SetFloat("SpeedY", 0);
                yield return new WaitForSeconds(3);
            }
        }

    }
    public Vector3 GetRandomPosition()
    {
        float x = Random.Range(-10.0f, 10.0f);
        float z = Random.Range(-10.0f, 10.0f);

        return new Vector3(x, 0 ,z);
    }
}
