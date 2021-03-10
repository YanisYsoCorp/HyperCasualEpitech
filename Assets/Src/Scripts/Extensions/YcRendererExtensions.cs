using UnityEngine;

public static class YcRendererExtensions {

    static public float GetOpacity(this Renderer that) {
        foreach (Material m in that.materials) {
            return m.color.a;
        }
        return 0f;
    }

    static public void SetOpacity(this Renderer that, float a) {
        foreach (Material m in that.materials) {
            Color color = m.color;
            color.a = a;
            m.color = color;
        }
    }

    static public void SetColor(this Renderer that, Color color) {
        foreach (Material m in that.materials) {
            m.color = color;
        }
    }

    static public void ChangeAllMaterial(this Renderer that, Material m) {
        if (that.materials.Length > 0) {
            Material[] mats = new Material[that.materials.Length];
            for (int i = 0; i < that.materials.Length; i++) {
                mats[i] = m;
            }
            that.materials = mats;
        }
    }

}