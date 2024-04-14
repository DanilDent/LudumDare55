﻿using UnityEngine;

public sealed class EntitiesInBuldingWaypointController : MonoBehaviour
{
    private BuildingsHolder _waypointsHolder;
    private IBuilding _startBulding;
    private IBuilding _moveToBulding;

    private bool _isStartBuldingSelected;

    private void Start()
    {
        _waypointsHolder = BuildingsHolder.Instance;
        _startBulding = GetComponent<IBuilding>();

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

    public void SetMoveToBuilding(IBuilding target)
    {
        _moveToBulding = target;
    }

    private void OnBuldingClicked(IBuilding bulding)
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
            _startBulding.MoveEntitiesToNewTarget(bulding);
            //_startBulding.MoveEntitiesToNewWaypoint(bulding.Waypoint);
        }

        _isStartBuldingSelected = false;
    }

    public void OnDestroyBuldingMoveTo(IBuilding building)
    {
        if (_moveToBulding == building)
        {
            _moveToBulding = _waypointsHolder.GetNearestBuldingByPosition(_startBulding);
            _startBulding.MoveEntitiesToNewTarget(_moveToBulding);
            _startBulding.MoveEntitiesToNewWaypoint(_moveToBulding.Waypoint);
        }
    }

    private void OnEnitySpawned(GameObject entity)
    {
        Debug.Log("Spawned");
        entity.GetComponent<UnitComp>().AddTarget(_moveToBulding.GetTransform());
        //entity.moveTo(_movetoBulding.Waypoint);
    }
}