using UnityEngine;

public static class YcTexture2DExtensions {

    public static Texture2D Duplicate(this Texture2D that) {
        Texture2D texture = new Texture2D(that.width, that.height);
        texture.SetPixels(that.GetPixels());
        return texture;
    }

    public static void ReplaceColor(this Texture2D that, Color color, Color colorReplace) {
        Color[] pixels = that.GetPixels();
        for (int i = 0; i < pixels.Length; i++) {
            if (pixels[i] == color) {
                pixels[i] = colorReplace;
            }
        }
        that.SetPixels(pixels);
    }

}