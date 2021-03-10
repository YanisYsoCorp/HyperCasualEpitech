using UnityEngine;

namespace YsoCorp {
    namespace GameUtils {

        public class ObjectJson<T> {
            public T data;
        }

        [System.Serializable]
        public class AdJson {
            public string game_key;
            public int game_id;
            public int video_id;
            public int device_id;
            public string link;
            public string video;
            public string name;
        }

        [DefaultExecutionOrder(-10)]
        public class UtilsAds : BaseManager {

            public delegate void OnComplete(AdJson ad);

            public T JsonToObject<T>(string str) {
                return JsonUtility.FromJson<ObjectJson<T>>(str).data;
            }

            private string GetUrl(string action) {
                string gets = "";
                if (this.crossPromo.ad != null) {
                    gets += "&ads_game_key=" + this.crossPromo.ad.game_key;
                    gets += "&ads_game_id=" + this.crossPromo.ad.game_id;
                    gets += "&ads_device_id=" + this.crossPromo.ad.device_id;
                    gets += "&ads_video_id=" + this.crossPromo.ad.video_id;
                }
                string url = this.ycManager.requestManager.GetUrl("ads/" + action, gets);
                return url;
            }

            public void RequestAd(OnComplete action) {
                if (this.ycManager.ycConfig.gameYcId != "") {
                    this.ycManager.requestManager.SendGet(this.GetUrl("request"), (string response) => {
                        if (response != null) {
                            action(this.JsonToObject<AdJson>(response));
                        } else {
                            action(null);
                        }
                    });
                }
            }

            public void DisplayAd() {
                this.ycManager.requestManager.SendGet(this.GetUrl("display"));
            }

            public void ClickAd() {
                this.ycManager.requestManager.SendGet(this.GetUrl("click"));
            }

        }

    }
}
