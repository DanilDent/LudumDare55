using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JHelpers
{
    public abstract class BaseStateMachineOnSwitchers : MonoBehaviour, IStateSwitcher
    {
        protected List<BaseStateOnSwitcher> _states = new();
        protected BaseStateOnSwitcher _currentState;

        public abstract void Init();
        
        public void SwitchState<T>() where T : BaseStateOnSwitcher
        {
            var state = _states.FirstOrDefault(s => s is T);
            _currentState.Exit();
            state.Enter();
            _currentState = state;
        }
    }
}