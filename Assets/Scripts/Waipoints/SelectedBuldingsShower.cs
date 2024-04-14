using UnityEngine;

[DefaultExecutionOrder(10)]
public sealed class SelectedBuldingsShower : MonoBehaviour
{
    private BuildingsHolder _waypointsHolder;
    private BackClickDetector _backClickDetector;
    private IBuilding _selected;
    private IBuilding _selectedTarget;

    private bool _firstBuldingSelected;

    private void Start()
    {
        _waypointsHolder = BuildingsHolder.Instance;
        _backClickDetector = BackClickDetector.Instance;

        _backClickDetector.OnBackClick += OnBackClick;
        _waypointsHolder.OnBuildingClick += OnBuldingClicked;
    }

    private void OnDestroy()
    {
        _backClickDetector.OnBackClick -= OnBackClick;
        _waypointsHolder.OnBuildingClick -= OnBuldingClicked;
    }

    private void OnBuldingClicked(IBuilding bulding)
    {
        _waypointsHolder.DisableAllSelecetedSpriteOnBuldings();

        if (bulding.Team == TeamEnum.Player && bulding.IsEnoughResourcesToSpawn == false)
        {
            return;
        }

        if (_firstBuldingSelected)
        {
            _firstBuldingSelected = false;

            if (bulding.IsSelecteble() == false || _selected == bulding || _selected.Team == TeamEnum.Enemy)
            {
                return;
            }

            _selectedTarget = bulding;
        }
        else
        {
            _selected = bulding;
            _selectedTarget = bulding.CurrentTarget;
            _firstBuldingSelected = true;
        }

        _selected.SelectedSprite.color = Color.grey;
        _selected.SelectedSprite.gameObject.SetActive(true);
        _selectedTarget.SelectedSprite.color = Color.green;
        _selectedTarget.SelectedSprite.gameObject.SetActive(true);
    }

    private void OnBackClick()
    {
        _waypointsHolder.DisableAllSelecetedSpriteOnBuldings();
        _selected = null;
        _selectedTarget = null;
        _firstBuldingSelected = false;
    }
}