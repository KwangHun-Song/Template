using System.Collections.Generic;

namespace DrawLine {
    public static class CollectionExtensions {
        public static IEnumerable<T> Reversed<T>(this IList<T> originList) {
            for (int i = originList.Count - 1; i >= 0; i--) {
                yield return originList[i];
            }
        }
    }
}