using UnityEngine;
using UnityEditor;

namespace SoodUtils
{
    public static class SoodGizmos
    {
#if UNITY_EDITOR
        public static void WireDisc2D(Vector3 position, float radius)
        {
            Handles.DrawWireDisc(position, Vector3.back, radius);
        }

        public static void WireDisc(Vector3 position, Vector3 direction, float radius)
        {
            Handles.DrawWireDisc(position, direction, radius);
        }
#endif
    }
}