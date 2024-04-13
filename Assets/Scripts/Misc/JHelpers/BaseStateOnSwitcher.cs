namespace JHelpers
{
    public abstract class BaseStateOnSwitcher : IBaseState
    {
        protected IStateSwitcher _switcher;

        public BaseStateOnSwitcher(IStateSwitcher switcher)
        {
            _switcher = switcher;
        }

        public abstract void Enter();

        public abstract void Exit();
    }
}