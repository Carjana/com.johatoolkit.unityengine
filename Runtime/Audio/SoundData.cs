using System;
using JohaToolkit.UnityEngine.Extensions;
using Sirenix.OdinInspector;
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
        public bool randomPitch;
        [MinMaxSlider(MinPitch, MaxPitch, true), ShowIf(nameof(randomPitch))] public Vector2 pitchRange = new(0, 0);
        [HideIf(nameof(randomPitch))]public float defaultPitch = 1;
        public float Pitch
        {
            get
            {
                float pitchVal = randomPitch ? pitchRange.RandomRange() : defaultPitch;
                if(pitchVal == 0)
                    pitchVal = Mathf.Epsilon;
                return pitchVal;
            }
        }

        [Range(-1, 1)] public float stereoPan;
        [Range(0, 1.1f)] public float reverbZoneMix = 1f;
    }
}
