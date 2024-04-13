using UnityEngine;
using UnityEngine.UI;

namespace JHelpers.UI
{
    public class ToggleSwitchColorChange : ToggleSwitch
    {
        [Header("Elements to Recolor")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _handleImage;

        [Space]
        [SerializeField] private bool _recolorBackground;
        [SerializeField] private bool _recolorHandle;

        [Header("Colors")]
        [SerializeField] private Color _backgroundColorOff = Color.white;
        [SerializeField] private Color _backgroundColorOn = Color.white;
        [Space]
        [SerializeField] private Color _handleColorOff = Color.white;
        [SerializeField] private Color _handleColorOn = Color.white;

        private bool _isBackgroundImageNotNull;
        private bool _isHandleImageNotNull;

        protected override void OnValidate()
        {
            base.OnValidate();

            CheckForNull();
            ChangeColors();
        }

        public override void Init(bool state)
        {
            base.Init(state);

            TransitionEffect += ChangeColors;
            CheckForNull();
            ChangeColors();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            TransitionEffect -= ChangeColors;
        }

        private void CheckForNull()
        {
            _isBackgroundImageNotNull = _backgroundImage != null;
            _isHandleImageNotNull = _handleImage != null;
        }


        private void ChangeColors()
        {
            if (_recolorBackground && _isBackgroundImageNotNull)
                _backgroundImage.color = Color.Lerp(_backgroundColorOff, _backgroundColorOn, SliderValue);

            if (_recolorHandle && _isHandleImageNotNull)
                _handleImage.color = Color.Lerp(_handleColorOff, _handleColorOn, SliderValue);
        }
    }
}