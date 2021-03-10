using UnityEngine;
using System.Collections.Generic;

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class TenjinManager : MmpBaseManager {

            private static string API_KEY = "BP2IBD5EPSJLBT2JYHWDGTVGXQVF6YGK";

            private BaseTenjin _tenjin;

            public override void Init() {
                if (this._tenjin == null) {
                    this._tenjin = Tenjin.getInstance(API_KEY);
                    this._tenjin.Connect();
                }
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

        }
    }
}

