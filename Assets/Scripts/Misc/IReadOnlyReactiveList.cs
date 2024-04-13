using System;
using System.Collections.Generic;

namespace Misc
{
    public interface IReadOnlyReactiveList<T>
    {
        T this[int i] { get; set; }

        int Count { get; }

        void AddListener(Action<IReadOnlyCollection<T>> action, bool fireInit = true);
        bool Contains(T element);
        IEnumerator<T> GetEnumerator();
        void RemoveAllListeners();
        void RemoveListener(Action<IReadOnlyCollection<T>> action);
    }
}