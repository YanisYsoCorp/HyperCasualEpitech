using UnityEngine;
using UnityEngine.UI;
using System;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class AdsManager : BaseManager {

            public static string DATA_PRIVACY_URL = "https://www.ysocorp.com/privacy-policy";
            private static string SDK_KEY = "Wbl6_lWhN3Hoy5kL_UNgTb6Ed7o69yD8-mFOzFG68AmdgawolWGk0K2W_GHiai_D5N5pE_DA7MSfJoJZ_oOm1G";

            public Image iInterstitial;
            public Image iLoader;
            public Image iBanner;
            public Button bRemoveAds;

            public bool rewardedVisible { get; set; }
            public bool interstitialVisible { get; set; }

            private Action<bool> aRewarded = null;
            private Action aInterstitial = null;
            private bool _finishRewarded = false;
            private float _delayInterstitial = 0f;

#if !UNITY_EDITOR
            private bool _isEnableRewarded = false;
#endif
            private bool _isEnableInterstitial = false;
            private bool _isEnableBanner = false;

            private bool _bannerHide = true;
            private bool _hasDataPrivacy = false;
            private bool _canDisplayInterstitial = false;

            private MaxSdkBase.SdkConfiguration _sdkConfiguration;

            private float _onDisplayTimeScale;


            protected override void Awake() {
                base.Awake();
                this.bRemoveAds.onClick.AddListener(() => {
                    this.ycManager.inAppManager.BuyProductIDAdsRemove();
                });
                this.iBanner.gameObject.SetActive(false);
                this.iInterstitial.gameObject.SetActive(false);
                if (this.GetInterstitialKey() != "" || this.GetRewardedKey() != "" || this.GetBannerKey() != "") {
                    MaxSdkCallbacks.OnSdkInitializedEvent += this.OnSdkInitializedEvent;
                    MaxSdk.SetSdkKey(SDK_KEY);
                    MaxSdk.SetUserId(this.ycManager.ycConfig.deviceKey);
                    MaxSdk.InitializeSdk();
                } else {
                    this.ycManager.ycConfig.LogWarning("[Ads] not init");
                }
#if UNITY_EDITOR
                this._hasDataPrivacy = true;
#endif
            }

            public bool IsInterstitialOrRewardedVisible() {
                return this.rewardedVisible || this.interstitialVisible;
            }

            public string GetAppTrackingStatus() {
#if UNITY_IOS
                if (this._sdkConfiguration != null) {
                    return this._sdkConfiguration.AppTrackingStatus.ToString();
                }
#endif
                return "";
            }

            public AnalyticsManager.AdData OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
                AnalyticsManager.AdData adData = new AnalyticsManager.AdData();
                adData.revenue = (float)adInfo.Revenue;
                adData.country_code = MaxSdk.GetSdkConfiguration().CountryCode;
                adData.network_name = adInfo.NetworkName;
                adData.network_placement = adInfo.NetworkPlacement;
                adData.placement = adInfo.Placement;
                adData.ad_unit_id = adInfo.AdUnitIdentifier;
                adData.creative_id = adInfo.CreativeIdentifier;
                return adData;
            }

            void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration) {
                this._sdkConfiguration = sdkConfiguration;
                this.ycManager.mmpManager.Init();
                if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies) {
                    this._hasDataPrivacy = true;
                    this.InitConsentAndAds();
                } else if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.DoesNotApply) {
                    this.InitConsentAndAds();
                } else {
                    this.InitConsentAndAds();
                }
            }

            void LoaderRotate() {
                Vector3 angles = this.iLoader.transform.localEulerAngles;
                angles.z -= Time.deltaTime * 270f;
                this.iLoader.transform.localEulerAngles = angles;
            }

            void InitConsentAndAds() {
                this.InitConsent();
                this.InitAds();
            }

            void InitConsent() {
                MaxSdk.SetHasUserConsent(this.ycManager.dataManager.GetGdprAds());
                MaxSdk.SetIsAgeRestrictedUser(false);
            }

            private void Start() {
                this.ycManager.inAppManager.onPurchased.AddListener((string id) => {
                    if (id == this.ycManager.ycConfig.InAppRemoveAds) {
                        this.BuyAdsShow();
                    }
                });
            }

            private void Update() {
                this.bRemoveAds.gameObject.SetActive(
                    this.ycManager.ycConfig.InAppRemoveAdsCanRemoveInBanner &&
                    this.ycManager.ycConfig.InAppRemoveAds != "" &&
                    this.ycManager.dataManager.GetAdsShow()
                );
                this.LoaderRotate();
                if (this._delayInterstitial > 0f) {
                    this._delayInterstitial -= Time.deltaTime;
                }
            }

            private void InitAds() {
                this.InitializeBannerAds();
                this.InitializeInterstitialAds();
                this.InitializeRewardedAds();
            }

            public void TimeScaleOff() {
                this._onDisplayTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }

            public void TimeScaleOn() {
                Time.timeScale = this._onDisplayTimeScale;
            }

            public void DisplayGDPR() {
                if (this._hasDataPrivacy) {
                    bool hide = false;
                    if (this._bannerHide == false) {
                        this.HideBanner(true);
                        hide = true;
                    }
                    this.ycManager.gdprManager.Show(() => {
                        this.InitConsent();
                        if (hide == true) {
                            this.HideBanner(true);
                            this.HideBanner(false);
                        }
                    });
                } else {
                    Application.OpenURL(DATA_PRIVACY_URL);
                }
            }

            // REWARDED

            public void InitializeRewardedAds() {
                if (this.GetRewardedKey() != "") {
#if !UNITY_EDITOR
                    this._isEnableRewarded = true;
#endif

                    MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => { };
                    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo) => {
                        this._finishRewarded = true;
                    };
                    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.rewardedVisible = false;
                        this.TimeScaleOn();
                        this.LoadRewardedAd();
                        this.ycManager.soundManager.UnPauseMusic();
                        this.aRewarded(this._finishRewarded);
                    };
                    MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo) => {
                        this.Invoke("LoadRewardedAd", 2f);
                    };
                    MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) => {
                        this.LoadRewardedAd();
                    };
                    MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.rewardedVisible = true;
                        this.TimeScaleOff();
                        this.ycManager.analyticsManager.RewardedShow();
                        this.ycManager.dataManager.IncrementRewardedsNb();
                    };
                    MaxSdkCallbacks.Rewarded.OnAdClickedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.ycManager.analyticsManager.RewardedClick();
                    };
                    MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.ycManager.analyticsManager.GetSession().ads_rewarded.Add(this.OnAdRevenuePaidEvent(adUnitId, adInfo));
                    };
                    this.LoadRewardedAd();
                }
            }

            private void LoadRewardedAd() {
                MaxSdk.LoadRewardedAd(this.GetRewardedKey());
            }

            public bool IsRewardBasedVideo() {
#if UNITY_EDITOR
                return true;
#else
                return this._isEnableRewarded == true && MaxSdk.IsRewardedAdReady(this.GetRewardedKey());
#endif
            }

            public bool ShowRewarded(Action<bool> action) {
                this.aRewarded = action;
                if (this.IsRewardBasedVideo()) {
                    this.ycManager.soundManager.PauseMusic();
                    this._finishRewarded = false;
                    MaxSdk.ShowRewardedAd(this.GetRewardedKey());
                    return true;
                }
                action(false);
                return false;
            }

            float GetInterstitialDelay() {
                return this.ycManager.ycConfig.InterstitialInterval;
            }


            // INTERSTITIAL

            public void InitializeInterstitialAds() {
                if (this.GetInterstitialKey() != "") {
                    this._isEnableInterstitial = true;
                    this._delayInterstitial = this.GetInterstitialDelay();
                    MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => { };
                    MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo) => {
                        this.Invoke("LoadInterstitial", 2f);
                    };
                    MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) => {
                        this.LoadInterstitial();
                    };
                    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.interstitialVisible = false;
                        this.TimeScaleOn();
                        this.LoadInterstitial();
                        this.ycManager.soundManager.UnPauseMusic();
                        this.iInterstitial.gameObject.SetActive(false);
                        this.aInterstitial();
                        this._delayInterstitial = this.GetInterstitialDelay();
                    };
                    MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.interstitialVisible = true;
                        this.TimeScaleOff();
                        this.ycManager.analyticsManager.InterstitialShow();
                        this.ycManager.dataManager.IncrementInterstitialsNb();
                    };
                    MaxSdkCallbacks.Interstitial.OnAdClickedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.ycManager.analyticsManager.InterstitialClick();
                    };
                    MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.ycManager.analyticsManager.GetSession().ads_interstitial.Add(this.OnAdRevenuePaidEvent(adUnitId, adInfo));
                    };
                    this.LoadInterstitial();
                }
            }

            private void LoadInterstitial() {
                MaxSdk.LoadInterstitial(this.GetInterstitialKey());
            }

            public bool IsInterstitial(bool force = false) {
                bool cond = this._canDisplayInterstitial && this.IsAdsShow() && this._isEnableInterstitial && (this._delayInterstitial <= 0f || force == true) && this.ycManager.rateManager.IsShow() == false;
#if !UNITY_EDITOR
                if (cond) {
                    return MaxSdk.IsInterstitialReady(this.GetInterstitialKey());
                }
#endif
                return cond;
            }

            void _ShowInterstitial() {
                MaxSdk.ShowInterstitial(this.GetInterstitialKey());
            }

            public bool ShowInterstitial(Action action, float delai = -1f, bool force = false) {
                if (this.IsInterstitial(force)) {
                    this.iInterstitial.gameObject.SetActive(delai >= 0f);
                    this.ycManager.soundManager.PauseMusic();
                    this.aInterstitial = action;
                    if (delai <= 0f) {
                        this._ShowInterstitial();
                    } else {
                        this.Invoke("_ShowInterstitial", delai);
                    }
                    return true;
                }
                this._canDisplayInterstitial = true;
                action();
                return false;
            }

            private void InitializeBannerAds() {
                if (this.GetBannerKey() != "") {
                    this._isEnableBanner = true;
                    MaxSdk.CreateBanner(this.GetBannerKey(), MaxSdkBase.BannerPosition.BottomCenter);
                    MaxSdkCallbacks.Banner.OnAdClickedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.ycManager.analyticsManager.BannerClick();
                    };
                    MaxSdkCallbacks.Banner.OnAdLoadedEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.ycManager.analyticsManager.BannerShow();
                    };
                    MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (string adUnitId, MaxSdkBase.AdInfo adInfo) => {
                        this.ycManager.analyticsManager.GetSession().ads_banner.Add(this.OnAdRevenuePaidEvent(adUnitId, adInfo));
                    };
                    if (this.ycManager.ycConfig.BannerDisplayOnInit) {
                        this.HideBanner(false);
                    }
                }
            }

            public void HideBanner(bool hide) {
                if (this._isEnableBanner == true) {
                    if (this.IsAdsShow()) {
                        this._bannerHide = hide;
                        if (hide == true) {
                            MaxSdk.HideBanner(this.GetBannerKey());
                        } else {
                            MaxSdk.ShowBanner(this.GetBannerKey());
                        }
                        this.iBanner.gameObject.SetActive(!hide);
                    } else {
                        MaxSdk.HideBanner(this.GetBannerKey());
                        this.iBanner.gameObject.SetActive(false);
                    }
                } else {
                    this.ycManager.ycConfig.LogWarning("[Ads] not init");
                }
            }

            // ADS
            public bool IsAdsShow() {
                return this.ycManager.dataManager.GetAdsShow();
            }

            public void BuyAdsShow() {
                this.ycManager.dataManager.BuyAdsShow();
                this.HideBanner(true);
            }

            string GetRewardedKey() {
#if UNITY_ANDROID
                return this.ycManager.ycConfig.AndroidRewarded;
#elif UNITY_IPHONE
                return this.ycManager.ycConfig.IosRewarded;
#else
                return "";
#endif
            }

            string GetInterstitialKey() {
#if UNITY_ANDROID
                return this.ycManager.ycConfig.AndroidInterstitial;
#elif UNITY_IPHONE
                return this.ycManager.ycConfig.IosInterstitial;
#else
                return "";
#endif
            }

            string GetBannerKey() {
#if UNITY_ANDROID
                return this.ycManager.ycConfig.AndroidBanner;
#elif UNITY_IPHONE
                return this.ycManager.ycConfig.IosBanner;
#else
                return "";
#endif
            }


        }
    }
}