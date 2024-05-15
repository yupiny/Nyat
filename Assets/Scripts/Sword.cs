using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Collider swordCollider;
    private GameObject rootObject;

    private void Awake()
    {
        swordCollider = GetComponent<Collider>();
        rootObject = transform.root.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        End_Collision();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rootObject == other.gameObject)
            return;

        IDamagable damage = other.gameObject.GetComponent<IDamagable>();
        damage?.Damage(rootObject, this, 20);
    }

        public void Begin_Collision()
    {
        swordCollider.enabled = true;
    }

    public void End_Collision()
    {
        swordCollider.enabled = false;

       // hittedList.Clear();
    }
}
