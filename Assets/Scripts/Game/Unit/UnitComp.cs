using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComp))]
[RequireComponent(typeof(MovementComp))]
[RequireComponent(typeof(CombatAIComp))]
public class UnitComp : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private HealthComp _healthComp;
    [SerializeField] private MovementComp _movementComp;
    [SerializeField] private CombatAIComp _combatAIComp;

    public IBuilding _creatorBuilding;

    public UnitSO UnitSO => _unitSO;

    public HealthComp HealthComp => _healthComp;

    public TeamEnum Team => _team;

    [SerializeField] private TeamEnum _team;

    private List<Transform> _targetsList = new List<Transform>();
    private UnitSO _unitSO;
    public bool IsDead
    {
        get => _healthComp.IsDead;
    }

    public Transform CurrentTarget => _targetsList.Count > 0 ? _targetsList[_targetsList.Count - 1] : null;

    public void AddTarget(Transform target)
    {
        _targetsList.Add(target);
        _movementComp.SetTarget(CurrentTarget);
    }

    public void PrependTarget(Transform target)
    {
        _targetsList.Insert(0, target);
        _movementComp.SetTarget(CurrentTarget);
    }

    public void RemoveTarget(Transform target)
    {
        _targetsList.Remove(target);
        _movementComp.SetTarget(CurrentTarget);
    }

    private void Update()
    {
        HandleMovementTarget();
        PerformMergerProximityCheck();
    }

    public void Construct(TeamEnum team, UnitSO unitSO)
    {
        _movementComp = GetComponent<MovementComp>();
        _healthComp = GetComponent<HealthComp>();
        _combatAIComp = GetComponent<CombatAIComp>();

        _team = team;
        _unitSO = unitSO;

        _healthComp.Construct(_unitSO);
        _movementComp.Construct(_unitSO);
        _combatAIComp.Construct(_unitSO);

        _healthComp.OnDied += HandleOnDied;
    }

    private void HandleMovementTarget()
    {
        if (_movementComp.TargetTransform == null)
        {
            _movementComp.SetTarget(CurrentTarget);
            return;
        }
        if (_movementComp.TargetTransform.gameObject == null)
        {
            _movementComp.SetTarget(CurrentTarget);
            return;
        }
    }

    private void PerformMergerProximityCheck()
    {
        if (CurrentTarget == null)
        {
            return;
        }

        if (!CurrentTarget.TryGetComponent<Merger>(out var targetMerger))
        {
            return;
        }

        float veryClose = 0.01f;
        if (Vector3.Distance(targetMerger.transform.position, transform.position) < veryClose)
        {
            Debug.Log($"Unit {gameObject.name} came to merger {gameObject.name}");
            targetMerger.AddEntity();
        }
    }

    private void HandleOnDied(HealthComp healthComp)
    {
        _healthComp.OnDied -= HandleOnDied;

        _healthComp.enabled = !IsDead;
        _movementComp.enabled = !IsDead;
        _combatAIComp.enabled = !IsDead;

        if (IsDead)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
    }
}
