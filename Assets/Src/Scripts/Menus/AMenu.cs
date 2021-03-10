using UnityEngine;

namespace YsoCorp {

    public abstract class AMenu : YCBehaviour {

        public virtual void Display() {
            this.gameObject.SetActive(true);
        }

        public virtual void Hide() {
            this.gameObject.SetActive(false);
        }

    }

}
