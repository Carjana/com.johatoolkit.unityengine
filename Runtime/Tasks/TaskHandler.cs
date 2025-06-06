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
        [SerializeReference] protected TaskBase[] taskSchedule;
        public TaskBase[] TaskSchedule => taskSchedule;
        [SerializeField] protected bool loop;
        public bool IsLooping => loop;
        public int CurrentTaskIndex { get; private set; }
        public bool IsExecutingTasks { get; private set; }
        public bool IsOverridingSchedule { get; private set; }
        
        public event Action<TaskBase, bool> TaskStarted;
        public event Action<TaskBase, bool> TaskCompleted;
        public event Action<TaskBase> TaskCancelled;
        
        protected AwaitableCompletionSource CurrentTaskCompleted;
        
        protected CancellationTokenSource Cts;
        
        public virtual bool ValidateTasks(BaseTaskAgent agent)
        {
            foreach (TaskBase taskBase in taskSchedule)
            {
                if (taskBase.Init(agent)) 
                    continue;
                logger.LogError($"Task validation failed: {taskBase}. Does the agent implement every required interface?");
                return false;
            }

            return true;
        }
        
        public virtual async Awaitable ExecuteTaskScheduleAsync(int startIndex)
        {
            if (startIndex < 0 || startIndex >= taskSchedule.Length || IsExecutingTasks)
                return;
            
            CurrentTaskIndex = startIndex-1;
            do
            {
                CurrentTaskIndex++;
                if (CurrentTaskIndex >= taskSchedule.Length)
                    CurrentTaskIndex = 0;

                await ExecuteTaskAsync(taskSchedule[CurrentTaskIndex]);
                if (Cts.IsCancellationRequested)
                    break;
            } while ((loop || CurrentTaskIndex < taskSchedule.Length - 1) && !IsOverridingSchedule);

            logger.LogInfo("Task Schedule completed/cancelled");
        }

        private async Awaitable ExecuteTaskAsync(TaskBase task)
        {
            Cts?.Dispose();
            Cts = new CancellationTokenSource();
            CurrentTaskCompleted = new AwaitableCompletionSource();
            
            bool taskStarted = await task.StartTask();
            TaskStarted?.Invoke(task, taskStarted);
            IsExecutingTasks = taskStarted;
            
            if (!taskStarted)
            {
                logger.LogWarning($"Task {task} failed to start");
                CurrentTaskCompleted?.SetResult();
                return;
            }

            try
            {
                bool taskCompleted = await task.IsComplete(Cts.Token);
                TaskCompleted?.Invoke(task, taskCompleted);
                if (!taskCompleted)
                {
                    logger.LogWarning($"Task {task} is not complete");
                }
            }
            catch (OperationCanceledException e)
            {
                    
                TaskCancelled?.Invoke(task);
                logger.LogWarning($"Task {task} was cancelled");
            }
            finally
            {
                CurrentTaskCompleted.SetResult();
                IsExecutingTasks = false;
            }
        }

        public virtual async Awaitable CancelTaskAsync()
        {
            if (!IsExecutingTasks)
                return;
            logger.LogInfo("Cancelling current task...");
            Cts?.Cancel();
            await CurrentTaskCompleted.Awaitable;
            logger.LogInfo("Current task cancelled");
        }

        public virtual void ContinueTaskSchedule()
        {
            if (IsExecutingTasks)
                return;
            logger.LogInfo($"Continuing task schedule (TaskIndex: {CurrentTaskIndex})...");
            _ = ExecuteTaskScheduleAsync(CurrentTaskIndex);
        }

        public virtual async Awaitable OverrideTaskScheduleAsync(TaskBase task)
        {
            logger.LogInfo($"Overriding CurrentTask with Task {task}...");
            IsOverridingSchedule = true;
            if (IsExecutingTasks)
                await CancelTaskAsync();
            await ExecuteTaskAsync(task);
            IsOverridingSchedule = false;
            logger.LogInfo("Task schedule Override completed");
        }
    }
}
