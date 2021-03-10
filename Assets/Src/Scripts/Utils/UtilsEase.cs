using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace YsoCorp {

    public sealed class UtilsEase {

        public static float InQuad(float t) {
            return t * t;
        }

        public static float OutQuad(float t) {
            return t * (2 - t);
        }

        public static float InOutQuad(float t) {
            return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

        public static float InCubic(float t) {
            return t * t * t;
        }

        public static float OutCubic(float t) {
            return (--t) * t * t + 1;
        }

        public static float InOutCubic(float t) {
            return t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
        }

        public static float InQuart(float t) {
            return t * t * t * t;
        }

        public static float OutQuart(float t) {
            return 1 - (--t) * t * t * t;
        }

        public static float InOutQuart(float t) {
            return t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
        }

        public static float InQuint(float t) {
            return t * t * t * t * t;
        }

        public static float OutQuint(float t) {
            return 1 + (--t) * t * t * t * t;
        }

        public static float InOutQuint(float t) {
            return t < .5 ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
        }

    }
}