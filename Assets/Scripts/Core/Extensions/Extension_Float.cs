using UnityEngine;

namespace VHS {
    public static class Extension_Float {
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax) => (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;

        public static float OneMinus(this float value) => 1f - value;

        public static float Square(this float value) => value * value;

        public static float RandomBinomial(float range = 1) => (Random.value - Random.value) * range;

        public static bool IsWithinRange(this float value, Vector2 range, bool includeMax = true,bool includeMin = true) => IsWithinRange(value,range.x,range.y, includeMax, includeMin); 
        public static bool IsWithinRange(this float value, float min, float max, bool includeMax = true, bool includeMin = true) {
            bool withinRange = value >= min && value <= max;

            if(withinRange) {
                if(!includeMax && value == max)
                    return false;

                if(!includeMin && value == min)
                    return false;
            }

            return withinRange;
        }
        
        public static bool IsWithinRange(this int value, Vector2Int range, bool includeMax = true,bool includeMin = true) => IsWithinRange(value,range.x,range.y, includeMax, includeMin); 
        public static bool IsWithinRange(this int value, int min, int max, bool includeMax = true, bool includeMin = true) {
            bool withinRange = value >= min && value <= max;

            if(withinRange) {
                if(!includeMax && value == max)
                    return false;

                if(!includeMin && value == min)
                    return false;
            }

            return withinRange;
        }

        public static float Spring(float from, float to, float time) {
            time = Mathf.Clamp01(time);
            time = (Mathf.Sin(time * Mathf.PI * (.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
            return from + (to - from) * time;
        }
    }
}