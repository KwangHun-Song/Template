using System;
using System.Linq;

namespace Utility {
    public static class ComparableExtension {
        /// <summary>
        /// 해당 값이 최소값과 최대값 안으로만 나오도록 설정한다.
        /// </summary>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T> {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
        
            return val;
        }
        public static T ClampMin<T>(this T val, T min) where T : IComparable<T> {
            if (val.CompareTo(min) < 0) return min;
        
            return val;
        }
        public static T ClampMax<T>(this T val, T max) where T : IComparable<T> {
            if (val.CompareTo(max) > 0) return max;
        
            return val;
        }

        /// <summary>
        /// 해당 값이 min과 max 사이에 있는지 여부를 나타낸다.
        /// </summary>
        public static bool IsIn<T>(this T val, T min, T max) where T : IComparable<T> {
            if (val.CompareTo(min) < 0) return false;
            if (val.CompareTo(max) > 0) return false;
        
            return true;
        }
        
        /// <summary>
        /// Math.Min과 같은 동작이나 여러 개의 파라미터를 받을 수 있다.
        /// </summary>
        public static T Min<T>(this T val, params T[] others) where T : IComparable<T> {
            var list = others.ToList();
            list.Add(val);
            return list.Min();
        }

        /// <summary>
        /// Math.Max과 같은 동작이나 여러 개의 파라미터를 받을 수 있다.
        /// </summary>
        public static T Max<T>(this T val, params T[] others) where T : IComparable<T> {
            var list = others.ToList();
            list.Add(val);
            return list.Max();
        }
    }
}