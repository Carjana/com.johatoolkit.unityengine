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

        private bool _isPaused;
        
        public SoundData Data { get; private set; }

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
            _isPaused = true;
            _audioSource.Pause();
        }

        public void UnPause()
        {
            _audioSource.UnPause();
            _isPaused = false;
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
            _audioSource.pitch = soundData.pitch;
            _audioSource.panStereo = soundData.stereoPan;
            _audioSource.reverbZoneMix = soundData.reverbZoneMix;            
        }

        private IEnumerator WaitForSoundToStop()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying || (_isPaused && _audioSource.time <= _audioSource.clip.length));
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
            _isPaused = false;
            _audioSource.Stop();
            SoundManager.Instance.ReturnToPool(this);
        }

        private void InvokeSoundEmitterStopped()
        {
            _soundEmitterStopped?.Invoke();
            foreach (Action soundEmitterStoppedListener in _soundEmitterStoppedListeners)
            {
                SoundEmitterStopped -= soundEmitterStoppedListener;
            }
        }
    }
}