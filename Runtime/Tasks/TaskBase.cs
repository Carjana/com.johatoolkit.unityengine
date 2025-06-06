using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    [Serializable]
    public abstract class TaskBase
    {
        public abstract bool Init(BaseTaskAgent agent);
        public virtual async Awaitable<bool> StartTask()
        {
            return await Task.FromResult(true);
        }
        public abstract Awaitable<bool> IsComplete(CancellationToken cancellationToken);
    }
}