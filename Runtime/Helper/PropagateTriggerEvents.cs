using Sirenix.OdinInspector;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Helper
{
    public class PropagateTriggerEvents : MonoBehaviour
    {
        [SerializeField] private bool propagateToParent;

        [SerializeField, InfoBox("if true, this component assumes, that the parent doesn't change and caches it in Start")] private bool cacheParent;

        private IPropagateTriggerEvents _cachedParent;
        
        private void Start()
        {
            if (cacheParent)
            {
                _cachedParent = GetComponentInParent<IPropagateTriggerEvents>();
                if(_cachedParent == null)
                    Debug.LogError("PropagateCollisionEvents: No parent with IPropagateCollisionEvents found. Make sure to set cacheParent to false if you don't want to cache the parent.");
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!TryGetParent(out IPropagateTriggerEvents target)) 
                return;
            target.OnTriggerEnter(other);
        }

        public void OnTriggerStay(Collider other)
        {
            if (!TryGetParent(out IPropagateTriggerEvents target))
                return;
            target.OnTriggerStay(other);
        }

        public void OnTriggerExit(Collider other)
        {
            if (!TryGetParent(out IPropagateTriggerEvents target))
                return;
            target.OnTriggerExit(other);
        }

        private bool TryGetParent(out IPropagateTriggerEvents target)
        {
            target = cacheParent ? _cachedParent : transform.GetComponentInParent<IPropagateTriggerEvents>();
            return target != null;
        }
    }
    
    public interface IPropagateTriggerEvents
    {
        public void OnTriggerEnter(Collider other){}
        public void OnTriggerExit(Collider other){}
        public void OnTriggerStay(Collider other){}
    }
}
