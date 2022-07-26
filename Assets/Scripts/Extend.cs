using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public static class Extend
    {
        private static readonly Random RAND = new();
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
        
        public static int GetRandInt(int startInclusive, int endExclusive)
        {
            return RAND.Next(startInclusive, endExclusive);
        }

        public static  T GetRandom<T>(this IEnumerable<T> set)
        {
            IEnumerable<T> enumerable = set as T[] ?? set.ToArray();
            return enumerable.ElementAt(RAND.Next(enumerable.Count()));
        }

        public static HashSet<T> GetRandomN<T>(this HashSet<T> set, int numElements)
        {
            return set.OrderBy(_ => RAND.Next()).Take(numElements).ToHashSet();
        }
    }
}