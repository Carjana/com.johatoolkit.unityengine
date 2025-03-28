using System;
using System.Collections.Generic;

namespace JohaToolkit.Unity.Extensions
{
    public static class ListExtensions
    {
        private static Random Rand = new Random();
        public static void FisherYatesShuffle<T>(this IList<T> items)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items));

            for (int i = 0; i < items.Count; i++)
            {
                int j = Rand.Next(i, items.Count);

                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }

        public static void Swap<T>(this IList<T> items, int indexA, int indexB)
        {
            if(items == null)
                throw new ArgumentNullException();
            
            if(indexA < 0 || indexA >= items.Count)
                throw new ArgumentOutOfRangeException($"IndexA ({indexA}) is out of Range!");
            if (indexB < 0 || indexB >= items.Count)
                throw new ArgumentOutOfRangeException($"IndexB ({indexB}) is out of Range!");

            T temp = items[indexA];
            items[indexA] = items[indexB];
            items[indexB] = temp;
        }
    }
}
