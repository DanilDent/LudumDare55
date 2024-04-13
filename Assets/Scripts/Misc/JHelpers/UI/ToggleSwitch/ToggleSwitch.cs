using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JHelpers.UI
{
    public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
    {
        [Header("Slider setup")]
        [SerializeField, Range(0, 1f)]
        protected float SliderValue;
        public bool CurrentValue { get; private set; }

        private bool _previousValue;
        private Slider _slider;

        [Header("Animation")]
        [SerializeField, Range(0, 1f)] private float _animationDuration = 0.5f;
        [SerializeField]
        private AnimationCurve _slideEase =
            AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Coroutine _animateSliderCoroutine;

        [Header("Events")]
        [SerializeField] private UnityEvent _onToggleOn;
        [SerializeField] private UnityEvent _onToggleOff;

        private ToggleSwitchGroupManager _toggleSwitchGroupManager;

        protected Action TransitionEffect;

        public event Action OnToggleOn;
        public event Action OnToggleOff;

        protected virtual void OnValidate()
        {
            SetupToggleComponents();

            _slider.value = SliderValue;
        }

        public virtual void Init(bool state)
        {
            CurrentValue = state;
            SetupSliderComponent();
        }

        protected virtual void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void SetupToggleComponents()
        {
            if (_slider != null)
                return;

            SetupSliderComponent();
        }

        private void SetupSliderComponent()
        {
            _slider = GetComponent<Slider>();

            if (_slider == null)
            {
                Debug.Log("No slider found!", this);
                return;
            }

            _slider.interactable = false;
            var sliderColors = _slider.colors;
            sliderColors.disabledColor = Color.white;
            _slider.colors = sliderColors;
            _slider.transition = Selectable.Transition.None;
            _slider.value = CurrentValue ? 1 : 0;
        }

        public void SetupForManager(ToggleSwitchGroupManager manager)
        {
            _toggleSwitchGroupManager = manager;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Toggle();
        }

        private void Toggle()
        {
            if (_toggleSwitchGroupManager != null)
                _toggleSwitchGroupManager.ToggleGroup(this);
            else
                SetStateAndStartAnimation(!CurrentValue);
        }

        public void ToggleByGroupManager(bool valueToSetTo)
        {
            SetStateAndStartAnimation(valueToSetTo);
        }

        private void SetStateAndStartAnimation(bool state)
        {
            _previousValue = CurrentValue;
            CurrentValue = state;

            if (_previousValue != CurrentValue)
            {
                if (CurrentValue)
                {
                    _onToggleOn?.Invoke();
                    OnToggleOn?.Invoke();
                }
                else
                {
                    _onToggleOff?.Invoke();
                    OnToggleOff?.Invoke();
                }
            }

            if (_animateSliderCoroutine != null)
                StopCoroutine(_animateSliderCoroutine);

            _animateSliderCoroutine = StartCoroutine(AnimateSlider());
        }

        private IEnumerator AnimateSlider()
        {
            float startValue = _slider.value;
            float endValue = CurrentValue ? 1 : 0;

            float time = 0;
            
            if (_animationDuration > 0)
            {
                while (time < _animationDuration)
                {
                    time += Time.deltaTime;

                    float lerpFactor = _slideEase.Evaluate(time / _animationDuration);
                    _slider.value = SliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                    TransitionEffect?.Invoke();

                    yield return null;
                }
            }

            _slider.value = endValue;
        }
    }
}