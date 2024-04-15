using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private LevelController _lvlController;

    [SerializeField] private Button[] _buttons;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _lvlController.Speed.AddListener(OnSpeedChanged);

        _buttons[0].onClick.AddListener(OnStopClicked);
        _buttons[1].onClick.AddListener(OnSpeed1xClicked);
        _buttons[2].onClick.AddListener(OnSpeed2xClicked);
    }

    private void Unsubscribe()
    {
        _lvlController.Speed.RemoveListener(OnSpeedChanged);

        _buttons[0].onClick.RemoveListener(OnStopClicked);
        _buttons[1].onClick.RemoveListener(OnSpeed1xClicked);
        _buttons[2].onClick.RemoveListener(OnSpeed2xClicked);
    }

    private void OnStopClicked()
    {
        _lvlController.Speed.Value = 0;
    }

    private void OnSpeed1xClicked()
    {
        _lvlController.Speed.Value = 1;
    }

    private void OnSpeed2xClicked()
    {
        _lvlController.Speed.Value = 2;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void OnSpeedChanged(int value)
    {
        Array.ForEach(_buttons, (button) => { button.GetComponentInChildren<Image>().color = Color.white; });
        _buttons[value].GetComponentInChildren<Image>().color = Color.green;
    }
}
