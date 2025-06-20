using Sirenix.OdinInspector;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Audio
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "JoHaToolkit/Sound/SoundData")]
    public class SoundDataAsset : ScriptableObject
    {
        [SerializeField] private SoundData soundData;
        [SerializeField] private bool randomPitch;
        [SerializeField, ShowIf(nameof(randomPitch)), MinMaxSlider(-3,3)] private Vector2 overridePitch;

        public SoundData GetSoundData()
        {
            SoundBuilder soundBuilder = new(soundData);
            if(randomPitch)
                soundBuilder.WithRandomPitch(overridePitch.x, overridePitch.y);
            return soundBuilder.Build();
        }
    }
}