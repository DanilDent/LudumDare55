using UnityEngine;
using Misc;
using System;
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

    public event Action<IBulding> Clicked;
    public event Action<IBulding> Dead;
    public event Action<GameObject> EntitySpawned;

    private void Start()
    {
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
            var entity = Instantiate(_testEntityPrefab, transform.position + Vector3.right, Quaternion.identity, _entityContainer);
            //_entitiesInSpawner.Add(entity);
            EntitySpawned?.Invoke(entity);
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
}