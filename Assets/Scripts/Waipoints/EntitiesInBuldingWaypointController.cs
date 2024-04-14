using UnityEngine;

public sealed class EntitiesInBuldingWaypointController : MonoBehaviour
{
    private BuildingsHolder _waypointsHolder;
    private IBulding _startBulding;
    private IBulding _moveToBulding;

    private bool _isStartBuldingSelected;

    private void Start()
    {
        _waypointsHolder = BuildingsHolder.Instance;
        _startBulding = GetComponent<IBulding>();

        _startBulding.EntitySpawned += OnEnitySpawned;
        _waypointsHolder.OnBuildingClick += OnBuldingClicked;
        _waypointsHolder.OnBuldingDead += OnDestroyBuldingMoveTo;
    }

    private void OnDestroy()
    {
        _startBulding.EntitySpawned -= OnEnitySpawned;
        _waypointsHolder.OnBuildingClick -= OnBuldingClicked;
        _waypointsHolder.OnBuldingDead -= OnDestroyBuldingMoveTo;
    }

    private void OnBuldingClicked(IBulding bulding)
    {
        if (_startBulding.Membership == Membership.Enemy)
        {
            return;
        }

        if (_startBulding == bulding)
        {
            _isStartBuldingSelected = true;
            return;
        }

        if (_isStartBuldingSelected == false)
        {
            return;
        }

        if (bulding.IsSelecteble())
        {
            _moveToBulding = bulding;
            _startBulding.MoveEntitiesToNewWaypoint(bulding.Waypoint);
        }

        _isStartBuldingSelected = false;
    }

    private void OnDestroyBuldingMoveTo(IBulding bulding)
    {
        if (_startBulding == bulding)
        {
            return;
        }

        if (_moveToBulding == bulding)
        {
            _moveToBulding = _waypointsHolder.GetNearestBuldingByPosition(_startBulding);
            _startBulding.MoveEntitiesToNewWaypoint(_moveToBulding.Waypoint);
        }
    }

    private void OnEnitySpawned(GameObject entity)
    {
        Debug.Log("Spawned");
        //entity.moveTo(_movetoBulding.Waypoint);
    }
}