using System;
using UnityEngine;
using System.Linq;

public class JsonUtilityArray {

    [System.Serializable]
    public class ObjectJson<T> {
        public T[] data;
    }

    public static T[] FromJson<T>(string str) {
        ObjectJson<T> obj = JsonUtility.FromJson<ObjectJson<T>>("{\"data\":" + str + "}");
        return obj.data;
    }

    public static string ToJson<T>(T[] data) {
        ObjectJson<T> obj = new ObjectJson<T>() {
            data = data
        };
        string str = JsonUtility.ToJson(obj);
        str = str.Remove(0, 8);
        str = str.Remove(str.Length - 1, 1);
        return str;
    }

}