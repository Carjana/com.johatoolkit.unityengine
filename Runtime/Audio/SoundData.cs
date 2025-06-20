using System;
using UnityEngine;
using UnityEngine.Audio;

namespace JohaToolkit.UnityEngine.Audio
{
    [Serializable]
    public class SoundData
    {
        public const float MinPitch = -3f;
        public const float MaxPitch = 3f;
        
        public AudioClip clip;
        public AudioMixerGroup output;
        public bool isFrequentSound;
        public bool bypassEffects;
        public bool bypassListenerEffects;
        public bool bypassReverbZones;
        public bool playOnAwake;
        public bool loop;

        [Range(0,256)] public int priority = 128;
        [Range(0, 1)] public float volume = 1f;
        [Range(MinPitch, MaxPitch)] public float pitch = 1f;
        [Range(-1, 1)] public float stereoPan;
        [Range(0, 1.1f)] public float reverbZoneMix = 1f;
    }
}
