using System;

namespace Misc
{
    public interface IReadOnlyReactiveProperty<T>
    {
        T Value { get; }

        void AddListener(Action<T> action, bool fireInit = true);
        void RemoveListener(Action<T> action);
    }
}