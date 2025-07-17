using JohaToolkit.UnityEngine.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace JohaToolkit.UnityEngine.Audio
{
    public class SoundBuilder
    {
        private readonly SoundData _soundData;
        public SoundBuilder(SoundData baseData)
        {
            _soundData = baseData;
        }

        public SoundBuilder(AudioClip clip, AudioMixerGroup audioMixerGroup)
        {
            _soundData = new SoundData()
            {
                clip = clip,
                output = audioMixerGroup,
            };
        }

        public SoundBuilder WithRandomPitch(float minPitch, float maxPitch)
        {
            _soundData.randomPitch = true;
            _soundData.pitchRange = new Vector2(minPitch, maxPitch).ClampVectorValues(SoundData.MinPitch, SoundData.MaxPitch);
            return this;
        }
        
        public SoundData Build()
        {
            return _soundData;
        }
    }
}