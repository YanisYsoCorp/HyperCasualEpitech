using UnityEngine;
using Facebook.Unity;
using System.Runtime.InteropServices;
/*#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif*/

namespace YsoCorp {

    namespace GameUtils {
        [DefaultExecutionOrder(-10)]
        public class FBManager : BaseManager {

            private bool _isEnable = false;

            protected override void Awake() {
                base.Awake();
                if (this.ycManager.ycConfig.FbAppId != "") {
                    this._isEnable = true;
                    if (!FB.IsInitialized) {
                        FB.Init(this.InitCallback, this.OnHideUnity);
                    } else {
                        this.InitCallback();
                    }
                } else {
                    this.ycManager.ycConfig.LogWarning("[Facebook] not init");
                }
            }

            private void InitCallback() {
                if (this._isEnable == true) {
                    if (FB.IsInitialized) {
                        FB.ActivateApp();
#if UNITY_IOS
                        //bool status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED;
                        AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
                        FB.Mobile.SetAdvertiserTrackingEnabled(true);
#endif
                    } else {
                        Debug.Log("Failed to Initialize the Facebook SDK");
                    }
                }
            }

            private void OnHideUnity(bool isGameShown) {
                if (this._isEnable == true) {
                    if (!isGameShown) {
                        Time.timeScale = 0;
                    } else {
                        Time.timeScale = 1;
                    }
                }
            }
        }
    }

}

namespace AudienceNetwork {
    public static class AdSettings {

#if UNITY_IOS || UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);
#endif

        public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled) {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_IPHONE)
            FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
#endif
        }

    }
}

/*namespace Unity.Advertisement.IosSupport {
    public class ATTrackingStatusBinding {
#if UNITY_IOS
        [DllImport("__Internal")] private static extern void InterfaceTrackingAuthorizationRequest();
        [DllImport("__Internal")] private static extern int InterfaceGetTrackingAuthorizationStatus();
#endif

        public enum AuthorizationTrackingStatus {
            NOT_DETERMINED = 0,
            RESTRICTED,
            DENIED,
            AUTHORIZED
        }

        public static void RequestAuthorizationTracking() {
#if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                InterfaceTrackingAuthorizationRequest();
            }
#endif
        }

        public static AuthorizationTrackingStatus GetAuthorizationTrackingStatus() {
#if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                return (AuthorizationTrackingStatus)InterfaceGetTrackingAuthorizationStatus();
            }
#endif
            return AuthorizationTrackingStatus.NOT_DETERMINED;
        }
    }
}*/