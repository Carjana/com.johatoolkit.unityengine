using System;
using System.Threading;
using JohaToolkit.UnityEngine.Extensions;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Timer
{
    public class CountdownTimer
    {
        public bool IsRunning { get; private set; }
        public event Action TimerStarted;

        public event Action TimerFinished;
        
        public TimeSpan RemainingTime{ get; private set; }
        public TimeSpan StartTime { get; private set; }

        public float Progress => Mathf.Clamp01((float)(RemainingTime.TotalSeconds / StartTime.TotalSeconds));

        public CountdownTimer() { }

        public CountdownTimer(TimeSpan startTime) => Start(startTime);

        public void Tick(float deltaTime)
        {
            if (!IsRunning)
                return;
            
            RemainingTime -= deltaTime.Seconds();
            if (RemainingTime <= TimeSpan.Zero)
            {
                Stop();
            }
            
        }
        
        public void Start(TimeSpan startTime)
        {
            if (IsRunning)
                return;
            StartTime = startTime;
            RemainingTime = startTime;
            IsRunning = true;
            TimerStarted?.Invoke();
        }

        public void AddTime(TimeSpan time)
        {
            if(!IsRunning)
                return;
            RemainingTime += time;
            StartTime += time;
        }

        public void Stop()
        {
            if(!IsRunning)
                return;
            IsRunning = false;
            TimerFinished?.Invoke();
        }
        
        public async Awaitable WaitForCompletion(CancellationToken cancellationToken = default)
        {
            while (IsRunning)
            {
                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }
    }
}
