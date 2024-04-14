using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Config/Unit")]
public class UnitSO : ScriptableObject
{
    public string Name => _name;
    public UnitComp Prefab => _prefab;
    public UnitTypeSO UnitType => _unitType;
    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public float EnemyDetectionDistance => _enemyDetecationDistance;
    public float Speed => _speed;
    public float AttackRate => _attackRate;
    public float AttackRange => _attackRange;
    public Projectile ProjectilePrefab => _projectilePrefab;

    [SerializeField] private string _name;
    [SerializeField] private UnitComp _prefab;
    [SerializeField] private UnitTypeSO _unitType;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _enemyDetecationDistance;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _attackRate = 0.5f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private Projectile _projectilePrefab;
}
