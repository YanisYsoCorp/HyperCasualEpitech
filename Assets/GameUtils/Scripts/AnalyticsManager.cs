using UnityEngine;
using System;
using System.Collections.Generic;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class AnalyticsManager : BaseManager {

            private static bool SHOW_DEBUG = false;
            private static int TIME_LOAD_NB = 10;

            public class AppData {
                public string device_key;
                public int device_platform;
                public string device_advertising_id;
                public string device_model;
                public string device_os_version;
                public int device_memory_size;
                public int device_processor_count;
                public int device_processor_frequency;
                public string device_processor_type;
                public string game_key;
                public string game_version;
                public string game_ab_testing;
                public string sdk_version;
                public SessionData session;
            }

            [Serializable]
            public class SessionData {
                public string key;
                public bool is_first;
                public int nb_start = 0;
                public int nb_win = 0;
                public int nb_lose = 0;
                public int playtime = 0;
                public int time_load;
                public int time_no_ads;
                public int time_ads;
                public int fps;
                public int fps_no_ads;
                public int fps_ads;
                public string app_tracking_status;
                public string push_notif_status;
                public TenjinData tenjin;
                public Dictionary<string, int> events = new Dictionary<string, int>();
                public List<GameData> games = new List<GameData>();
                public List<InAppData> inApps = new List<InAppData>();
                public List<AdData> ads_interstitial = new List<AdData>();
                public List<AdData> ads_rewarded = new List<AdData>();
                public List<AdData> ads_banner = new List<AdData>();
            }

            [Serializable]
            public class GameData {
                public int level = 0;
                public float score = 0f;
                public string status = "START";
                public int playtime = 0;
            }

            [Serializable]
            public class InAppData {
                public string id;
                public float price = 0f;
                public string iso_country_code;
            }

            [Serializable]
            public class AdData {
                public float revenue;
                public string country_code;
                public string network_name;
                public string ad_unit_id;
                public string network_placement;
                public string placement;
                public string creative_id;
            }

            [Serializable]
            public class TenjinData {
                public bool is_init;
                public string advertising_id;
                public string ad_network;
                public string campaign_id;
                public string campaign_name;
                public string site_id;
                public string referrer;
                public string deferred_deeplink_url;
                public bool clicked_tenjin_link;
                public bool is_first_session;
            }

            private bool _onStart = false;

            AppData _appData;
            GameData _gameD;

            float _gamePlaytime;
            float _sessionPlaytime;

            float _timeLoad;
            int _timeNb;

            float _fpsNoAdsTime;
            int _fpsNoAdsNb;

            float _fpsAdsTime;
            int _fpsAdsNb;

            void Start() {
                this._appData = new AppData();
                this._appData.device_key = this.ycManager.requestManager.GetDeviceKey();
                this._appData.device_advertising_id = SystemInfo.deviceUniqueIdentifier;
                this._appData.device_model = SystemInfo.deviceModel;
                this._appData.device_os_version = SystemInfo.operatingSystem;
                this._appData.device_memory_size = SystemInfo.systemMemorySize;
                this._appData.device_processor_count = SystemInfo.processorCount;
                this._appData.device_processor_frequency = SystemInfo.processorFrequency;
                this._appData.device_processor_type = SystemInfo.processorType;
                this._appData.device_platform = this.ycManager.requestManager.GetDevicePlatform();
                this._appData.game_key = this.ycManager.requestManager.GetGameKey();
                this._appData.game_version = this.ycManager.requestManager.GetGameVersion();
                this._appData.game_ab_testing = this.ycManager.requestManager.GetGameAbTesting();
                this._appData.sdk_version = YCConfig.VERSION;
                this.SessionStart();
                this._onStart = true;
            }

            void Update() {
                if (this._timeNb < TIME_LOAD_NB) {
                    this._timeLoad += Time.unscaledDeltaTime;
                    this._timeNb++;
                } else {
                    if (this.ycManager.adsManager.IsInterstitialOrRewardedVisible()) {
                        this._fpsAdsTime += Time.unscaledDeltaTime;
                        this._fpsAdsNb++;
                    } else {
                        this._fpsNoAdsTime += Time.unscaledDeltaTime;
                        this._fpsNoAdsNb++;
                    }
                }
            }

            protected override void OnApplicationQuit() {
                base.OnApplicationQuit();
                this.SessionEnd();
            }

            string CreateGUIDV4() {
                return Guid.NewGuid().ToString();
            }

            void OnApplicationPause(bool pause) {
                if (this._onStart == true) {
                    if (pause) {
                        this.SessionEnd();
                    } else {
                        this.SessionStart();
                    }
                }
            }

            public SessionData GetSession() {
                if (this._appData.session == null) {
                    this._appData.session = new SessionData();
                    this._sessionPlaytime = Time.time;
                    this._appData.session.key = this.CreateGUIDV4();
                }
                this._appData.session.tenjin = this.ycManager.mmpManager.tenjinManager.tenjinData;
                return this._appData.session;
            }

            public void SessionStart() {
                if (this._appData.session == null) {
                    this.GetSession();
                    this.DebugSession();
                }
            }

            public int GetPlaytime(float t) {
                return (int)((Time.time - t) * 1000);
            }

            public float GetFps() {
                if ((this._fpsAdsNb + this._fpsNoAdsNb) == 0 || (this._fpsAdsTime + this._fpsNoAdsTime) == 0) {
                    return 0f;
                }
                return (float)Math.Round((this._fpsAdsNb + this._fpsNoAdsNb) / (this._fpsAdsTime + this._fpsNoAdsTime));
            }

            public float GetFpsNoAds() {
                if (this._fpsNoAdsNb == 0 || this._fpsNoAdsTime == 0) {
                    return 0f;
                }
                return (float)Math.Round(this._fpsNoAdsNb / this._fpsNoAdsTime);
            }

            public float GetFpsAds() {
                if (this._fpsAdsNb == 0 || this._fpsAdsTime == 0) {
                    return 0f;
                }
                return (float)Math.Round(this._fpsAdsNb / this._fpsAdsTime);
            }

            public void SessionEnd() {
                if (this._appData.session != null) {
                    this.GetSession().is_first = this.ycManager.IsFirstTimeAppLaunched();
                    this.GetSession().playtime = this.GetPlaytime(this._sessionPlaytime);
                    this.GetSession().time_load = (int)Math.Round(this._timeLoad);
                    this.GetSession().time_ads = (int)Math.Round(this._fpsAdsTime);
                    this.GetSession().time_no_ads = (int)Math.Round(this._fpsNoAdsTime);
                    this.GetSession().fps = (int)this.GetFps();
                    this.GetSession().fps_ads = (int)this.GetFpsAds();
                    this.GetSession().fps_no_ads = (int)this.GetFpsNoAds();
                    this.GetSession().app_tracking_status = this.ycManager.adsManager.GetAppTrackingStatus();
                    this.GetSession().push_notif_status = this.ycManager.pushNotificationManager.GetStatus();
                    if (this.GetSession().games.Count > 0) {
                        GameData gameData = this.GetSession().games[this.GetSession().games.Count - 1];
                        if (gameData.playtime == 0) {
                            gameData.playtime = this.GetPlaytime(this._gamePlaytime);
                        }
                    }
                    this.ycManager.requestManager.SendPost(this.GetUrlEmpty("session"), this.GetAppJson());
                    this.DebugSession();
                    this._appData.session = null;
                    this._gameD = null;
                }
            }

            private string GetUrlEmpty(string action) {
                return this.ycManager.requestManager.GetUrlEmpty("analytics/" + action);
            }

            public void OnGameStarted(int level = 1) {
                if (this._gameD != null) {
                    this._gameD.playtime = this.GetPlaytime(this._gamePlaytime);
                }
                this._gameD = new GameData();
                this._gameD.level = level;
                this._gamePlaytime = Time.time;
                this.GetSession().games.Add(this._gameD);
                this.GetSession().nb_start++;
                this.DebugSession();
            }

            public void OnGameFinished(bool win, float score = 0) {
                if (this._gameD != null) {
                    this._gameD.score = score;
                    this._gameD.playtime = this.GetPlaytime(this._gamePlaytime);
                    this.GetSession().nb_start--;
                    if (win) {
                        this._gameD.status = "WIN";
                        this.GetSession().nb_win++;
                    } else {
                        this._gameD.status = "LOSE";
                        this.GetSession().nb_lose++;
                    }
                    this._gameD = null;
                    this.DebugSession();
                }
            }

            public void InAppBought(string id, float price, string isoCountryCode) {
                this.GetSession().inApps.Add(new InAppData() {
                    id = id,
                    price = price,
                    iso_country_code = isoCountryCode
                });
            }

            private void AddEvent(string key) {
                if (this.GetSession().events.ContainsKey(key)) {
                    this.GetSession().events[key]++;
                } else {
                    this.GetSession().events.Add(key, 1);
                }
                this.DebugSession();
            }

            public void InterstitialShow() { this.AddEvent("interstitial_show"); }
            public void InterstitialClick() { this.AddEvent("interstitial_click"); }

            public void RewardedShow() { this.AddEvent("rewarded_show"); }
            public void RewardedClick() { this.AddEvent("rewarded_click"); }

            public void BannerShow() { this.AddEvent("banner_show"); }
            public void BannerClick() { this.AddEvent("banner_click"); }

            public string GetAppJson() {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this._appData);
            }

            public string GetSessionJson() {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this.GetSession());
            }

            public void DebugSession() {
                if (SHOW_DEBUG) {
                    Debug.Log("DebugSession " + this.GetSessionJson());
                }
            }
        }
    }
}
