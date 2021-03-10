using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class YcArrayExtensions {

    public static T[] RandomSelf<T>(this T[] that) {
        System.Random rnd = new System.Random();
        int n = that.Length;
        while (n > 1) {
            int k = rnd.Next(n--);
            T temp = that[n];
            that[n] = that[k];
            that[k] = temp;
        }
        return that;
    }

    public static T[] Random<T>(this T[] that) {
        T[] nArray = new T[that.Length];
        Array.Copy(that, nArray, that.Length);
        return nArray.RandomSelf();
    }

    public static T[] RemoveDisable<T>(this T[] that) where T : MonoBehaviour {
        List<T> list = new List<T>();
        foreach (T t in that) {
            if (t.enabled == true) {
                list.Add(t);
            }
        }
        return list.ToArray();
    }

}