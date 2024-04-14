using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBulding
{
    public event Action<IBulding> Dead;
    public event Action<IBulding> Clicked;
    public event Action<GameObject> EntitySpawned;

    public Membership Membership { get; }
    public Vector3 Waypoint { get; }
    public bool IsSelecteble();
    public void MoveEntitiesToNewWaypoint(Vector3 waypoint);
}