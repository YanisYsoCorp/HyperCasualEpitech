using UnityEngine;
using Facebook.Unity;

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
                        FB.ActivateApp();
                    }
                } else {
                    this.ycManager.ycConfig.LogWarning("[Facebook] not init");
                }
            }

            private void InitCallback() {
                if (this._isEnable == true) {
                    if (FB.IsInitialized) {
                        FB.ActivateApp();
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
