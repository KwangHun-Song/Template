using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawLine {
    public static class CollectionExtensions {
        public static IEnumerable<T> Reversed<T>(this IList<T> originList) {
            for (int i = originList.Count - 1; i >= 0; i--) {
                yield return originList[i];
            }
        }

        public static T PrevOrDefault<T>(this IList<T> list, T targetItem) {
            var index = list.IndexOf(targetItem);
            
            return index <= 0 ? default : list[index - 1];
        }

        public static T NextOrDefault<T>(this IList<T> list, T targetItem) {
            var index = list.IndexOf(targetItem);

            if (index == -1 || index == list.Count - 1) {
                return default;
            }
            
            return list[index + 1];
        }
    }
}