using UnityEngine;
using System.Globalization;

public static class YcIntegerExtensions {

    static CultureInfo ci;

    public static string ToStringWithSuffix(this int num) {
        string number = num.ToString();
        if (number.EndsWith("11")) return number + "th";
        if (number.EndsWith("12")) return number + "th";
        if (number.EndsWith("13")) return number + "th";
        if (number.EndsWith("1")) return number + "st";
        if (number.EndsWith("2")) return number + "nd";
        if (number.EndsWith("3")) return number + "rd";
        return number + "th";
    }

    public static string ToDisplay(this int num) {
        if (ci == null) {
            ci = new CultureInfo("en-us");
        }
        return num.ToString("N0", ci);
    }
}