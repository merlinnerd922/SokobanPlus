using UnityEngine;

namespace DefaultNamespace
{
    public static class Extend
    {
        public static bool IsOdd(this int i)
        {
            return i % 2 == 1;
        }
        
        public static Bounds GetMaxBounds(this GameObject g) {
            var b = new Bounds(g.transform.position, Vector3.zero);
            foreach (Renderer r in g.GetComponentsInChildren<Renderer>()) {
                b.Encapsulate(r.bounds);
            }
            return b;
        }

        public static void SetHeight(this Camera c, int height)
        {
            var transform = c.transform;
            var transformPosition = transform.position;
            transform.position = new Vector3(transformPosition.x, height, transformPosition.z);
        }

        public static void SetRotation(this Camera c, int x, int y, int z)
        {
            c.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}