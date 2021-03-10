using UnityEngine;

namespace YsoCorp {

    namespace GameUtils {

        public class MmpBaseManager : BaseManager {

            public virtual void Init() { }
            public virtual void SendEvent(string eventName) { }
            public virtual void SetConsent(bool consent) { }

        }

    }

}

