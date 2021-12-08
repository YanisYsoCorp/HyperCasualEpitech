using UnityEngine;
using System.Collections.Generic;

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class TenjinManager : MmpBaseManager {

            private static string API_KEY = "BP2IBD5EPSJLBT2JYHWDGTVGXQVF6YGK";

            private BaseTenjin _tenjin;
            public AnalyticsManager.TenjinData tenjinData = new AnalyticsManager.TenjinData();

            public override void Init() {
                if (this._tenjin == null) {
                    this._tenjin = Tenjin.getInstance(API_KEY);
#if UNITY_IOS
                    this._tenjin.RegisterAppForAdNetworkAttribution();
                    this._tenjin.Connect();
                    this._tenjin.GetDeeplink(this.DeferredDeeplinkCallback);
#elif UNITY_ANDROID
                    this._tenjin.Connect();
                    this._tenjin.GetDeeplink(this.DeferredDeeplinkCallback);
#endif
                }
            }

            int GetInters() {
                int inters = this.ycManager.dataManager.GetInterstitialsNb();
                if (inters >= 50) { return 7; } // 111
                if (inters >= 25) { return 6; } // 110
                if (inters >= 20) { return 5; } // 101
                if (inters >= 15) { return 4; } // 100
                if (inters >= 10) { return 3; } // 011
                if (inters >= 5) { return 2; } // 010
                if (inters >= 1) { return 1; } // 001
                return 0; // 000
            }

            int GetRewards() {
                int rewardes = this.ycManager.dataManager.GetRewardedsNb();
                if (rewardes >= 20) { return 56; } // 111000
                if (rewardes >= 15) { return 48; } // 110000
                if (rewardes >= 10) { return 40; } // 101000
                if (rewardes >= 5) { return 32; } // 100000
                if (rewardes >= 3) { return 24; } // 011000
                if (rewardes >= 2) { return 16; } // 010000
                if (rewardes >= 1) { return 8; } // 001000
                return 0; // 000
            }

            protected override void OnDestroy() {
#if UNITY_IOS
                if (this.ycManager.dataManager.GetDiffTimestamp() <= 60 * 60 * 24) {
                    this._tenjin.UpdateConversionValue(this.GetInters() + this.GetRewards());
                }
#endif
            }

            public override void SendEvent(string eventName) {
                if (this._tenjin) {
                    this._tenjin.SendEvent(eventName);
                }
            }

            public override void SetConsent(bool consent) {
                if (this._tenjin) {
                    if (consent) {
                        this._tenjin.OptIn();
                    } else {
                        this._tenjin.OptOut();
                    }
                }
            }

            private void OnApplicationPause(bool paused) {
                if (paused == false) {
                    this.Init();
                }
            }

            public void DeferredDeeplinkCallback(Dictionary<string, string> data) {
                this.tenjinData.is_init = true;
                if (data.ContainsKey("advertising_id")) {
                    this.tenjinData.advertising_id = data["advertising_id"];
                }
                if (data.ContainsKey("ad_network")) {
                    this.tenjinData.ad_network = data["ad_network"];
                }
                if (data.ContainsKey("campaign_id")) {
                    this.tenjinData.campaign_id = data["campaign_id"];
                }
                if (data.ContainsKey("campaign_name")) {
                    this.tenjinData.campaign_name = data["campaign_name"];
                }
                if (data.ContainsKey("site_id")) {
                    this.tenjinData.site_id = data["site_id"];
                }
                if (data.ContainsKey("referrer")) {
                    this.tenjinData.referrer = data["referrer"];
                }
                if (data.ContainsKey("deferred_deeplink_url")) {
                    this.tenjinData.deferred_deeplink_url = data["deferred_deeplink_url"];
                }
                if (data.ContainsKey("clicked_tenjin_link")) {
                    this.tenjinData.clicked_tenjin_link = (data["clicked_tenjin_link"].ToLower() == "true");
                }
                if (data.ContainsKey("is_first_session")) {
                    this.tenjinData.is_first_session = (data["is_first_session"].ToLower() == "true");
                }
            }

        }
    }
}

