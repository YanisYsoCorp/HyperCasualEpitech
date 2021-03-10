using System.Text.RegularExpressions;
using UnityEngine;

public static class YcStringExtensions {

    public static Color ToColor(this string that) {
        Color color;
        ColorUtility.TryParseHtmlString(that, out color);
        return color;
    }

    public static string ToCapitalize(this string that) {
        if (string.IsNullOrEmpty(that)) {
            return string.Empty;
        }
        return char.ToUpper(that[0]) + that.Substring(1);
    }

    public static string AddSpacesToSentence(this string that) {
        return Regex.Replace(that, "([a-z])([A-Z])", "$1 $2");
    }

    public static string ToCamelCase(this string that) {
        if (string.IsNullOrEmpty(that)) {
            return string.Empty;
        }
        string resultSpace = that.AddSpacesToSentence();
        string result = "";
        foreach (string r in resultSpace.Split(' ')) {
            result += char.ToUpper(r[0]) + r.Substring(1);
        }
        return char.ToLower(result[0]) + result.Substring(1);
    }

    public static string EnumToString(this string that) {
        if (string.IsNullOrEmpty(that)) {
            return string.Empty;
        }
        string result = that.AddSpacesToSentence();
        result = result.ToLower();
        return char.ToUpper(result[0]) + result.Substring(1);
    }

}