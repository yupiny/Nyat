using UnityEngine;

public class SparrowAttack : MonoBehaviour
{
    // damgae �������̽� ��� �ް� OnDamge ����
    // ���� �ݶ��̴� ����, Ű�� ��� o
    // �÷��̾� hp ����� , dead ����� ��
    // 

    private Collider sparrowCollider;

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
    }

    private void Start ()
   {
        End_Collision();
   }

     private void OnTriggerEnter(Collider other)
    {
        if (gameObject == other.gameObject)
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