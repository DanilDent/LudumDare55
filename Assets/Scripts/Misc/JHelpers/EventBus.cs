using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JHelpers
{
    public class EventBus
    {
        private Dictionary<string, List<CallbackWithPriority>> _signalCallbacks = new Dictionary<string, List<CallbackWithPriority>>();
        private Dictionary<string, List<CallbackWithPriority>> _callbacksToDelete = new Dictionary<string, List<CallbackWithPriority>>();
        private Dictionary<string, List<CallbackWithPriority>> _callbacksToAdd = new Dictionary<string, List<CallbackWithPriority>>();

        public void Subscribe<T>(Action<T> callback, int priority = 0) where T : ISignal
        {
            //mb use hashcode or smth else
            string key = typeof(T).Name;

            if (_callbacksToAdd.ContainsKey(key))
            {
                _callbacksToAdd[key].Add(new CallbackWithPriority(priority, callback));
            }
            else
            {
                _callbacksToAdd.Add(key, new() { new CallbackWithPriority(priority, callback) });
            }
        }

        public void Invoke<T>(T signal) where T : ISignal
        {
            string key = typeof(T).Name;

            AddSignals(key);
            DeleteSignals(key);

            if (_signalCallbacks.ContainsKey(key))
            {
                foreach (var obj in _signalCallbacks[key])
                {
                    var callBack = obj.Callback as Action<T>;
                    callBack?.Invoke(signal);
                }
            }
        }

        public void Unsubscribe<T>(Action<T> callback) where T : ISignal
        {
            string key = typeof(T).Name;

            if (_signalCallbacks.ContainsKey(key))
            {
                var callbackToDelete = _signalCallbacks[key].FirstOrDefault(x => x.Callback.Equals(callback));

                if (callbackToDelete != null)
                {
                    if (_callbacksToDelete.ContainsKey(key))
                    {
                        _callbacksToDelete[key].Add(callbackToDelete);
                    }
                    else
                    {
                        _callbacksToDelete.Add(key, new() { callbackToDelete });
                    }
                }
            }
            else
            {
                //Debug.LogErrorFormat("Trying to unsubscribe for not existing key! {0} ", key);
            }
        }

        private void DeleteSignals(string key)
        {
            if (!_callbacksToDelete.ContainsKey(key))
                return;

            foreach (var value in _callbacksToDelete[key])
                _signalCallbacks[key].Remove(value);

            _callbacksToDelete.Remove(key);
        }

        private void AddSignals(string key)
        {
            if (!_callbacksToAdd.ContainsKey(key))
                return;

            foreach (var value in _callbacksToAdd[key])
            {
                if (_signalCallbacks.ContainsKey(key))
                    _signalCallbacks[key].Add(value);
                else
                    _signalCallbacks.Add(key, new() { value });
            }

            _signalCallbacks[key] = _signalCallbacks[key].OrderByDescending(x => x.Priority).ToList();
            _callbacksToAdd.Remove(key);
        }
    }
}