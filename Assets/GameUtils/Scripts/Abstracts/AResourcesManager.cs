using UnityEngine;
using System.Collections.Generic;

namespace YsoCorp {

    public class AResourcesManager : YCBehaviour {

        bool _needUnload = false;

        public void DebugCheckNbResources<T>(string path, int nb) where T : Object {
#if UNITY_EDITOR
            T[] elems = this.LoadIterator<T>(path);
            if (elems.Length != nb) {
                Debug.LogError(path + " not correct number " + nb + "!=" + elems.Length);
            }
            this.UnloadUnusedAssets();
#endif
        }

        private void Update() {
            if (this._needUnload == true) {
                this._needUnload = false;
                Resources.UnloadUnusedAssets();
            }
        }

        public T Load<T>(string path) where T : Object {
            return Resources.Load<T>(path);
        }

        public void UnloadUnusedAssets() {
            this._needUnload = true;
        }

        public T[] LoadIterator<T>(string path, int startIndex = 0) where T : Object {
            List<T> ts = new List<T>();
            for (int i = startIndex; ; i++) {
                T t = this.Load<T>(path + i);
                if (t) {
                    ts.Add(t);
                } else {
                    break;
                }
            }
            Debug.Log("[" + path + "] : " + ts.Count + " " + typeof(T).Name + " load !");
            return ts.ToArray();
        }

        public Dictionary<string, T> LoadDictionary<T>(string path) where T : Object {
            Dictionary<string, T> dic = new Dictionary<string, T>();
            T[] files = Resources.LoadAll<T>(path);
            foreach (T t in files) {
                dic[t.name] = t;
            }
            Debug.Log("[" + path + "] : " + dic.Count + " " + typeof(T).Name + " load !");
            return dic;
        }

        public T LoadJson<T>(string file) {
            TextAsset jsonFile = (TextAsset)Resources.Load(file, typeof(TextAsset));
            if (jsonFile == null) {
                Debug.Log("[ JSON: " + file + " not found !");
                return default(T);
            }
            return JsonUtility.FromJson<T>(jsonFile.text);
        }

    }
}
