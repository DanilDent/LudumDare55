namespace JHelpers
{
    public interface IStateSwitcher
    {
        public void SwitchState<T>() where T : BaseStateOnSwitcher;
    }
}