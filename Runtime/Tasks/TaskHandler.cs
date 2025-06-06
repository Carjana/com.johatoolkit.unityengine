using System;
using System.Threading;
using JohaToolkit.UnityEngine.ScriptableObjects.Logging;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    [Serializable]
    public class TaskHandler
    {
        [SerializeField] private JoHaLogger logger; 
        [SerializeReference] public TaskBase[] tasks;
        [SerializeField] private bool loop;

        public event Action<TaskBase, bool> TaskStarted;
        public event Action<TaskBase, bool> TaskCompleted;
        public event Action<TaskBase> TaskCancelled;
        
        private int _currentTaskIndex = 0;
        private bool _isExecutingTasks;
        private CancellationTokenSource _cts;

        public bool ValidateTasks(BaseTaskAgent agent)
        {
            foreach (TaskBase taskBase in tasks)
            {
                if (taskBase.Init(agent)) 
                    continue;
                logger.LogError($"Task validation failed: {taskBase}. Does the agent implement every required interface?");
                return false;
            }

            return true;
        }
        
        public async Awaitable ExecuteTasks(int startIndex)
        {
            if (startIndex < 0 || startIndex >= tasks.Length)
                return;

            _isExecutingTasks = true;
            _currentTaskIndex = startIndex-1;
            while ((loop || _currentTaskIndex < tasks.Length - 1) && _isExecutingTasks)
            {
                _currentTaskIndex++;
                if (_currentTaskIndex >= tasks.Length)
                    _currentTaskIndex = 0;

                TaskBase currentTaskBase = tasks[_currentTaskIndex];
                bool taskStarted = await currentTaskBase.StartTask();
                TaskStarted?.Invoke(currentTaskBase, taskStarted);
                if (!taskStarted)
                {
                    logger.LogWarning($"Task {currentTaskBase} failed to start. Skipping to next task.");
                    continue;
                }

                _cts?.Dispose();
                _cts = new CancellationTokenSource();
                try
                {
                    bool taskCompleted = await currentTaskBase.IsComplete(_cts.Token);
                    TaskCompleted?.Invoke(currentTaskBase, taskCompleted);
                    if (!taskCompleted)
                    {
                        logger.LogWarning($"Task {currentTaskBase} is not complete. Skipping to next task.");
                        continue;
                    }
                }
                catch (OperationCanceledException e)
                {
                    TaskCancelled?.Invoke(currentTaskBase);
                    logger.LogWarning($"Task {currentTaskBase} was cancelled: {e.Message}. Continuing to next task.");
                    continue;
                }
            }
        }

        public void CancelTask()
        {
            if (!_isExecutingTasks)
                return;
            _isExecutingTasks = false;
            _cts?.Cancel();
        }

        public void ContinueTasks()
        {
            if (_isExecutingTasks) 
                return;
            logger.LogInfo($"Continuing Task {_currentTaskIndex}");
            _ = ExecuteTasks(_currentTaskIndex);
        }
    }
}
