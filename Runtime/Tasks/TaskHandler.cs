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
        [SerializeField] protected bool loop;
        public TaskBase[] TaskSchedule => taskSchedule;
        public bool IsLooping => loop;
        
        protected Awaitable<bool> CurrentTask;
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
            } while ((loop || CurrentTaskIndex < taskSchedule.Length - 1) && IsExecutingTasks);

            logger.LogInfo("Execution of tasks completed");
            IsExecutingTasks = false;
        }

        private async Awaitable ExecuteTaskAsync(TaskBase task)
        {
            IsExecutingTasks = true;
            bool taskStarted = await task.StartTask();
            TaskStarted?.Invoke(task, taskStarted);
            if (!taskStarted)
            {
                logger.LogWarning($"Task {task} failed to start. Skipping to next task.");
                return;
            }

            Cts?.Dispose();
            Cts = new CancellationTokenSource();
            CurrentTaskCompleted = new AwaitableCompletionSource();
            try
            {
                bool taskCompleted = await task.IsComplete(Cts.Token);
                TaskCompleted?.Invoke(task, taskCompleted);
                if (!taskCompleted)
                {
                    logger.LogWarning($"Task {task} is not complete. Skipping to next task.");
                }
            }
            catch (OperationCanceledException e)
            {
                    
                TaskCancelled?.Invoke(task);
                logger.LogWarning($"Task {task} was cancelled: {e.Message}. Continuing to next task.");
            }
            finally
            {
                CurrentTaskCompleted.SetResult();
            }
        }

        public virtual async Awaitable CancelTaskAsync()
        {
            if (!IsExecutingTasks)
                return;
            IsExecutingTasks = false;
            logger.LogInfo("cancelling Task..");
            Cts?.Cancel();
            await CurrentTaskCompleted.Awaitable;
        }

        public virtual void ContinueTaskSchedule()
        {
            if (IsExecutingTasks) 
                return;
            logger.LogInfo($"Continuing Task {CurrentTaskIndex}");
            _ = ExecuteTaskScheduleAsync(CurrentTaskIndex);
        }

        public virtual async Awaitable OverrideTaskScheduleAsync(TaskBase task)
        {
            IsOverridingSchedule = true;
            await CancelTaskAsync();
            await ExecuteTaskAsync(task);
            IsExecutingTasks = false;
            IsOverridingSchedule = false;
            ContinueTaskSchedule();
        }
    }
}
