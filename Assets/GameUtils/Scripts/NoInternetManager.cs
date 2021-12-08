using UnityEngine;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class NoInternetManager : BaseManager {

            public GameObject canvas;

            protected override void Awake() {
                base.Awake();
                this.canvas.SetActive(false);
            }

            public void CheckInternet() {
                bool internetNotAvailable = Application.internetReachability == NetworkReachability.NotReachable;
                this.canvas.SetActive(internetNotAvailable && this.ycManager.dataManager.GetAdsShow());
            }

        }
    }
}
