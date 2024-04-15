using Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour, IDamageble, IBuilding, IPointerClickHandler
{

    [SerializeField] private SpawnerConfigSO _config;
    [SerializeField] private Transform _entityContainer;
    [SerializeField] private Sprite _sprite;

    private bool _canSpawn;
    //private List<Entity> _entitiesInSpawner;

    public ReactiveProperty<int> CurrentResourceCount { get; private set; } = new ReactiveProperty<int>();
    public ReactiveProperty<int> CurrentHealth { get; private set; } = new ReactiveProperty<int>();
    public ReactiveProperty<float> CurrentTimeBeforeSpawn { get; private set; } = new ReactiveProperty<float>();

    public Vector3 Waypoint => transform.position;

    public Membership Membership => _config.Membership;
    public TeamEnum Team => _config.Team;

    private IBuilding _currentTarget;

    public event Action<IBuilding> Clicked;
    public event Action<IBuilding> Dead;
    public event Action<GameObject> EntitySpawned;

    private UnitFactory _unitFactory;
    private GlobalConfigHolder _globalConfigHolder;
    private LevelInfoHolder _levelInfoHolder;

    public HealthComp HealthComp => _healthComp;
    public SpriteRenderer SelectedSprite { get; set; }

    [SerializeField] private HealthComp _healthComp;

    private List<UnitComp> _createdUnits = new List<UnitComp>();

    public bool IsEnoughResourcesToSpawn => CurrentResourceCount.Value >= _config.SpawnCostInResources;

    public IBuilding CurrentTarget { get => _currentTarget; set => _currentTarget = value; }

    private void Start()
    {
        _unitFactory = UnitFactory.Instance;
        _globalConfigHolder = GlobalConfigHolder.Instance;
        _levelInfoHolder = LevelInfoHolder.Instance;

        _healthComp = GetComponent<HealthComp>();
        _healthComp.Construct(_config);

        var target = _levelInfoHolder.Waypoints.FirstOrDefault(_ => _.Sender.gameObject == gameObject)?.Target.transform;
        CurrentTarget = target.GetComponent<IBuilding>();
        GetComponent<EntitiesInBuldingWaypointController>().SetMoveToBuilding(CurrentTarget);
        InitFromConfig();
    }

    private void InitFromConfig()
    {
        CurrentHealth = new(_config.SpawnerHealth);
        CurrentResourceCount = new(_config.MaxResourceCount);
        CurrentTimeBeforeSpawn = new(_config.TimeToSpawn);

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

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (_canSpawn == false)
        {
            return;
        }

        if (CurrentTimeBeforeSpawn == null)
        {
            return;
        }

        CurrentTimeBeforeSpawn.Value -= Time.deltaTime;

        if (CurrentTimeBeforeSpawn.Value <= 0)
        {
            CurrentTimeBeforeSpawn.Value = _config.TimeToSpawn;
            TrySpawn();
        }
    }

    public void StartSpawning()
    {
        //CurrentTimeBeforeSpawn.Value = _config.TimeToSpawn;
        _canSpawn = true;
    }

    public void PauseSpawning()
    {
        _canSpawn = false;
    }

    private void TrySpawn()
    {
        if (CurrentResourceCount.Value < _config.SpawnCostInResources)
        {
            return;
        }

        CurrentResourceCount.Value -= _config.SpawnCostInResources;

        for (int i = 0; i < _config.EntitySpawnCountPerSpawn; i++)
        {
            var teamContainer = _globalConfigHolder.GetTeamUnitsContaienr(_config.Team);
            var entity = _unitFactory.Create(teamContainer, transform.position + Vector3.right, _config.Team, _config.UnitToSpawn);
            _createdUnits.Add(entity);
            entity.HealthComp.OnDied += HandleOnDied;
            var target = _levelInfoHolder.Waypoints.FirstOrDefault(_ => _.Sender.gameObject == gameObject)?.Target.transform;

            EntitySpawned?.Invoke(entity.gameObject);
        }
    }

    private void HandleOnDied(HealthComp comp)
    {
        comp.OnDied -= HandleOnDied;
        _createdUnits.Remove(comp.UnitComp);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth.Value -= damage;

        if (CurrentHealth.Value <= 0)
        {
            Dead?.Invoke(this);
            //if we destroy this GO all units will die instantly
            gameObject.SetActive(false);
        }
    }

    public bool IsSelecteble()
    {
        if (CurrentHealth.Value <= 0)
        {
            return false;
        }

        if (_config.Team == TeamEnum.Enemy)
        {
            return true;
        }

        if (CurrentResourceCount.Value < _config.SpawnCostInResources)
        {
            return false;
        }


        if (_config.Membership == Membership.Player)
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
        CurrentTarget = target;
        for (int i = 0; i < _createdUnits.Count; i++)
        {
            var unitComp = _createdUnits[i];
            if (CurrentTarget?.GetTransform() != null)
            {
                unitComp.RemoveTarget(CurrentTarget?.GetTransform());
            }
            CurrentTarget = target;
            unitComp.PrependTarget(CurrentTarget?.GetTransform());
        }
    }

    public void InvokeDead(IBuilding building)
    {
        Dead?.Invoke(this);
    }
}