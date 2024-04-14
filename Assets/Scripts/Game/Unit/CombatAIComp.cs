using System.Linq;
using UnityEngine;

public class CombatAIComp : MonoBehaviour
{
    private UnitComp _unitComp;
    private UnitFactory _unitFactory;
    private AttackStrategyCompBase _attackStrategyComp;
    private float _enemyDetectionDistance => _unitSO.EnemyDetectionDistance;
    private float _attackRange => _unitSO.AttackRange;

    [SerializeField] private HealthComp _currentAttackTarget;
    private MovementComp _movementComp;

    private UnitSO _unitSO;
    private float _lastAttackTime = float.NegativeInfinity;
    private bool _isInAttackingState;
    private BuildingsHolder _buildingsHolder;

    public void Construct(UnitSO unitSO)
    {
        _unitSO = unitSO;

        _buildingsHolder = BuildingsHolder.Instance;

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
        if (!SearchForEnemies())
        {
            CheckForTargetBuildingProximity();
        }
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

            if (Vector3.Distance(transform.position, enemyUnit.transform.position) < _enemyDetectionDistance && _currentAttackTarget == null)
            {
                _unitComp.AddTarget(enemyUnit.transform);
                enemyUnit.HealthComp.OnDied += HandleOnDied;
                _currentAttackTarget = enemyUnit.HealthComp;
                return true;
            }
        }

        return false;
    }

    private bool CheckForTargetBuildingProximity()
    {
        if (_currentAttackTarget != null)
        {
            return false;
        }

        IBulding targetBuilding = _buildingsHolder.Buildings.FirstOrDefault(_ => _.GetTransform().gameObject == _movementComp.TargetTransform.gameObject);
        if (targetBuilding == null)
        {
            _currentAttackTarget = null;
            return false;
        }
        if (targetBuilding.GetTransform().TryGetComponent<Spawner>(out var spawner))
        {
            if (Vector3.Distance(spawner.transform.position, transform.position) < _attackRange + 0.25f)
            {
                _currentAttackTarget = spawner.GetComponent<HealthComp>();
                return true;
            }
        }

        _currentAttackTarget = null;
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

        if (_currentAttackTarget == null)
        {
            _lastAttackTime = float.NegativeInfinity;
            return;
        }

        if (Vector3.Distance(_currentAttackTarget.transform.position, transform.position) > _attackRange)
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
            _attackStrategyComp.Attack(_unitSO.Damage, _currentAttackTarget, _unitSO.ProjectilePrefab);
        }
    }


    private void HandleOnDied(HealthComp comp)
    {
        comp.OnDied -= HandleOnDied;
        _unitComp.RemoveTarget(comp.transform);
        if (_currentAttackTarget?.GetComponent<UnitComp>() != null)
        {
            _currentAttackTarget = null;
        }
    }
}
