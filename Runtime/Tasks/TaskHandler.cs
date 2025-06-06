using System;
using System.Threading;
using JohaToolkit.UnityEngine.ScriptableObjects.Logging;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    [Serializable]
    public class TaskHandler
    {
        [SerializeField] protected JoHaLogger logger; 
        [SerializeReference] protected TaskBase[] tasks;
        [SerializeField] protected bool loop;
        public TaskBase[] Tasks => tasks;
        public bool IsLooping => loop;
        public int CurrentTaskIndex { get; private set; } = 0;
        public bool IsExecutingTasks { get; private set; }
        
        public event Action<TaskBase, bool> TaskStarted;
        public event Action<TaskBase, bool> TaskCompleted;
        public event Action<TaskBase> TaskCancelled;
        
        
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
            if (startIndex < 0 || startIndex >= tasks.Length || IsExecutingTasks)
                return;

            IsExecutingTasks = true;
            CurrentTaskIndex = startIndex-1;
            while ((loop || CurrentTaskIndex < tasks.Length - 1) && IsExecutingTasks)
            {
                CurrentTaskIndex++;
                if (CurrentTaskIndex >= tasks.Length)
                    CurrentTaskIndex = 0;

                TaskBase currentTaskBase = tasks[CurrentTaskIndex];
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

            IsExecutingTasks = false;
        }

        public void CancelTask()
        {
            if (!IsExecutingTasks)
                return;
            IsExecutingTasks = false;
            _cts?.Cancel();
        }

        public void ContinueTasks()
        {
            if (IsExecutingTasks) 
                return;
            logger.LogInfo($"Continuing Task {CurrentTaskIndex}");
            _ = ExecuteTasks(CurrentTaskIndex);
        }
    }
}
