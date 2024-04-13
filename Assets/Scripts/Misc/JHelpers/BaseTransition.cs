namespace JHelpers
{
    public abstract class BaseTransition
    {
        protected IBaseState _from;
        protected IBaseState _to;

        public BaseTransition(IBaseState from, IBaseState to)
        {
            _from = from;
            _to = to;
        }
    }
}