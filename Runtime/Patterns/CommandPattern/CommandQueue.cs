using System;
using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.Patterns.CommandPattern
{
    public class CommandQueue : CommandQueue<object>
    {
        public ICommand Peek()
        {
            return _queue.Peek() as ICommand;
        }
    }
    public class CommandQueue<T>
    {
        protected Queue<ICommand<T>> _queue;
        
        public int Count => _queue?.Count ?? 0;

        public void AddCommand(ICommand<T> command)
        {
            _queue ??= new Queue<ICommand<T>>();
            _queue.Enqueue(command);
        }

        public bool ExecuteNext(T parameter)
        {
            _queue.TryDequeue(out ICommand<T> command);
            if (command == null)
                return false;
            command.Execute(parameter);
            return true;
        }
        
        public bool ExecuteNext()
        {
            if (_queue.TryPeek(out ICommand<T> command) && command is not ICommand)
            {
                throw new InvalidOperationException("Cannot execute command without parameter when the command requires a parameter. Use ExecuteNext(T parameter) instead.");
            }
            return ExecuteNext(default);
        }

        public virtual ICommand<T> Peek() => _queue.Peek();
        
    }
}