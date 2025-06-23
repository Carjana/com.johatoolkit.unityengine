using System;
using JohaToolkit.UnityEngine.ScriptableObjects.Logging;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    [Serializable]
    public class TaskHandler
    {
        [SerializeField] protected JoHaLogger logger;
        
        [SerializeField] protected TaskSchedule baseSchedule;

        public bool IsOverridingBaseSchedule { get; protected set; }
        private TaskSchedule _currentSchedule;
        private int _activeOverrideCount;

        public event Action<TaskSchedule> OnScheduleStarted;
        public event Action<TaskSchedule> OnScheduleEnded;
        
        public virtual bool InitBaseTaskSchedule(BaseTaskAgent agent) => baseSchedule.InitTaskSchedule(agent);

        public virtual void StartExecuteBaseSchedule(int startIndex)
        {
            _ = ExecuteScheduleAsync(baseSchedule, startIndex);
        }

        protected async Awaitable ExecuteScheduleAsync(TaskSchedule taskSchedule, int startIndex)
        {
            OnScheduleStarted?.Invoke(taskSchedule);
            logger?.LogInfo($"Started Task Schedule: {taskSchedule.ScheduleName}");
            _currentSchedule = taskSchedule;
            await taskSchedule.ExecuteSchedule(startIndex);
            logger?.LogInfo($"Finished Task Schedule: {taskSchedule.ScheduleName}");
            OnScheduleEnded?.Invoke(taskSchedule);
        }

        public virtual async Awaitable CancelScheduleAsync()
        {
            logger?.LogInfo($"Canceling Task Schedule {_currentSchedule.ScheduleName}...");
            await _currentSchedule.CancelSchedule();
            logger?.LogInfo($"Canceling Complete! ({_currentSchedule.ScheduleName})");
            _currentSchedule = null;
        }

        public virtual void ContinueBaseTaskSchedule()
        {
            logger?.LogInfo("Continuing Base Task Schedule...");
            _ = ExecuteScheduleAsync(baseSchedule, baseSchedule.CurrentTaskIndex);
        }
        

        public virtual async Awaitable OverrideTaskScheduleAsync(TaskSchedule newSchedule, bool continueBaseScheduleOnCompletion = false)
        {
            logger?.LogInfo($"Overriding {_currentSchedule.ScheduleName} TaskSchedule with {newSchedule.ScheduleName}...");
            IsOverridingBaseSchedule = true;
            _activeOverrideCount++;
            await CancelScheduleAsync();
            
            await ExecuteScheduleAsync(newSchedule, 0);
            
            _activeOverrideCount--;
            if (_activeOverrideCount != 0)
                return;
            
            IsOverridingBaseSchedule = false;
            if (continueBaseScheduleOnCompletion)
            {
                ContinueBaseTaskSchedule();
            }
        }
    }
}
