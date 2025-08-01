using Sirenix.OdinInspector;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Helper
{
    public class PropagateCollisionEvents : MonoBehaviour, IPropagateCollisionEvents
    {
        [SerializeField] private bool propagateToParent;

        [SerializeField, InfoBox("if true, this component assumes, that the parent doesn't change and caches it in Start")] private bool cacheParent;

        private IPropagateCollisionEvents _cachedParent;
        
        private void Start()
        {
            if (cacheParent)
            {
                _cachedParent = GetComponentInParent<IPropagateCollisionEvents>();
                if(_cachedParent == null)
                    Debug.LogError("PropagateCollisionEvents: No parent with IPropagateCollisionEvents found. Make sure to set cacheParent to false if you don't want to cache the parent.");
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            if (!TryGetParent(out IPropagateCollisionEvents target)) 
                return;
            target.OnCollisionEnter(other);
        }

        public void OnCollisionStay(Collision other)
        {
            if (!TryGetParent(out IPropagateCollisionEvents target))
                return;
            target.OnCollisionStay(other);
        }

        public void OnCollisionExit(Collision other)
        {
            if (!TryGetParent(out IPropagateCollisionEvents target))
                return;
            target.OnCollisionExit(other);
        }

        private bool TryGetParent(out IPropagateCollisionEvents target)
        {
            target = cacheParent ? _cachedParent : transform.GetComponentInParent<IPropagateCollisionEvents>();
            return target != null;
        }
    }

    public interface IPropagateCollisionEvents
    {
        public void OnCollisionEnter(Collision other){}
        public void OnCollisionExit(Collision other){}
        public void OnCollisionStay(Collision other){}
    }
}
