using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelResultsController : MonoSingleton<LevelResultsController>
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private TextMeshProUGUI _btnText;

    private GameController _gameController;

    protected override void Awake()
    {
        base.Awake();
        _gameController = GameController.Instance;
        _content.gameObject.SetActive(false);
    }

    public void ShowWin()
    {
        _content.gameObject.SetActive(true);
        _winText.text = "You win";
        _btnText.text = _gameController.LevelsHolder.Levels.Length == _gameController.CurrenLevel ? "You passed all levels!" : "Next level";
        _button.onClick.RemoveAllListeners();
        LevelController.Instance.SetSpeed(0);
        _button.onClick.AddListener(() => _gameController.OnWin());
    }


    public void ShowLose()
    {
        _content.gameObject.SetActive(true);
        _winText.text = "You lost";
        _btnText.text = "Try again";
        _button.onClick.RemoveAllListeners();
        LevelController.Instance.SetSpeed(0);
        _button.onClick.AddListener(() => _gameController.OnLose());
    }

    public void ShowDraw()
    {
        _content.gameObject.SetActive(true);
        _winText.text = "Draw";
        _btnText.text = "Try again";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => _gameController.OnDraw());
    }
}
