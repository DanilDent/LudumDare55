using Misc;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Transform LevelContainer;

    public ReactiveProperty<int> Speed = new ReactiveProperty<int>();

    [SerializeField] private Spawner[] spawners;

    private void Start()
    {
        spawners = FindObjectsOfType<Spawner>();
        foreach (var spawner in spawners)
        {
            spawner.StartSpawning();
        }
    }

    private void Awake()
    {
        var gameController = GameController.Instance;
        var lvlPfab = gameController.CurrentLvlPrefab;
        var lvlInstance = Instantiate(lvlPfab, LevelContainer);

        Speed.AddListener(OnSpeedChanged);
        SetSpeed(0);
    }

    private void OnDestroy()
    {
        Speed.RemoveListener(OnSpeedChanged);
    }

    public void Pause(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }

    public void SetSpeed(int speed)
    {
        if (speed != 0 && speed != 1 && speed != 2)
        {
            return;
        }

        Time.timeScale = speed;
    }

    private void OnSpeedChanged(int value)
    {
        Time.timeScale = value;
    }
}
