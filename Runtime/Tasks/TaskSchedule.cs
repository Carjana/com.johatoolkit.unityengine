using System;
using System.Threading;
using JohaToolkit.UnityEngine.ScriptableObjects.Logging;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    [Serializable]
    public class TaskSchedule
    {
        [SerializeField] private JoHaLogger logger;
        [SerializeField] private string scheduleName;
        public string ScheduleName => scheduleName;
        [SerializeReference] private TaskBase[] tasks;
        public TaskBase[] Tasks => tasks;
        [SerializeField] private bool loop;
        public bool IsLooping => loop;

        public int CurrentTaskIndex { get; private set; }
        public bool IsExecutingTasks { get; private set; }
        
        private CancellationTokenSource _cts;
        private AwaitableCompletionSource _currentTaskCompleted;

        public bool InitTaskSchedule(BaseTaskAgent agent)
        {
            foreach (TaskBase taskBase in tasks)
            {
                if (taskBase.Init(agent))
                    continue;
                
                logger?.LogError($"Task validation failed: {taskBase}. Does the agent implement every required interface?");
                return false;
            }

            return true;
        }

        public TaskSchedule SetTasks(TaskBase[] newTasks)
        {
            tasks = newTasks;
            return this;
        }

        public TaskSchedule SetLooping(bool shouldLoop)
        {
            loop = shouldLoop;
            return this;
        }

        public async Awaitable ExecuteSchedule(int startIndex)
        {
            IsExecutingTasks = true;
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            
            if (startIndex < 0 || startIndex >= tasks.Length)
                return;
            
            CurrentTaskIndex = startIndex-1;
            do
            {
                CurrentTaskIndex++;
                if (CurrentTaskIndex >= tasks.Length)
                    CurrentTaskIndex = 0;
                
                _currentTaskCompleted = new AwaitableCompletionSource();
                await ExecuteTaskAsync(tasks[CurrentTaskIndex]);
                _currentTaskCompleted.SetResult();
            } while ((loop || CurrentTaskIndex < tasks.Length - 1) && !_cts.IsCancellationRequested);

            IsExecutingTasks = false;
        }

        private async Awaitable ExecuteTaskAsync(TaskBase task)
        {
            bool taskStarted = await task.StartTask();

            if (!taskStarted)
                return;
            
            try
            {
                if (await task.IsComplete(_cts.Token))
                {
                    logger?.LogInfo($"Task {task.taskName} completed");
                    return;
                }
                
            }
            catch (OperationCanceledException)
            {
                
            }
            
            logger?.LogError($"Task {task.taskName} failed/Canceled");
        }
        
        public async Awaitable CancelSchedule()
        {
            if (!IsExecutingTasks)
                return;
            _cts.Cancel();
            await _currentTaskCompleted.Awaitable;
        }
    }
}