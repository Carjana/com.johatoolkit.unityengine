namespace JohaToolkit.UnityEngine.Patterns.CommandPattern
{
    public interface ICommand : ICommand<object>
    {
        void ICommand<object>.Execute(object parameter)
        {
            Execute();
        }

        public void Execute();
    }

    public interface ICommand<in T>
    {
        public void Execute(T parameter);
    }

    public interface IUndoableCommand<in T> : ICommand<T>
    {
        public void Undo(T parameter);
    }

    public interface IUndoableCommand : ICommand
    {
        public void Undo();
    }
}
