using UnityEngine;

public class CombatAIComp : MonoBehaviour
{
    private UnitComp _unitComp;
    private UnitFactory _unitFactory;
    private AttackStrategyCompBase _attackStrategyComp;
    private float _enemyDetectionDistance => _unitSO.EnemyDetectionDistance;
    private float _attackRange => _unitSO.AttackRange;

    private HealthComp _currentTarget;
    private MovementComp _movementComp;

    private UnitSO _unitSO;
    private float _lastAttackTime = float.NegativeInfinity;
    private bool _isInAttackingState;
    public void Construct(UnitSO unitSO)
    {
        _unitSO = unitSO;
        _attackStrategyComp = AttackStrategyFactory.Instance.Create(GetComponent<UnitComp>(), _unitSO.UnitType.AttackType);
        _attackStrategyComp.Construct();
        _movementComp = GetComponent<MovementComp>();
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
        _movementComp.canMove = !_isInAttackingState;
    }

    private bool SearchForEnemies()
    {
        TeamEnum enemyTeam = TeamManager.InverseTeam(_unitComp.Team);
        var enemyUnits = _unitFactory.GetAllCreatedTeamUnits(enemyTeam);
        foreach (var enemyUnit in enemyUnits)
        {
            if (enemyUnit.IsDead) continue;

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
        _isInAttackingState = false;

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
            _isInAttackingState = false;
            return;
        }
        else
        {
            _isInAttackingState = true;
        }

        if (Time.time > _lastAttackTime + _unitSO.AttackRate)
        {
            _lastAttackTime = Time.time;
            _attackStrategyComp.Attack(_unitSO.Damage, _currentTarget, _unitSO.ProjectilePrefab);
        }
    }


    private void HandleOnDied(HealthComp comp)
    {
        comp.OnDied -= HandleOnDied;
        _unitComp.RemoveTarget(comp.transform);
        _currentTarget = null;
    }
}
