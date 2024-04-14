using UnityEngine;

// TODO: use object pooling here if perfomance died
public class Projectile : MonoBehaviour
{
    public float speed;

    private Vector3 _dir;
    [SerializeField] private TeamEnum _team;
    private int _damage;
    private UnitComp _senderUnitComp;
    private Rigidbody2D _rb;

    public void Construct(Vector2 position, Vector3 dir, TeamEnum team, int damage, UnitComp senderUnitComp)
    {
        transform.position = position;
        _dir = dir;
        _team = team;
        _damage = damage;
        _senderUnitComp = senderUnitComp;
        _rb = GetComponent<Rigidbody2D>();

        _rb.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var unitComp = collision.gameObject.GetComponent<UnitComp>();
        var targetHealth = collision.gameObject.GetComponent<HealthComp>();
        Debug.Log($"Collision: {collision.gameObject.name}, {gameObject.name}");

        if (unitComp == null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 5f);
            return;
        }

        if (unitComp.Team == _team)
        {
            return;
        }

        if (targetHealth == null)
        {
            return;
        }

        if (targetHealth.IsUnit)
        {
            float newTargetHealth = targetHealth.Health.Value -
                _damage * GlobalConfigHolder.Instance.GetDamageModifier(_senderUnitComp.UnitSO.UnitType, targetHealth.UnitComp.UnitSO.UnitType);
            if (newTargetHealth < 0)
            {
                newTargetHealth = 0;
            }
            targetHealth.Health.Value = (int)newTargetHealth;
            Debug.Log($"Damaged: {gameObject.name}, health: {targetHealth.Health.Value}");
        }

        gameObject.SetActive(false);
        Destroy(gameObject, 5f);
    }
}
