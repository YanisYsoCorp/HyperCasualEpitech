using UnityEngine;

public static class YcSpriteExtensions {

    public static Sprite Duplicate(this Sprite that) {
        Texture2D t = that.texture;
        return Sprite.Create(t.Duplicate(), new Rect(0, 0, t.width, t.height), Vector2.one / 2f);
    }

}