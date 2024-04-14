using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfigSO", menuName = "Spawner Config")]
public class SpawnerConfigSO : BaseSpawnConfigSO
{
    [field: SerializeField] public float TimeToSpawn { get; private set; }
    [field: SerializeField] public int SpawnerHealth { get; private set; }
}