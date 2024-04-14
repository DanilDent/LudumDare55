using UnityEngine;

public class TestGameController : MonoBehaviour
{
    [SerializeField] private bool _checkBoxForValidate;

    [SerializeField] private Spawner[] spawners;

    [ExecuteInEditMode]
    private void OnValidate()
    {
        spawners = FindObjectsOfType<Spawner>();
    }

    private void Start()
    {
        foreach (var spawner in spawners)
        {
            spawner.StartSpawning();
        }
    }
}
