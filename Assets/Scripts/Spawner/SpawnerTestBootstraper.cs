using UnityEngine;

public sealed class SpawnerTestBootstraper : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Merger _merger;

    [SerializeField] private Spawner _spawnerToKill;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _spawner.StartSpawning();
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            _spawner.PauseSpawning();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            //_merger.AddUnit();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _spawnerToKill.TakeDamage(999999);
        }
    }
}