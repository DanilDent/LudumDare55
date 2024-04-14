using UnityEngine;

public abstract class BaseSpawnConfigSO : ScriptableObject
{
    [field: SerializeField] public Membership Membership { get; protected set; }
    [field: SerializeField] public EntityType EntityType { get; protected set; }
    [field: SerializeField] public int MaxResourceCount { get; protected set; }
    [field: SerializeField] public int EntitySpawnCountPerSpawn { get; protected set; }
    [field: SerializeField] public int SpawnCostInResources { get; protected set; }
}