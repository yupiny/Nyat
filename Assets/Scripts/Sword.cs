using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Collider swordCollider;
    private GameObject rootObject;
    private List<GameObject> gameObjList;

    private void Awake()
    {
        gameObjList = new List<GameObject>();
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

        // 만약에 리스트에 없다면 리스트에 넣어준다.
        // 만약에 리스트에 있었다면 return해서 밑에 데미지 주는 함수를 호출하지 않는다. 
        if (gameObjList.Contains(other.gameObject))
            return;

        gameObjList.Add(other.gameObject);

        IDamagable damage = other.gameObject.GetComponent<IDamagable>();
        damage?.Damage(rootObject, 20);
    }

        public void Begin_Collision()
    {
        swordCollider.enabled = true;
    }

    public void End_Collision()
    {
        swordCollider.enabled = false;

        gameObjList.Clear();
    }
}
