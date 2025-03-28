namespace JohaToolkit.UnityEngine.Extensions
{
    public static class MathExtensions
    {
        public static float IntervalRemap(this float number, float lowerLimit, float upperLimit, float newLowerLimit, float newUpperLimit)
        {
            float scale = (number - lowerLimit) / (upperLimit - lowerLimit);
            return newLowerLimit + (newUpperLimit - newLowerLimit) * scale;
        }
    }

}
