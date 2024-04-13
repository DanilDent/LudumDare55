using UnityEngine;

public class CombatAIComp : MonoBehaviour
{
    private UnitComp _unitComp;
    private UnitFactory _unitFactory;
    private AttackStrategyCompBase _attackStrategyComp;
    private float _enemyDetectionDistance => _unitSO.EnemyDetectionDistance;
    private float _attackRange => _unitSO.AttackRange;

    private HealthComp _currentTarget;

    private UnitSO _unitSO;
    private float _lastAttackTime = float.NegativeInfinity;
    public void Construct(UnitSO unitSO)
    {
        _unitSO = unitSO;
        _attackStrategyComp = AttackStrategyFactory.Instance.Create(GetComponent<UnitComp>(), _unitSO.UnitType.AttackType);
        _attackStrategyComp.Construct();
    }

    private void Start()
    {
        _unitComp = GetComponent<UnitComp>();
        _unitFactory = UnitFactory.Instance;
    }

    private void Update()
    {
        SearchForEnemies();
        HandleCombat();
    }

    private bool SearchForEnemies()
    {
        TeamEnum enemyTeam = TeamManager.InverseTeam(_unitComp.Team);
        var enemyUnits = _unitFactory.GetAllCreatedTeamUnits(enemyTeam);
        foreach (var enemyUnit in enemyUnits)
        {
            if (Vector3.Distance(transform.position, enemyUnit.transform.position) < _enemyDetectionDistance && _currentTarget == null)
            {
                _unitComp.AddTarget(enemyUnit.transform);
                enemyUnit.HealthComp.OnDied += HandleOnDied;
                _currentTarget = enemyUnit.HealthComp;
                return true;
            }
        }

        return false;
    }

    private void HandleCombat()
    {
        if (_attackStrategyComp == null)
        {
            Debug.LogWarning($"No attack strategy assigned for unit {gameObject.name}");
            _lastAttackTime = float.NegativeInfinity;
            return;
        }

        if (_currentTarget == null)
        {
            _lastAttackTime = float.NegativeInfinity;
            return;
        }

        if (Vector3.Distance(_currentTarget.transform.position, transform.position) > _attackRange)
        {
            _lastAttackTime = float.NegativeInfinity;
            return;
        }

        if (Time.time > _lastAttackTime + _unitSO.AttackRate)
        {
            _lastAttackTime = Time.time;
            _attackStrategyComp.Attack(_unitSO.Damage, _currentTarget);
        }
    }



    private void HandleOnDied(HealthComp comp)
    {
        comp.OnDied -= HandleOnDied;
        _unitComp.RemoveTarget(comp.transform);
        _currentTarget = null;
    }
}
