using UnityEngine;

public static class YcRectTransformExtensions {

    public static void SetLeft(this RectTransform that, float left) {
        that.offsetMin = new Vector2(left, that.offsetMin.y);
    }

    public static void SetRight(this RectTransform that, float right) {
        that.offsetMax = new Vector2(-right, that.offsetMax.y);
    }

    public static void SetTop(this RectTransform that, float top) {
        that.offsetMax = new Vector2(that.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform that, float bottom) {
        that.offsetMin = new Vector2(that.offsetMin.x, bottom);
    }

}