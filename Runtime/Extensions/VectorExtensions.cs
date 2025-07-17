using UnityEngine;

namespace JohaToolkit.UnityEngine.Extensions
{
    public static class VectorExtensions
    {
        public static float RandomRange(this Vector2 vector) => Random.Range(vector.x, vector.y);
        public static Vector2 ClampVectorValues(this Vector2 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            vector.y = Mathf.Clamp(vector.y, min, max);
            return vector;
        }
    }
}
