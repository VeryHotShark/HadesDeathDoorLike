using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VHS {
    public static class Extension_List {
        /// <summary>
        /// returns random between given range
        /// </summary>
        /// <param name="range">range used for Min Max</param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this List<T> list) {
            int randomIndex = Random.Range(0,list.Count);
            return list[randomIndex];
        }
     
        public static T GetRandomElement<T>(this T[] array) {
            int randomIndex = Random.Range(0,array.Length);
            return array[randomIndex];
        }
    }
}