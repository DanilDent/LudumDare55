using UnityEngine;
using UnityEngine.UI;

namespace JHelpers.UI
{
    public class ToggleSwitchMaterialChange : ToggleSwitch
    {
        [Header("Background")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Material _backgroundMaterial;
        private Material _localCopyOfBackgroundMaterial;

        [Space]
        [SerializeField] private Image _handleImage;
        [SerializeField] private Material _handleMaterial;
        private Material _localCopyOfHandleMaterial;

        private bool _isBackgroundImageNotNull;
        private bool _isHandleImageNotNull;

        private bool _isBackgroundMaterialNotNull;
        private bool _isHandleMaterialNotNull;

        protected override void OnValidate()
        {
            base.OnValidate();

            SetupBackgroundMaterial();
            SetupHandleMaterial();

            TransitionImages();
        }

        public override void Init(bool state)
        {
            base.Init(state);
            TransitionEffect += TransitionImages;

            _isHandleMaterialNotNull = _handleImage.material != null;
            _isBackgroundMaterialNotNull = _backgroundImage.material != null;
            _isHandleImageNotNull = _handleImage != null;
            _isBackgroundImageNotNull = _backgroundImage != null;

            SetupBackgroundMaterial();
            SetupHandleMaterial();
            TransitionImages();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TransitionEffect -= TransitionImages;
        }

        private void SetupHandleMaterial()
        {
            _localCopyOfHandleMaterial = new Material(_handleMaterial);

            if (_isHandleImageNotNull)
                _handleImage.material = _localCopyOfHandleMaterial;
        }

        private void SetupBackgroundMaterial()
        {
            _localCopyOfBackgroundMaterial = new Material(_backgroundMaterial);

            if (_isBackgroundImageNotNull)
                _backgroundImage.material = _localCopyOfBackgroundMaterial;
        }

        private void TransitionImages()
        {
            if (_isBackgroundImageNotNull && _isBackgroundMaterialNotNull)
                _backgroundImage.material.SetFloat("_MixValue", SliderValue);

            if (_isHandleImageNotNull && _isHandleMaterialNotNull)
                _handleImage.material.SetFloat("_MixValue", SliderValue);
        }
    }
}




