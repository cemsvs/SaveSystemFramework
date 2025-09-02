using System;

namespace SaveSystemFramework.Examples
{
    [Serializable]
    public struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"({X:F1}, {Y:F1}, {Z:F1})";
        }

        public static Vector3 Zero => new Vector3(0, 0, 0);
    }
}