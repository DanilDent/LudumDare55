using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoSingleton<GameController>
{
    public int CurrenLevel => _currentLvlIndex;
    public GameObject CurrentLvlPrefab => _levelsHolder.Levels[_currentLvlIndex];

    [SerializeField] private LevelsHolder _levelsHolder;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private bool _gameStarted;
    private int _currentLvlIndex;

    private void Start()
    {
        SceneManager.LoadScene(SceneName.Loading.ToString());
    }

    private void Update()
    {
        if (_gameStarted)
        {
            return;
        }

        if (Input.anyKey)
        {
            _gameStarted = true;
            SceneManager.LoadScene(SceneName.Game.ToString());
        }
    }

    public enum SceneName
    {
        Bootstrap = 0,
        Game = 1,
        Loading = 2,
    }
}
