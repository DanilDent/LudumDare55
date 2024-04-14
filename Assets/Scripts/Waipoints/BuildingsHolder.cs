using Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class BuildingsHolder : MonoSingleton<BuildingsHolder>
{
    private List<IBulding> _buldings = new();

    public event Action<IBulding> OnBuildingClick;
    public event Action<IBulding> OnBuldingDead;

    protected override void Awake()
    {
        base.Awake();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<IBulding>(out var bulding))
            {
                _buldings.Add(bulding);
                bulding.Clicked += OnBuildingClicked;
                bulding.Dead += OnBuldingDie;
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var buldings in _buldings)
        {
            buldings.Clicked -= OnBuildingClicked;
        }
    }

    private void OnBuildingClicked(IBulding bulding)
    {
        OnBuildingClick?.Invoke(bulding);
    }

    private void OnBuldingDie(IBulding bulding)
    {
        OnBuldingDead?.Invoke(bulding);
    }

    public IBulding GetNearestBuldingByPosition(IBulding searchFrom)
    {
        Membership membership;

        if (searchFrom.Membership == Membership.Player)
        {
            membership = Membership.Enemy;
        }
        else 
        {
            membership = Membership.Player;
        }

        var searchList = _buldings.Where(b => b.Membership == membership).ToList();

        IBulding returnBulding = searchList[0];
        float minDistance = Vector2.Distance(searchFrom.Waypoint, returnBulding.Waypoint);

        foreach(var bulding in searchList)
        {
            float distance = Vector2.Distance(searchFrom.Waypoint, bulding.Waypoint);

            if (distance < minDistance)
            {
                minDistance = distance;
                returnBulding = bulding;
            }
        }

        return returnBulding;
    }
}