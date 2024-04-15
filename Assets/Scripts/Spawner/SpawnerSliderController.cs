using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(5)]
public class SpawnerSliderController : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Slider _resourcesSlider;
    [SerializeField] private TMP_Text _resourcesText;
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TMP_Text _timerText;
    
    [SerializeField] private Spawner _spawner;

    private int _maxHp;
    private int _maxResources;
    private float _maxTimeSpawn;

    private void Start()
    {
        _maxHp = _spawner.HealthComp.Health.Value;
        _maxResources = _spawner.CurrentResourceCount.Value;
        _maxTimeSpawn = _spawner.CurrentTimeBeforeSpawn.Value;

        _hpSlider.maxValue = _maxHp;
        _hpSlider.value = _hpSlider.maxValue;
        _hpText.text = _maxHp + "/" + _maxHp;
        _resourcesSlider.maxValue = _maxResources;
        _resourcesSlider.value = _resourcesSlider.maxValue;
        _resourcesText.text = _maxResources + "/" + _maxResources;
        _timerSlider.maxValue = _maxTimeSpawn;
        _timerSlider.value = 0;
        _timerText.text = _spawner.CurrentTimeBeforeSpawn + " sec";

        _spawner.HealthComp.Health.AddListener(OnHpChanged);
        _spawner.CurrentTimeBeforeSpawn.AddListener(OnTimerChanged);
        _spawner.CurrentResourceCount.AddListener(OnResourcesChanged);
    }

    private void OnDestroy()
    {
        _spawner.HealthComp.Health.RemoveListener(OnHpChanged);
        _spawner.CurrentTimeBeforeSpawn.RemoveListener(OnTimerChanged);
        _spawner.CurrentResourceCount.RemoveListener(OnResourcesChanged);
    }

    private void OnHpChanged(int hp)
    {
        _hpSlider.value = hp;
        _hpText.text = hp + "/" + _maxHp;
    }

    private void OnResourcesChanged(int resource)
    {
        _resourcesSlider.value = resource;
        _resourcesText.text = resource + "/" + _maxResources;
    }

    private void OnTimerChanged(float time)
    {
        _timerSlider.value = _maxTimeSpawn - time;
        _timerText.text = String.Format("{0:0.00}", time) + " sec";
    }
}