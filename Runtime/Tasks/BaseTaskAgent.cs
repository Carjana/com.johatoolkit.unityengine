using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    public class BaseTaskAgent : MonoBehaviour
    {
        [SerializeField] protected TaskHandler taskHandler;

        protected void Awake()
        {
            taskHandler.ValidateTasks(this);
        }
    }
}