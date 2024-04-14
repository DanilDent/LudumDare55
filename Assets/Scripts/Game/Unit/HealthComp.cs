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

    public UnitComp UnitComp => _unitComp;
    private UnitComp _unitComp;

    public bool IsUnit => _unitComp != null;

    public void Construct(UnitSO unitSO)
    {
        _unitSO = unitSO;
        Health.AddListener(OnHealthChanged);
        Health.Value = _unitSO.MaxHealth;
        _unitComp = GetComponent<UnitComp>();
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
            OnDied?.Invoke(this);
        }
    }
}
