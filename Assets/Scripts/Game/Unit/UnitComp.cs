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

    public UnitSO UnitSO => _unitSO;

    public HealthComp HealthComp => _healthComp;

    public TeamEnum Team => _team;

    [SerializeField] private TeamEnum _team;

    private List<Transform> _targetsList = new List<Transform>();
    private UnitSO _unitSO;
    private bool _isDead;
    public bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            _healthComp.enabled = !_isDead;
            _movementComp.enabled = !_isDead;
            _combatAIComp.enabled = !_isDead;

            if (_isDead)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                GetComponentInChildren<SpriteRenderer>().color = Color.black;
            }
        }
    }

    public Transform CurrentTarget => _targetsList.Count > 0 ? _targetsList[_targetsList.Count - 1] : null;

    public void AddTarget(Transform target)
    {
        _targetsList.Add(target);
        _movementComp.SetTarget(target);
    }

    public void RemoveTarget(Transform target)
    {
        _targetsList.Remove(target);
        if (_targetsList.Count > 0)
        {
            _movementComp.SetTarget(_targetsList[_targetsList.Count - 1]);
        }
        else
        {
            // perform search for a new target
        }
    }

    private void Update()
    {
        if (_movementComp.TargetPosition.gameObject == null)
        {
            _movementComp.SetTarget(CurrentTarget);
        }
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
    }
}
