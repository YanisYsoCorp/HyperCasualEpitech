using UnityEngine;
using System.Collections.Generic;

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class MmpManager : BaseManager {

            public AdjustManager adjustManager;
            public TenjinManager tenjinManager;

            private List<MmpBaseManager> _mmps = new List<MmpBaseManager>();


            protected override void Awake() {
                base.Awake();
                if (this.ycManager.ycConfig.MmpAdjust) {
                    this._mmps.Add(this.adjustManager);
                }
                if (this.ycManager.ycConfig.MmpTenjin) {
                    this._mmps.Add(this.tenjinManager);
                }
            }

            public void Init() {
                foreach (MmpBaseManager mmp in this._mmps) {
                    mmp.gameObject.SetActive(true);
                    mmp.Init();
                }
            }

            public void SendEvent(string eventName) {
                foreach (MmpBaseManager mmp in this._mmps) {
                    mmp.SendEvent(eventName);
                }
            }
            public void SetConsent(bool consent) {
                foreach (MmpBaseManager mmp in this._mmps) {
                    mmp.SetConsent(consent);
                }
            }

        }

    }

}

