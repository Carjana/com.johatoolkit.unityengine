using UnityEngine;

namespace JohaToolkit.UnityEngine.Tasks
{
    public class BaseTaskAgent : MonoBehaviour
    {
        [SerializeField] protected TaskHandler taskHandler;

        private void Awake()
        {
            taskHandler.ValidateTasks(this);
        }
    }
}