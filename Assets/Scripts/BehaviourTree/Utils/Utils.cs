using System.Collections;
using System.Collections.Generic;

namespace Ravenholm.Tools.BehaviourTree.Utils
{
    public static class Utils
    {
        public static System.Random random = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + random.Next(n - i);
                T t = list[r];
                list[r] = list[i];
                list[i] = t;
            }
        }
    }
}