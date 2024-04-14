using Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Merger : MonoBehaviour, IBuilding, IPointerClickHandler
{

    [SerializeField] private MergerConfigSO _config;
    [SerializeField] private Transform _entityContainer;
    [SerializeField] private Sprite _sprite;

    private Misc.KeyValuePair<UnitSO, int>[] _unitsInMerger;

    public ReactiveProperty<int> CurrentResourceCount { get; private set; }
    public Vector3 Waypoint => transform.position;

    public Membership Membership => _config.Membership;

    private IBuilding _currentTarget;
    public IBuilding CurrentTarget { get => _currentTarget; set => _currentTarget = value; }

    public TeamEnum Team => _config.Team;

    public SpriteRenderer SelectedSprite { get; set; }

    public bool IsEnoughResourcesToSpawn => CurrentResourceCount.Value >= _config.SpawnCostInResources;

    public event Action<IBuilding> Clicked;
    public event Action<IBuilding> Dead;
    public event Action<GameObject> EntitySpawned;

    private List<UnitComp> _createdUnits = new List<UnitComp>();

    private void Start()
    {
        InitFromConfig();
    }

    private void InitFromConfig()
    {
        CurrentResourceCount = new(_config.MaxResourceCount);
        _unitsInMerger = _config.MergeReciptConfigSO.RecipeInfo.Input.Select(k => new Misc.KeyValuePair<UnitSO, int>(k)).ToArray();

        var target = LevelInfoHolder.Instance.Waypoints.FirstOrDefault(_ => _.Sender.gameObject == base.gameObject)?.Target.transform;
        CurrentTarget = target.GetComponent<IBuilding>();
        GetComponent<EntitiesInBuldingWaypointController>().SetMoveToBuilding(CurrentTarget);

        var newGameObject = new GameObject();
        newGameObject.transform.SetParent(transform);
        newGameObject.SetActive(false);
        newGameObject.transform.position = transform.position;
        newGameObject.transform.localScale = new Vector3(.6f, .6f, 0);
        var spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _sprite;
        spriteRenderer.sortingOrder = 3;
        SelectedSprite = spriteRenderer;
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
            var teamContainer = GlobalConfigHolder.Instance.GetTeamUnitsContaienr(_config.Team);
            var entity = UnitFactory.Instance.Create(teamContainer,
                transform.position + Vector3.right,
                Team,
                _config.MergeReciptConfigSO.RecipeInfo.Output.Key);
            _createdUnits.Add(entity);
            entity.HealthComp.OnDied += HandleOnUnitDied;

            EntitySpawned?.Invoke(entity.gameObject);
        }
    }

    private void HandleOnUnitDied(HealthComp comp)
    {
        comp.OnDied -= HandleOnUnitDied;
        _createdUnits.Remove(comp.UnitComp);
    }

    public void AddUnit(UnitComp unit)
    {
        Misc.KeyValuePair<UnitSO, int> keyValuePair = _unitsInMerger.FirstOrDefault(k => k.Key == unit.UnitSO);

        if (keyValuePair == default)
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

    public Transform GetTransform()
    {
        return transform;
    }

    public void MoveEntitiesToNewTarget(IBuilding target)
    {
        for (int i = 0; i < _createdUnits.Count; i++)
        {
            var unitComp = _createdUnits[i];
            if (CurrentTarget?.GetTransform() != null)
            {
                unitComp.RemoveTarget(CurrentTarget?.GetTransform());
            }
            unitComp.PrependTarget(target?.GetTransform());
        }
        CurrentTarget = target;

    }

    public void InvokeDead(IBuilding building)
    {
        Dead?.Invoke(building);
    }
}