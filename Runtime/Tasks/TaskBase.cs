using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    [Serializable]
    public abstract class TaskBase
    {
        [SerializeField] public string taskName;
        public abstract bool Init(BaseTaskAgent agent);
        public abstract Awaitable<bool> StartTask();
        public abstract Awaitable<bool> IsComplete(CancellationToken cancellationToken);
        public override string ToString() => taskName;
    }
}