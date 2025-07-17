using System.Collections.Generic;
using System.Linq;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.ScriptableObjects.Logging;
using UnityEngine;
using UnityEngine.Pool;

namespace JohaToolkit.UnityEngine.Audio
{
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {
        [SerializeField] private JoHaLogger logger;
        [SerializeField] private SoundEmitter emitterPrefab;
        public List<SoundEmitter> ActiveSoundEmitters { get; } = new();

        private IObjectPool<SoundEmitter> _emitterPool;

        [Header("Pool Settings")]
        [SerializeField] private bool collectionCheck;
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxSize = 100;
        [SerializeField] private int maxFrequentSounds = 15;

        protected override void Awake()
        {
            IsPersistent = true;
            base.Awake();
            InitializePool();
        }

        private void InitializePool()
        {
            _emitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnGetSoundEmitter,
                OnReleaseSoundEmitter,
                OnDestroySoundEmitter,
                collectionCheck,
                defaultCapacity,
                maxSize
            );
        }

        public bool CanPlaySound(SoundData soundData)
        {
            if (!soundData.isFrequentSound)
                return true;

            if (ActiveSoundEmitters.Count(s => s.Data.isFrequentSound) < maxFrequentSounds)
                return true;
            
            try
            {
                ActiveSoundEmitters.First(s => s.Data.isFrequentSound).Stop();
                return true;
            }
            catch
            {
                logger?.LogWarning("Sound is already released");
            }

            return false;
        }
        
        public void ReturnToPool(SoundEmitter emitter)
        {
            _emitterPool.Release(emitter);
        }

        public void StopAll()
        {
            for (int i = ActiveSoundEmitters.Count - 1; i >= 0; i--)
            {
                ActiveSoundEmitters[i].Stop();
            }
        }

        public SoundEmitter Get()
        {
            return _emitterPool.Get();
        }

        public SoundEmitter Play(SoundData soundData, Vector3 position = default, Transform parent = null)
        {
            SoundEmitter emitter = Get();
            emitter.Play(soundData, position, parent ?? transform);
            return emitter;
        }

        public SoundEmitter Play(SoundDataAsset soundDataAsset, Vector3 position = default, Transform parent = null)
        {
            return Play(soundDataAsset.GetSoundData(), position, parent ?? transform);
        }

        private void OnDestroySoundEmitter(SoundEmitter obj)
        {
            Destroy(obj.gameObject);
        }

        private void OnReleaseSoundEmitter(SoundEmitter obj)
        {
            ActiveSoundEmitters.Remove(obj);
            obj.transform.position = transform.position;
            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(false);
        }

        private void OnGetSoundEmitter(SoundEmitter obj)
        {
            ActiveSoundEmitters.Add(obj);
            obj.gameObject.SetActive(true);
        }

        private SoundEmitter CreateSoundEmitter()
        {
            SoundEmitter emitter = Instantiate(emitterPrefab, transform);
            emitter.gameObject.SetActive(false);
            emitter.Initialize();
            return emitter;
        }
    }
}
