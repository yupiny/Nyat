using UnityEngine;

public class SparrowAttack : MonoBehaviour
{
    // damgae 인터페이스 상속 받고 OnDamge 구현
    // 본인 콜라이더 끄는, 키는 기능 o
    // 플레이어 hp 만들기 , dead 만들기 피
    // 

    private Collider sparrowCollider;
    LayerMask spaarowLayerMask;

    private void Awake()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders) 
        {
            if(collider.gameObject.name.Equals("Sparrow_collider"))
            {
                sparrowCollider= collider;
                break;
            }
        }
        spaarowLayerMask = LayerMask.GetMask("Sparrow");
    }

    private void Start ()
   {
        End_Collision();
   }

     private void OnTriggerEnter(Collider other)
    {
        if (gameObject == other.gameObject)
            return;
        if(other.gameObject.layer == LayerMask.NameToLayer("Sparrow"))
            return;

        IDamagable damage = other.gameObject.GetComponent<IDamagable>();
        damage?.Damage(gameObject, 20);
    }

    public void Begin_Collision()
    {
        sparrowCollider.enabled = true;
    }

    public void End_Collision()
    {
        sparrowCollider.enabled = false;
    }
}