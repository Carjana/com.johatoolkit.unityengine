namespace JohaToolkit.UnityEngine.DataStructures.StateMachine
{
    public interface IState
    {
        public void EnterState(IState from);
        public void ExitState(IState to);
        public void UpdateState();
    }
}
