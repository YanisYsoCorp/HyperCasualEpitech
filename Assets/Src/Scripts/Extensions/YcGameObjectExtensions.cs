using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class YcGameObjectExtensions {

    static void _AssertObject(GameObject that, UnityEngine.Object t, string n, bool assert = true) {
        if (assert && t == null) {
            Debug.LogError(that.name + " do not have object " + n);
        }
    }

    static GameObject _GetChildGameObjectByNameRec(Transform tr, string n) {
        foreach (Transform t in tr) {
            if (t.name == n) {
                return t.gameObject;
            } else {
                GameObject g = _GetChildGameObjectByNameRec(t, n);
                if (g != null) {
                    return g;
                }
            }
        }
        return null;
    }
    static public GameObject GetChildGameObjectByNameRec(this GameObject that, string n, bool assert = true) {
        GameObject t = _GetChildGameObjectByNameRec(that.transform, n);
        _AssertObject(that, t, n, assert);
        return t;
    }

    static GameObject _GetChildGameObjecstByNameRec(Transform tr, string n, List<GameObject> objects) {
        foreach (Transform t in tr) {
            if (t.name == n) {
                objects.Add(t.gameObject);
            } else {
                GameObject g = _GetChildGameObjecstByNameRec(t, n, objects);
                if (g != null) {
                    return g;
                }
            }
        }
        return null;
    }

    static public GameObject[] GetChildGameObjectsByNameRec(this GameObject that, string n, bool assert = true) {
        List<GameObject> objects = new List<GameObject>();
        _GetChildGameObjecstByNameRec(that.transform, n, objects);
        if (assert && objects.Count == 0) {
            Debug.LogError(that.name + " do not have object " + n);
        }
        return objects.ToArray();
    }

    static public GameObject GetChildGameObjectByName(this GameObject that, string n, bool assert = true) {
        GameObject t = null;
        foreach (Transform g in that.transform) {
            if (g.name == n) {
                t = g.gameObject;
                break;
            }
        }
        _AssertObject(that, t, n, assert);
        return t;
    }

    static public GameObject GetChildGameObjectByNameStart(this GameObject that, string n, bool assert = true) {
        GameObject t = null;
        foreach (Transform g in that.transform) {
            if (g.name.StartsWith(n)) {
                t = g.gameObject;
                break;
            }
        }
        _AssertObject(that, t, n, assert);
        return t;
    }

    static public GameObject AddGameObject(this GameObject that, string n) {
        GameObject g = new GameObject(n);
        g.transform.SetParent(that.transform);
        return g;
    }

    static public T AddGameObject<T>(this GameObject that, string n = "") where T : Component {
        if (n == null || n == "") {
            n = typeof(T).Name;
        }
        GameObject g = AddGameObject(that, n);
        return g.AddComponent<T>();
    }

    static public void SetLayerRec(this GameObject that, int layer) {
        that.layer = layer;
        foreach (Transform child in that.transform) {
            child.gameObject.SetLayerRec(layer);
        }
    }

    static public T GetComponentInChildrenAssert<T>(this GameObject that, bool includeInactive = false) where T : UnityEngine.Object {
        T t = that.GetComponentInChildren<T>(includeInactive);
        _AssertObject(that, t, typeof(T).FullName, true);
        return t;
    }

    static public T GetComponentAssert<T>(this GameObject that) where T : UnityEngine.Object {
        T t = that.GetComponent<T>();
        _AssertObject(that, t, typeof(T).FullName, true);
        return t;
    }

    static public T GetComponentInParentAssert<T>(this GameObject that) where T : UnityEngine.Object {
        T t = that.GetComponentInParent<T>();
        _AssertObject(that, t, typeof(T).FullName, true);
        return t;
    }

    static public GameObject AddGameObjectRT(this GameObject that, string n, float x = 100, float y = 100) {
        GameObject g = new GameObject(n, typeof(RectTransform));
        g.transform.SetParent(that.transform);
        g.transform.localPosition = Vector3.zero;
        g.transform.localScale = Vector3.one;
        g.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
        return g;
    }

    static public T AddGameObjectRT<T>(this GameObject that, string n = "", float x = 100, float y = 100) where T : Component {
        if (n == null || n == "") {
            n = typeof(T).Name;
        }
        GameObject g = AddGameObjectRT(that, n, x, y);
        return g.AddComponent<T>();
    }

    static public void ChangeAllMaterial(this GameObject that, Material mat) {
        foreach (Renderer r in that.GetComponents<Renderer>()) {
            r.ChangeAllMaterial(mat);
        }
        foreach (Renderer r in that.GetComponentsInChildren<Renderer>()) {
            r.ChangeAllMaterial(mat);
        }
    }

    static public void FadeAll(this GameObject that, float fade, float time) {
        Image iS = that.GetComponent<Image>();
        iS?.DOKill(true);
        iS?.GetComponent<Image>().DOFade(fade, time);
        Text tS = that.GetComponent<Text>();
        tS?.DOKill(true);
        tS?.GetComponent<Image>().DOFade(fade, time);
        foreach (Image i in that.GetComponentsInChildren<Image>()) {
            i.DOKill(true);
            i.DOFade(fade, time);
        }
        foreach (Text t in that.GetComponentsInChildren<Text>()) {
            t.DOKill(true);
            t.DOFade(fade, time);
        }
    }

}