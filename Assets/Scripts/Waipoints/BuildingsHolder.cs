using Misc;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class BuildingsHolder : MonoSingleton<BuildingsHolder>
{
    public List<IBuilding> Buildings => _buildings;

    [SerializeField] private List<IBuilding> _buildings = new();

    public event Action<IBuilding> OnBuildingClick;
    public event Action<IBuilding> OnBuldingDead;

    protected override void Awake()
    {
        base.Awake();

        IBuilding[] buldings = GetComponentsInChildren<IBuilding>();
        foreach (IBuilding bulding in buldings)
        {
            _buildings.Add(bulding);
            bulding.Clicked += OnBuildingClicked;
            bulding.Dead += OnBuldingDie;

            if (bulding is Spawner)
            {
                bulding.GetTransform().GetComponent<HealthComp>().OnDied += HandleOnDie;
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        foreach (var bulding in _buildings)
        {
            bulding.Clicked -= OnBuildingClicked;
            bulding.Dead -= OnBuldingDie;
        }
    }

    private void OnBuildingClicked(IBuilding bulding)
    {
        OnBuildingClick?.Invoke(bulding);
    }

    private void OnBuldingDie(IBuilding bulding)
    {
        OnBuldingDead?.Invoke(bulding);
    }

    private void HandleOnDie(HealthComp healthComp)
    {
        healthComp.OnDied -= HandleOnDie;
        var building = healthComp.GetComponent<IBuilding>();
        _buildings.Remove(building);
        var buildingGO = building.GetTransform().gameObject;
        buildingGO.gameObject.SetActive(false);
        building?.InvokeDead(building);
        Destroy(buildingGO, 5f);
    }

    public IBuilding GetNearestBuldingByPosition(IBuilding searchFrom)
    {
        Membership opponentMembership;

        if (searchFrom.Membership == Membership.Player)
        {
            opponentMembership = Membership.Enemy;
        }
        else
        {
            opponentMembership = Membership.Player;
        }

        var searchList = new List<IBuilding>();

        for (int i = 0; i < _buildings.Count; i++)
        {
            var b = _buildings[i];

            if (b == null || b?.GetTransform() == null || b?.GetTransform()?.GetComponent<HealthComp>() == null)
            {
                continue;
            }

            if (b.Membership == opponentMembership && !b.GetTransform().GetComponent<HealthComp>().IsDead)
            {
                searchList.Add(b);
            }
        }

        //var searchList = _buildings.Where(b => b.Membership == opponentMembership && !b.GetTransform().GetComponent<HealthComp>().IsDead).ToList();

        if (searchList.Count == 0)
        {
            return null;
        }

        IBuilding returnBulding = searchList[0];
        float minDistance = Vector2.Distance(searchFrom.Waypoint, returnBulding.Waypoint);

        foreach (var bulding in searchList)
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

    public void DisableAllSelecetedSpriteOnBuldings()
    {
        _buildings.ForEach(b => b.SelectedSprite.gameObject.SetActive(false));
    }

    //public IBuilding GetNearestOpponentBuldingByPosition(Transform searchFrom, TeamEnum searcherTeam)
    //{
    //    //TeamEnum opponentTeam

    //    Membership opponentMembership;

    //    if (searchFrom.Membership == Membership.Player)
    //    {
    //        opponentMembership = Membership.Enemy;
    //    }
    //    else
    //    {
    //        opponentMembership = Membership.Player;
    //    }

    //    var searchList = _buildings.Where(b => b.Membership == opponentMembership && !b.GetTransform().GetComponent<HealthComp>().IsDead).ToList();

    //    IBuilding returnBulding = searchList[0];
    //    float minDistance = Vector2.Distance(searchFrom.Waypoint, returnBulding.Waypoint);

    //    foreach (var bulding in searchList)
    //    {
    //        float distance = Vector2.Distance(searchFrom.Waypoint, bulding.Waypoint);

    //        if (distance < minDistance)
    //        {
    //            minDistance = distance;
    //            returnBulding = bulding;
    //        }
    //    }

    //    return returnBulding;
    //}
}