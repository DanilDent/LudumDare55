using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(5)]
public class MergerSliderControllercs : MonoBehaviour
{
    [SerializeField] private Slider _resourcesSlider;
    [SerializeField] private TMP_Text _resourcesText;

    [SerializeField] private Merger _merger;

    private int _maxResources;

    private void Start()
    {
        _maxResources = _merger.CurrentResourceCount.Value;

        _resourcesSlider.maxValue = _maxResources;
        _resourcesSlider.value = _resourcesSlider.maxValue;
        _resourcesText.text = _maxResources + "/" + _maxResources;

        _merger.CurrentResourceCount.AddListener(OnResourcesChanged);
    }

    private void OnDestroy()
    {
        _merger.CurrentResourceCount.RemoveListener(OnResourcesChanged);
    }

    private void OnResourcesChanged(int resource)
    {
        _resourcesSlider.value = resource;
        _resourcesText.text = resource + "/" + _maxResources;
    }
}