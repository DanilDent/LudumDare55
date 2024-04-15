using Misc;
using TMPro;
using UnityEngine;

public class LevelController : MonoSingleton<LevelController>
{
    public Transform LevelContainer;
    public TextMeshProUGUI LvlText;
    public ReactiveProperty<int> Speed = new ReactiveProperty<int>();

    public AstarPath pathfinder;

    [SerializeField] private Spawner[] spawners;

    private void Start()
    {
        pathfinder.Scan();
        spawners = FindObjectsOfType<Spawner>();
        foreach (var spawner in spawners)
        {
            spawner.StartSpawning();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        var gameController = GameController.Instance;
        var lvlPfab = gameController.CurrentLvlPrefab;
        LvlText.text = $"{(gameController.CurrenLevel + 1).ToString()}/{gameController.LevelsHolder.Levels.Length}";
        var lvlInstance = Instantiate(lvlPfab, LevelContainer);

        Speed.AddListener(OnSpeedChanged);
        SetSpeed(0);
    }

    protected override void OnDestroy()
    {
        Speed.RemoveListener(OnSpeedChanged);
        base.OnDestroy();
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
