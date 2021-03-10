using UnityEngine;
using System.Globalization;

public static class YcFloatExtensions {

    public static string ToStringTime(this float that) {
        int minutes = (int)that / 60;
        int seconds = (int)that - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}