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
        
        public virtual bool InitBaseTaskSchedule(BaseTaskAgent agent) => baseSchedule.InitTaskSchedule(agent);

        public virtual void StartExecuteBaseSchedule(int startIndex)
        {
            _ = ExecuteScheduleAsync(baseSchedule, startIndex);
        }

        protected async Awaitable ExecuteScheduleAsync(TaskSchedule taskSchedule, int startIndex)
        {
            _currentSchedule = taskSchedule;
            await taskSchedule.ExecuteSchedule(startIndex);
        }

        public virtual async Awaitable CancelScheduleAsync()
        {
            await _currentSchedule.CancelSchedule();
            _currentSchedule = null;
        }

        public virtual void ContinueBaseTaskSchedule()
        {
            baseSchedule.ContinueTaskSchedule();
        }
        

        public virtual async Awaitable OverrideTaskScheduleAsync(TaskSchedule newSchedule, bool continueBaseScheduleOnCompletion = false)
        {
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
