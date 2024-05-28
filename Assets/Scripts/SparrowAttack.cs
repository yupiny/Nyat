using System.Collections.Generic;
using UnityEngine;

public class SparrowAttack : MonoBehaviour
{
    // damgae 인터페이스 상속 받고 OnDamge 구현
    // 본인 콜라이더 끄는, 키는 기능 o
    // 플레이어 hp 만들기 , dead 만들기 피
    // 

    private List<Collider> sparrowColliders;
    private Collider sparrowAttackCollider;
    LayerMask spaarowLayerMask;

    private void Awake()
    {
        sparrowColliders = new List<Collider>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            sparrowColliders.Add(collider);

            if (collider.gameObject.name.Equals("Sparrow_collider"))
            {
                sparrowAttackCollider = collider;
                break;
            }
        }
        spaarowLayerMask = LayerMask.GetMask("Sparrow");
    }

    private void Start()
    {
        End_Collision();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject == other.gameObject)
            return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Sparrow"))
            return;

        IDamagable damage = other.gameObject.GetComponent<IDamagable>();
        damage?.Damage(gameObject, 20);
    }

    public void Begin_Collision()
    {
        sparrowAttackCollider.enabled = true;
    }

    public void End_Collision()
    {
        sparrowAttackCollider.enabled = false;
    }

    //참새가 죽을때 호출되는 애니메이션 이벤트
    public void Sparrow_End_Collision()
    {
        foreach (Collider collider in sparrowColliders)
        {
            collider.enabled = false;
        }
    }
}