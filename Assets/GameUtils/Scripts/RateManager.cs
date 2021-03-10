using UnityEngine;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class RateManager : BaseManager {

            protected override void Awake() {
                base.Awake();
            }

            private void Start() {
                if (this.ycManager.ycConfig.RateBoxShow) {
                    RateGame.Instance.RateGameSettings.iosAppID = this.ycManager.ycConfig.appleId;
                    RateGame.Instance.RateGameSettings.googlePlayBundleID = this.ycManager.ycConfig.GetAndroidId();
                }
            }

            public bool IsShow() {
                return RateGame.Instance.RatePopup != null;
            }

            public void OnGameFinished(bool levelComplete) {
                if (this.ycManager.ycConfig.RateBoxShow && levelComplete) {
                    this.Show();
                }
            }

            public void Show(bool force = false) {
                if (force) {
                    RateGame.Instance.ForceShowRatePopup();
                } else {
                    RateGame.Instance.ShowRatePopup();
                }
            }
        }
    }
}
