using System;

namespace Misc
{
    [Serializable]
    public class ReactiveProperty<T> : IReadOnlyReactiveProperty<T>
    {
        public ReactiveProperty() { }
        public ReactiveProperty(T value)
        {
            _value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                _action?.Invoke(value);
            }
        }

        public void AddListener(Action<T> action, bool fireInit = true)
        {
            _action += action;
            if (fireInit)
            {
                action?.Invoke(_value);
            }
        }

        public void RemoveListener(Action<T> action)
        {
            _action -= action;
        }

        public void RemoveAllListeners()
        {
            _action = null;
        }

        private T _value;
        private Action<T> _action;
    }
}