using UnityEngine;

public interface IDamagable 
{
    void Damage(GameObject attacker, float power, DoActionData doAction); //GameObject causer,
}
