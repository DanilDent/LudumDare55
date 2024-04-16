using Misc;
using System;
using UnityEngine;

public class HealthComp : MonoBehaviour
{
    [SerializeField] private int _debugHealth;

    public Action<HealthComp> OnDied;
    public ReactiveProperty<int> Health = new ReactiveProperty<int>();
    public int MaxHealth => _unitSO.MaxHealth;
    private UnitSO _unitSO;
    private SpawnerConfigSO _spawnerSO;

    public UnitComp UnitComp => _unitComp;
    private UnitComp _unitComp;

    public Spawner SpawnerComp => _spawnerComp;
    private Spawner _spawnerComp;

    public bool IsUnit => _unitComp != null;
    public bool IsSpawner => _spawnerComp != null;

    public bool IsDead => _isDead;

    [SerializeField] private bool _isDead;

    public void Construct(UnitSO unitSO)
    {
        _unitSO = unitSO;
        Health.Value = _unitSO.MaxHealth;
        Health.AddListener(OnHealthChanged);
        _unitComp = GetComponent<UnitComp>();
    }

    public void Construct(SpawnerConfigSO spawnerSO)
    {
        _spawnerSO = spawnerSO;
        Health.Value = spawnerSO.SpawnerHealth;
        Health.AddListener(OnHealthChanged);
        _spawnerComp = GetComponent<Spawner>();
    }

    private void OnDestroy()
    {
        Health.RemoveListener(OnHealthChanged);
    }

    private void OnHealthChanged(int health)
    {
        _debugHealth = health;
        if (health < 0)
        {
            Health.Value = 0;
            return;
        }

        if (health == 0)
        {
            if (TryGetComponent<CapsuleCollider2D>(out var collider))
            {
                collider.enabled = false;
            }
            _isDead = true;
            OnDied?.Invoke(this);
        }
    }
}
