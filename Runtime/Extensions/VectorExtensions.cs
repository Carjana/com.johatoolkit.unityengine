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
        
        public static Vector3 SetX(this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }
        
        public static Vector3 SetY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }
        
        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }
    }
}
