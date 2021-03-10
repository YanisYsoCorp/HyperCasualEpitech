using UnityEngine;

namespace YsoCorp {

    public class Joystick : Movable {

        public Transform tapPos;

        public override void GesturePanPress(float focusX, float focusY) {
            this.tapPos.position = new Vector2(focusX, focusY);
            
            if (this.gameObject.activeSelf == false) {
                this.gameObject.SetActive(true);
                this.transform.position = this.tapPos.position;
            }
            float maxValue = 100f;
            Vector3 direction = this.transform.position - this.tapPos.position;
            if (direction.magnitude > maxValue) {
                this.tapPos.position = this.transform.position - direction.normalized * maxValue;
            }
        }

        public override void GesturePanUp() {
            this.gameObject.SetActive(false);
        }

    }

}
