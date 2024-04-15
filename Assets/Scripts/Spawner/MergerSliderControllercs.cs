using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(5)]
public class MergerSliderControllercs : MonoBehaviour
{
    [SerializeField] private Slider _resourcesSlider;
    [SerializeField] private TMP_Text _resourcesText;
    [SerializeField] private TMP_Text _unitsInMergerCountText;
    [SerializeField] private TMP_Text _unitsOutputFromMergerText;

    [SerializeField] private Merger _merger;

    private int _maxResources;
    private int _requireUnits;

    private void Start()
    {
        _maxResources = _merger.CurrentResourceCount.Value;

        _resourcesSlider.maxValue = _maxResources;
        _resourcesSlider.value = _resourcesSlider.maxValue;
        _resourcesText.text = _maxResources + "/" + _maxResources;
        _requireUnits = _merger.UnitsRequireForMerge;
        _unitsInMergerCountText.text = 0 + "/" + _requireUnits;
        _unitsOutputFromMergerText.text = _merger.UnitsSpawnAfterMerge.ToString();

        _merger.UnitAddedToMerge += OnUnitAddedForMerge;
        _merger.CurrentResourceCount.AddListener(OnResourcesChanged);
    }

    private void OnDestroy()
    {
        _merger.UnitAddedToMerge -= OnUnitAddedForMerge;
        _merger.CurrentResourceCount.RemoveListener(OnResourcesChanged);
    }

    private void OnResourcesChanged(int resource)
    {
        _resourcesSlider.value = resource;
        _resourcesText.text = resource + "/" + _maxResources;
    }

    private void OnUnitAddedForMerge(int unitsCount)
    {
        _unitsInMergerCountText.text = unitsCount + "/" + _requireUnits;
    }
}