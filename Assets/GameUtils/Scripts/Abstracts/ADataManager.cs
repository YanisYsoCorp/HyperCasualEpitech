using System;
using UnityEngine;

namespace YsoCorp {

    public class ADataManager : YCBehaviour {

        private int _version = 1;
        private string _prefix = "";

        public ADataManager(string p = "", int v = 1) {
            this._prefix = p;
            this._version = v;
        }

        public string GetKey(string key) {
            return this._prefix + key + this._version;
        }

        public bool HasKey(string key) {
            return PlayerPrefs.HasKey(this.GetKey(key));
        }

        public void DeleteAll(bool forceDeletion = false) {
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
#endif
            if (forceDeletion == true) {
                PlayerPrefs.DeleteAll();
            }
        }

        public void DeleteKey(string key) {
            PlayerPrefs.DeleteKey(this.GetKey(key));
        }

        public void ForceSave() {
            PlayerPrefs.Save();
        }

        // INT
        public int GetInt(string key, int defaultValue = 0) {
            return PlayerPrefs.GetInt(this.GetKey(key), defaultValue);
        }
        public void SetInt(string key, int value) {
            PlayerPrefs.SetInt(this.GetKey(key), value);
        }

        // BOOL
        public bool GetBool(string key, bool defaultValue = false) {
            return PlayerPrefs.GetInt(this.GetKey(key), defaultValue ? 1 : 0) == 1;
        }
        public void SetBool(string key, bool value) {
            PlayerPrefs.SetInt(this.GetKey(key), value ? 1 : 0);
        }

        // FLOAT
        public float GetFloat(string key, float defaultValue = 0) {
            return PlayerPrefs.GetFloat(this.GetKey(key), defaultValue);
        }
        public void SetFloat(string key, float value) {
            PlayerPrefs.SetFloat(this.GetKey(key), value);
        }

        // STRING
        public string GetString(string key, string value = "") {
            return PlayerPrefs.GetString(this.GetKey(key), value);
        }
        public void SetString(string key, string value) {
            PlayerPrefs.SetString(this.GetKey(key), value);
        }

        // OBJECT
        public T GetObject<T>(string key, string value = "{}") {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(this.GetKey(key), value));
        }
        public void SetObject<T>(string key, T value) {
            PlayerPrefs.SetString(this.GetKey(key), Newtonsoft.Json.JsonConvert.SerializeObject(value));
        }

        // ARRAY
        public T[] GetArray<T>(string key, string value = "[]") {
            return this.GetObject<T[]>(key, value);
        }
        public void SetArray<T>(string key, T[] value) {
            this.SetObject(key, value);
        }

    }

}