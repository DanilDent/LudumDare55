using System;
using System.Collections;
using System.Collections.Generic;

namespace Misc
{
    public class ReactiveList<T> : IEnumerable<T>, IReadOnlyReactiveList<T>
    {
        private List<T> _list;

        public ReactiveList()
        {
            _list = new List<T>();
        }

        public ReactiveList(List<T> list)
        {
            _list = list;
        }

        public void Add(T element)
        {
            _list.Add(element);
            _action?.Invoke(_list);
        }

        public bool Remove(T element)
        {
            bool result = _list.Remove(element);
            _action?.Invoke(_list);
            return result;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            _list.AddRange(collection);
            _action?.Invoke(_list);
        }

        public void Clear()
        {
            _list.Clear();
            _action?.Invoke(_list);
        }

        public bool Contains(T element)
        {
            return _list.Contains(element);
        }

        public int Count => _list.Count;

        public T this[int i]
        {
            get => _list[i];
            set => _list[i] = value;
        }

        public void AddListener(Action<IReadOnlyCollection<T>> action, bool fireInit = true)
        {
            _action += action;
            if (fireInit)
            {
                action?.Invoke(_list);
            }
        }

        public void RemoveListener(Action<IReadOnlyCollection<T>> action)
        {
            _action -= action;
        }

        public void RemoveAllListeners()
        {
            _action = null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public List<T> GetList()
        {
            return _list;
        }

        private Action<IReadOnlyCollection<T>> _action;
    }
}
