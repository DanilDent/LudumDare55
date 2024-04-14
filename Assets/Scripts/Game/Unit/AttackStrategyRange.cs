using UnityEngine;
public class AttackStrategyRange : AttackStrategyCompBase
{
    public override void Attack(int damage, HealthComp targetHealth, Projectile projPrefab = null)
    {
        base.Attack(damage, targetHealth);
        PlayAnim();

        if (targetHealth.IsUnit)
        {
            CreateProjectile(projPrefab, targetHealth, damage);
        }
    }

    // TODO: object pool this if kills perfomance
    private void CreateProjectile(Projectile prefab, HealthComp targetHealth, int damage)
    {
        var instance = Instantiate<Projectile>(prefab, GlobalConfigHolder.Instance.ProjectilesContainer);
        Vector2 senderPos2D = new Vector2(transform.position.x, transform.position.y);
        instance.Construct(senderPos2D, (targetHealth.transform.position - transform.position).normalized, _unitComp.Team, damage, _unitComp);
    }
}
