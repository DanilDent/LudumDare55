using Misc;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Merger : MonoBehaviour, IBuilding, IPointerClickHandler
{
    #region testing
    [SerializeField] private GameObject _testEntityPrefab;
    #endregion

    [SerializeField] private MergerConfigSO _config;
    [SerializeField] private Transform _entityContainer;

    private int _entitiesInMerger;

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
    }

    private void TrySpawn()
    {
        if (CurrentResourceCount.Value < _config.SpawnCostInResources)
        {
            return;
        }

        if (_entitiesInMerger < _config.MergeReciptConfigSO.EntitysCountToMerge)
        {
            return;
        }

        CurrentResourceCount.Value -= _config.SpawnCostInResources;

        for (int i = 0; i < _config.EntitySpawnCountPerSpawn; i++)
        {
            var entity = Instantiate(_testEntityPrefab, transform.position + Vector3.right, Quaternion.identity, _entityContainer);
            //_entitiesInMergerAfterMerge.Add(entity);
            EntitySpawned?.Invoke(entity);
        }

        //destroy entities requier for merger
        //
        // for (int i = 0; i < _entitiesInMerger; i++)
        // {
        //     Destroy(_entitiesInMerger[0]);
        // }

        _entitiesInMerger = 0;
    }

    public void AddEntity()
    {
        _entitiesInMerger++;
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