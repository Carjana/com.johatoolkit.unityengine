using JohaToolkit.UnityEngine.Extensions;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Audio
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "JoHaToolkit/Sound/SoundData")]
    public class SoundDataAsset : ScriptableObject
    {
        [SerializeField] private SoundData[] soundData;

        public SoundData GetSoundData()
        {
            return soundData.Random();
        }
    }
}