using System;
using UnityEngine;

public interface IBuilding

{
    public event Action<IBuilding> Dead;
    public event Action<IBuilding> Clicked;
    public event Action<GameObject> EntitySpawned;

    public Membership Membership { get; }
    public TeamEnum Team { get; }
    public Vector3 Waypoint { get; }
    public bool IsSelecteble();
    public void MoveEntitiesToNewWaypoint(Vector3 waypoint);
    public void MoveEntitiesToNewTarget(IBuilding target);
    public IBuilding CurrentTarget { get; set; }
    public Transform CurrentTargetTransform { get; set; }
    public Transform GetTransform();
}