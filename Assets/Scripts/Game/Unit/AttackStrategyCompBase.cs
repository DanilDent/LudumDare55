using UnityEngine;

public abstract class AttackStrategyCompBase : MonoBehaviour
{
    protected UnitComp _unitComp;

    public virtual void Construct()
    {
        _unitComp = GetComponent<UnitComp>();
    }

    public virtual void Attack(int damage, HealthComp targetHealth, Projectile projPrefab = null)
    {
        Debug.Log($"Attack: {gameObject.name} -> {targetHealth.gameObject.name}");
    }

    protected virtual void PlayAnim()
    {

    }
}
