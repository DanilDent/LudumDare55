using Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Merger : MonoBehaviour, IBuilding, IPointerClickHandler
{
    #region testing
    [SerializeField] private GameObject _testEntityPrefab;
    #endregion

    [SerializeField] private MergerConfigSO _config;
    [SerializeField] private Transform _entityContainer;

    private Misc.KeyValuePair<UnitSO, int>[] _unitsInMerger;
    //private List<Entity> _entitiesInMergerForMerge;
    //private List<Entity> _entitiesInMergerAfterMerge

    public ReactiveProperty<int> CurrentResourceCount { get; private set; }
    public Vector3 Waypoint => transform.position;

    public Membership Membership => _config.Membership;

    private IBuilding _currentTarget;
    public IBuilding CurrentTarget { get => _currentTarget; set => _currentTarget = value; }
    public Transform CurrentTargetTransform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TeamEnum Team => throw new NotImplementedException();

    public event Action<IBuilding> Clicked;
    public event Action<IBuilding> Dead;
    public event Action<GameObject> EntitySpawned;

    private void Start()
    {
        InitFromConfig();
    }

    private void InitFromConfig()
    {
        CurrentResourceCount = new(_config.MaxResourceCount);
        _unitsInMerger = _config.MergeReciptConfigSO.RecipeInfo.Input.Select(k => new Misc.KeyValuePair<UnitSO, int>(k)).ToArray();
    }

    private void TrySpawn()
    {
        if (CurrentResourceCount.Value < _config.SpawnCostInResources)
        {
            return;
        }

        for (int i = 0; i < _unitsInMerger.Length; i++)
        {
            if (_unitsInMerger[i].Value < _config.MergeReciptConfigSO.RecipeInfo.Input[i].Value)
            {
                return;
            }
        }

        CurrentResourceCount.Value -= _config.SpawnCostInResources;

        for (int i = 0; i < _unitsInMerger.Length; i++)
        {
            _unitsInMerger[i].Value -= _config.MergeReciptConfigSO.RecipeInfo.Input[i].Value;
        }

        for (int i = 0; i < _config.MergeReciptConfigSO.RecipeInfo.Output.Value; i++)
        {
            var entity = UnitFactory.Instance.Create(GlobalConfigHolder.Instance.PlayerEntitiesContainer, 
                transform.position + Vector3.right / 4, 
                Team,
                _config.MergeReciptConfigSO.RecipeInfo.Output.Key);
            //_entitiesInMergerAfterMerge.Add(entity);
            EntitySpawned?.Invoke(entity.gameObject);
        }
    }

    public void AddUnit(UnitComp unit)
    {
        Misc.KeyValuePair<UnitSO, int> keyValuePair = _unitsInMerger.FirstOrDefault(k => k.Key == unit.UnitSO);

        if (keyValuePair.Key == null)
        {
            return;
        }

        UnitFactory.Instance.Destroy(unit);

        keyValuePair.Value++; 

        TrySpawn();
    }

    public bool IsSelecteble()
    {
        if (CurrentResourceCount.Value < _config.SpawnCostInResources)
        {
            return false;
        }

        if (_config.Membership == Membership.Enemy)
        {
            return false;
        }

        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(this);
    }

    public void MoveEntitiesToNewWaypoint(Vector3 waypoint)
    {
        Debug.Log("Moving from merger");
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void MoveEntitiesToNewTarget(IBuilding target)
    {
        CurrentTarget = target;
        foreach (Transform entityTransform in _entityContainer.transform)
        {
            var unitComp = entityTransform.GetComponent<UnitComp>();
            if (CurrentTarget.GetTransform() != null)
            {
                unitComp.RemoveTarget(CurrentTarget.GetTransform());
            }
            CurrentTarget = target;
            unitComp.PrependTarget(CurrentTarget.GetTransform());
        }
    }

    public void InvokeDead(IBuilding building)
    {
        Dead?.Invoke(building);
    }
}