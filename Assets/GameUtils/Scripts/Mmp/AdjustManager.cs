using UnityEngine;
using com.adjust.sdk;

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class AdjustManager : MmpBaseManager {

            public Adjust adjust;

            public override void Init() {
                if (this.ycManager.ycConfig.MmpAdjustAppToken != "") {
                    AdjustConfig config = new AdjustConfig(this.ycManager.ycConfig.MmpAdjustAppToken, AdjustEnvironment.Production, true);
                    config.setLogLevel(AdjustLogLevel.Suppress);
                    Adjust.start(config);
                } else {
                    Debug.LogWarning("[ADJUST] you active adjust but you forget the AppToken");
                }
            }

            public override void SendEvent(string eventName) { }

            public override void SetConsent(bool consent) { }

        }

    }

}

