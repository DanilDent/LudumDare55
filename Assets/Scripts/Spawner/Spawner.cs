using Misc;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour, IDamageble, IBulding, IPointerClickHandler
{
    #region testing
    [SerializeField] private GameObject _testEntityPrefab;
    #endregion

    [SerializeField] private SpawnerConfigSO _config;
    [SerializeField] private Transform _entityContainer;

    private bool _canSpawn;
    //private List<Entity> _entitiesInSpawner;

    public ReactiveProperty<int> CurrentResourceCount { get; private set; }
    public ReactiveProperty<int> CurrentHealth { get; private set; }
    public ReactiveProperty<float> CurrentTimeBeforeSpawn { get; private set; }

    public Vector3 Waypoint => transform.position;

    public Membership Membership => _config.Membership;

    private IBulding _currentTarget;
    public IBulding CurrentTarget { get => _currentTarget; set => _currentTarget = value; }
    public Transform CurrentTargetTransform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event Action<IBulding> Clicked;
    public event Action<IBulding> Dead;
    public event Action<GameObject> EntitySpawned;

    private UnitFactory _unitFactory;
    private GlobalConfigHolder _globalConfigHolder;
    private LevelInfoHolder _levelInfoHolder;

    private void Start()
    {
        _unitFactory = UnitFactory.Instance;
        _globalConfigHolder = GlobalConfigHolder.Instance;
        _levelInfoHolder = LevelInfoHolder.Instance;

        var target = _levelInfoHolder.Waypoints.FirstOrDefault(_ => _.Sender.gameObject == gameObject)?.Target.transform;
        CurrentTarget = target.GetComponent<IBulding>();
        GetComponent<EntitiesInBuldingWaypointController>().SetMoveToBuilding(CurrentTarget);
        InitFromConfig();
    }

    private void InitFromConfig()
    {
        CurrentHealth = new(_config.SpawnerHealth);
        CurrentResourceCount = new(_config.MaxResourceCount);
        CurrentTimeBeforeSpawn = new(_config.TimeToSpawn);
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

        CurrentTimeBeforeSpawn.Value -= Time.deltaTime;

        if (CurrentTimeBeforeSpawn.Value <= 0)
        {
            CurrentTimeBeforeSpawn.Value = _config.TimeToSpawn;
            TrySpawn();
        }
    }

    public void StartSpawning()
    {
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
            var entity = _unitFactory.Create(_entityContainer, transform.position + Vector3.right, _config.Team, _config.UnitToSpawn);
            var target = _levelInfoHolder.Waypoints.FirstOrDefault(_ => _.Sender.gameObject == gameObject)?.Target.transform;

            EntitySpawned?.Invoke(entity.gameObject);
        }
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
        if (CurrentResourceCount.Value < _config.SpawnCostInResources)
        {
            return false;
        }

        if (CurrentHealth.Value <= 0)
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

    public void MoveEntitiesToNewWaypoint(Vector3 waypoint)
    {

        Debug.Log("Move from spawner");
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void MoveEntitiesToNewTarget(IBulding target)
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
}