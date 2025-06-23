using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    public class BaseTaskAgent : MonoBehaviour
    {
        [SerializeField] protected TaskHandler taskHandler;

        protected virtual void Awake()
        {
            taskHandler.InitBaseTaskSchedule(this);
        }
    }
}