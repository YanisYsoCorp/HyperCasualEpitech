using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace YsoCorp {
    namespace GameUtils {

        public enum e_Platforms {
            None = 0,
            Ios = 1,
            Android = 2
        }

        [DefaultExecutionOrder(-10)]
        public class RequestManager : BaseManager {

            static string URL_PROD = "https://games-api.ysocorp.com/v";
            static string URL_DEV = "http://localhost:3000/v";
            static int VERSION = 6;

#if UNITY_EDITOR
            static bool DEV_MODE = true;
#else
            static bool DEV_MODE = false;
#endif

            public delegate void OnComplete(string json);

            Dictionary<UnityWebRequest, OnComplete> _requests = new Dictionary<UnityWebRequest, OnComplete>();

            protected override void Awake() {
                base.Awake();
            }

            public void Start() {
                Application.RequestAdvertisingIdentifierAsync(
                    (string advertisingId, bool trackingEnabled, string error) => {
                        if (advertisingId != null && advertisingId != "") {
                            this.ycManager.dataManager.SetAdvertisingId(advertisingId);
                        }
                    }
                );
            }

            private e_Platforms GetPlatform() {
#if UNITY_ANDROID
                return e_Platforms.Android;
#elif UNITY_IOS
                return e_Platforms.Ios;
#else
                return e_Platforms.None;
#endif
            }

            private string GetAdvertisingId() {
                return this.ycManager.dataManager.GetAdvertisingId();
            }

            public string GetGameKey() { return this.ycManager.ycConfig.gameYcId; }
            public string GetGameVersion() { return Application.version; }
            public string GetGameAbTesting() { return this.ycManager.abTestingManager.GetPlayerSample(); }

            public int GetDevicePlatform() { return (int)this.GetPlatform(); }
            public string GetDeviceKey() { return this.ycManager.ycConfig.deviceKey; }
            public string GetDeviceAdvertisingId() { return this.GetAdvertisingId(); }

            static public string GetUrlEmptyStatic(string path, bool prod = false) {
                string url = URL_PROD + VERSION + "/" + path;
                if (DEV_MODE && prod == false) {
                    url = URL_DEV + VERSION + "/" + path;
                }
                return url;
            }

            public string GetUrlEmpty(string path) {
                return GetUrlEmptyStatic(path);
            }

            public string GetUrl(string path, string gets = "") {
                string url = this.GetUrlEmpty(path);
                url += "?";
                url += "&game_key=" + this.GetGameKey();
                url += "&game_version=" + this.GetGameVersion();
                url += "&game_ab_testing=" + this.GetGameAbTesting();
                url += "&device_platform=" + this.GetDevicePlatform();
                url += "&device_key=" + this.GetDeviceKey();
                url += "&device_advertising_id=" + this.GetDeviceAdvertisingId();
                url += gets;
                return url;
            }

            public void Update() {
                foreach (KeyValuePair<UnityWebRequest, OnComplete> request in this._requests) {
                    UnityWebRequest r = request.Key;
                    OnComplete a = request.Value;
                    if (r.isDone) {
                        if (!r.isNetworkError && !r.isHttpError) {
                            a(r.downloadHandler.text);
                        } else {
                            a(null);
                            Debug.Log("[REQUEST][" + r.GetType() + "][ERROR] " + r.url + " " + r.error);
                        }
                        this._requests.Remove(r);
                        return;
                    }
                }
            }

            public void SendGet(string uri, OnComplete action = null) {
                UnityWebRequest r = UnityWebRequest.Get(uri);
                r.SendWebRequest();
                if (action != null) {
                    this._requests.Add(r, action);
                }
                Debug.Log("[REQUEST][GET] " + uri);
            }

            public void SendPost(string uri, string data, OnComplete action = null) {
                UnityWebRequest r = new UnityWebRequest(uri, "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
                r.uploadHandler = new UploadHandlerRaw(bodyRaw);
                r.downloadHandler = new DownloadHandlerBuffer();
                r.SetRequestHeader("Content-Type", "application/json");
                r.SendWebRequest();
                if (action != null) {
                    this._requests.Add(r, action);
                }
                Debug.Log("[REQUEST][POST] " + uri + " = " + data);
            }

        }

    }
}
