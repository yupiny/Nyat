using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour
{

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(MoveTo());
    }
    private Animator animator;
    private Vector3 moveToPosition;
    private IEnumerator MoveTo()
    {
        float speed = 0.0f;
        float move = 0.0f;
        bool bFirst = false;
        moveToPosition = transform.position;

        while (true)
        {
            if (Vector3.Distance(moveToPosition, transform.position) < 0.1f) //µµ´Þ
            {
                animator.SetFloat("SpeedX", 0);
                speed = Random.Range(0.1f, 0.03f);
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

            transform.position += transform.forward * speed;
            animator.SetFloat("SpeedX", 2);
            animator.SetFloat("SpeedY", move);

            yield return new WaitForFixedUpdate();
        }
    }
    public Vector3 GetRandomPosition()
    {
        float x = Random.Range(-10.0f, 10.0f);
        float z = Random.Range(-10.0f, 10.0f);

        return new Vector3(x, 0 ,z);
    }
}
