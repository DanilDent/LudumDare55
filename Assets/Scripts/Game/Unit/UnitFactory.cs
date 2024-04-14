using Misc;
using System.Collections.Generic;
using UnityEngine;

// Handles life time of units
public class UnitFactory : MonoSingleton<UnitFactory>
{
    private Dictionary<TeamEnum, HashSet<UnitComp>> _createdUnits;
    private Queue<UnitComp> _deadQueue = new Queue<UnitComp>();

    protected override void Awake()
    {
        base.Awake();

        _createdUnits = new Dictionary<TeamEnum, HashSet<UnitComp>>();
        _createdUnits.Add(TeamEnum.Player, new HashSet<UnitComp>());
        _createdUnits.Add(TeamEnum.Enemy, new HashSet<UnitComp>());
    }

    public UnitComp Create(Transform parent, Vector3 position, TeamEnum team, UnitSO unitSO)
    {
        UnitComp instance = Instantiate(unitSO.Prefab, parent);
        instance.transform.position = position;
        instance.Construct(team, unitSO);
        instance.GetComponent<HealthComp>().OnDied += HandleOnDied;
        _createdUnits[team].Add(instance);
        return instance;
    }

    public HashSet<UnitComp> GetAllCreatedTeamUnits(TeamEnum team)
    {
        return _createdUnits[team];
    }

    private void HandleOnDied(HealthComp comp)
    {
        comp.OnDied -= HandleOnDied;
        _deadQueue.Enqueue(comp.GetComponent<UnitComp>());
    }

    private void LateUpdate()
    {
        while (_deadQueue.Count > 0)
        {
            var deadUnit = _deadQueue.Dequeue();
            _createdUnits[deadUnit.Team].Remove(deadUnit);
            Destroy(deadUnit.gameObject, 5f);
        }
    }
}
