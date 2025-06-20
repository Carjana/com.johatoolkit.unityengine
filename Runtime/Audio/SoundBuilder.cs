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
            _soundData.pitch = Random.Range(minPitch, maxPitch);
            _soundData.pitch = Mathf.Clamp(_soundData.pitch, SoundData.MinPitch, SoundData.MaxPitch);
            return this;
        }
        
        public SoundData Build()
        {
            if (_soundData.pitch == 0)
                _soundData.pitch = Mathf.Epsilon;
            return _soundData;
        }
    }
}