using UnityEngine;
public class AttackStrategyMelee : AttackStrategyCompBase
{
    public override void Attack(float damage, HealthComp targetHealth)
    {
        base.Attack(damage, targetHealth);
        PlayAnim();

        if (targetHealth.IsUnit)
        {
            float newTargetHealth = targetHealth.Health.Value -
                damage * GlobalConfigHolder.Instance.GetDamageModifier(_unitComp.UnitSO.UnitType, targetHealth.UnitComp.UnitSO.UnitType);
            if (newTargetHealth < 0)
            {
                newTargetHealth = 0;
            }
            targetHealth.Health.Value = (int)newTargetHealth;
            Debug.Log($"Damaged: {gameObject.name}, health: {targetHealth.Health.Value}");
        }
    }
}
