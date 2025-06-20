using UnityEngine;

namespace JohaToolkit.UnityEngine.Extensions
{
    public static class AudioExtensions
    {
        public static float ToLogarithmicValue(float sliderValue)
        {
            return Mathf.Log10(Mathf.Max(0.001f, sliderValue))*20f;
        }
    }
}
