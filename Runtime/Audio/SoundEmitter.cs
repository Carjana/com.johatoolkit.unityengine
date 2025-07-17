using System;
using System.Collections;
using System.Collections.Generic;
using JohaToolkit.UnityEngine.Extensions;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Audio
{
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Coroutine _playingCoroutine;

        public bool IsPaused { get; private set; }
        public SoundData Data { get; private set; }
        public float Time => _audioSource.time;

        private List<Action> _soundEmitterStoppedListeners = new();
        private event Action _soundEmitterStopped;
        public event Action SoundEmitterStopped
        {
            add
            {
                _soundEmitterStopped += value;
                _soundEmitterStoppedListeners.Add(value);
            }
            remove
            {
                _soundEmitterStopped -= value;
                _soundEmitterStoppedListeners.Remove(value);
            }
        }

        public void Initialize()
        {
            _audioSource = this.GetOrAddComponent<AudioSource>();
        }
        
        public void Play(SoundData soundData, Vector3 position, Transform parent)
        {
            if(_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }
            
            transform.position = position;
            transform.SetParent(parent);
            
            SetAudioData(soundData);
            
            _audioSource.Play();
            _playingCoroutine = StartCoroutine(WaitForSoundToStop());
        }

        public void Pause()
        {
            IsPaused = true;
            _audioSource.Pause();
        }

        public void UnPause()
        {
            _audioSource.UnPause();
            IsPaused = false;
        }
        
        private void SetAudioData(SoundData soundData)
        {
            Data = soundData;
            _audioSource.clip = soundData.clip;
            _audioSource.outputAudioMixerGroup = soundData.output;
            _audioSource.bypassEffects = soundData.bypassEffects;
            _audioSource.bypassListenerEffects = soundData.bypassListenerEffects;
            _audioSource.bypassReverbZones = soundData.bypassReverbZones;
            _audioSource.playOnAwake = soundData.playOnAwake;
            _audioSource.loop = soundData.loop;
            _audioSource.priority = soundData.priority;
            _audioSource.volume = soundData.volume;
            _audioSource.pitch = soundData.Pitch;
            _audioSource.panStereo = soundData.stereoPan;
            _audioSource.reverbZoneMix = soundData.reverbZoneMix;            
        }

        private IEnumerator WaitForSoundToStop()
        {
            yield return new WaitWhile(() => _audioSource.loop || _audioSource.time <= _audioSource.clip.length);
            Stop();
        }
        
        public void Stop()
        {
            InvokeSoundEmitterStopped();
            if(_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }
            
            _playingCoroutine = null;
            IsPaused = false;
            _audioSource.Stop();
            SoundManager.Instance.ReturnToPool(this);
        }

        private void InvokeSoundEmitterStopped()
        {
            _soundEmitterStopped?.Invoke();
            for (int i = _soundEmitterStoppedListeners.Count - 1; i >= 0; i--)
            {
                SoundEmitterStopped -= _soundEmitterStoppedListeners[i];
            }
        }
    }
}