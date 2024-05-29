using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody box;

    private void Awake()
    {
        box = GetComponent<Rigidbody>();
    }

    private void Start()
    {
      
    }

    private void Update()
    {
            
    }


    private void PushBack()
    {
        box.isKinematic = false;
        box.velocity = Vector3.zero;
        box.AddForce(transform.forward * -1f * 30, ForceMode.Force);
        CancelInvoke("IsKinematicTrue");
        Invoke("IsKinematicTrue", 2f);
    }

    private void IsKinematicTrue()
    {
        box.isKinematic = true;
    }

    
}