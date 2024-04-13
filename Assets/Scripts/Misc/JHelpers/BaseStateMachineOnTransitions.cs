using System.Collections.Generic;
using UnityEngine;

namespace JHelpers
{
    public abstract class BaseStateMachineOnTransitions : MonoBehaviour
    {
        protected List<BaseTransition> _transitions = new();

        public abstract void Init();
    }
}