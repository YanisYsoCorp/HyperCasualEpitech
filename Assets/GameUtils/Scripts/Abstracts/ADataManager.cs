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

        public void SetInt(string key, int value) {
            PlayerPrefs.SetInt(this.GetKey(key), value);
        }

        public void SetFloat(string key, float value) {
            PlayerPrefs.SetFloat(this.GetKey(key), value);
        }

        public void SetString(string key, string value) {
            PlayerPrefs.SetString(this.GetKey(key), value);
        }

        public int GetInt(string key, int defaultValue = 0) {
            return PlayerPrefs.GetInt(this.GetKey(key), defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0) {
            return PlayerPrefs.GetFloat(this.GetKey(key), defaultValue);
        }

        public string GetString(string key, string value = "") {
            return PlayerPrefs.GetString(this.GetKey(key), value);
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

        public void ForceSave() {
            PlayerPrefs.Save();
        }

    }

}