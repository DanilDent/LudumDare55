using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoSingleton<GameController>
{
    public LevelsHolder LevelsHolder => _levelsHolder;

    public int CurrenLevel => _currentLvlIndex;
    public GameObject CurrentLvlPrefab => _levelsHolder.Levels[_currentLvlIndex];

    [SerializeField] private LevelsHolder _levelsHolder;

    public string Message = "";

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

    public void OnWin()
    {
        if (_currentLvlIndex == _levelsHolder.Levels.Length - 1)
        {
            _currentLvlIndex = 0;
            SceneManager.LoadScene(SceneName.Loading.ToString());
            return;
        }
        _currentLvlIndex++;
        SceneManager.LoadScene(SceneName.Game.ToString());
    }

    public void OnLose()
    {
        SceneManager.LoadScene(SceneName.Game.ToString());
    }

    public void OnDraw()
    {

    }

    public enum SceneName
    {
        Bootstrap = 0,
        Game = 1,
        Loading = 2,
    }
}
