using System;
using System.Collections.Generic;

namespace NodeCanvasAddons.AStarPathfinding
{
    public static class ListExtensions
    {
        public static List<List<T>> Split<T>(this List<T> source, int size)
        {
            var ret = new List<List<T>>();
            for (var i = 0; i < source.Count; i += size)
            { ret.Add(source.GetRange(i, Math.Min(size, source.Count - i))); }
            return ret;
        }
    }
}
