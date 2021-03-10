using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace YsoCorp {


    public class UiJoystick : YCBehaviour {

        static float MAX_DISTANCE = 200f;

        public enum e_Action {
            BEGAN,
            EXECUTING,
            ENDED,
        }

        public class OnMove : UnityEvent<Vector2, e_Action> {}

        public bool isStatic = false;

        [Header("Not touch")]
        public Image iBg;
        public Image iPoint;

        public DigitalRubyShared.PanGestureRecognizer pan { get; set; }
        public OnMove onMove { get; set; } = new OnMove();
        public OnMove onMoveV3 { get; set; } = new OnMove();

        private bool _isPan;
        private Vector2 _startPos;
        private Vector2 _pos;

        protected override void Awake() {
            base.Awake();
            this.iBg.gameObject.SetActive(false);
            this.pan = new DigitalRubyShared.PanGestureRecognizer();
            this.pan.ThresholdUnits = 0f;
            this.pan.StateUpdated += (DigitalRubyShared.GestureRecognizer g) => {
                if (this.gameObject.activeInHierarchy == true) {
                    if (this.pan.State == DigitalRubyShared.GestureRecognizerState.Began) {
                        this._isPan = true;
                        this._startPos = new Vector2(this.pan.FocusX, this.pan.FocusY);
                        this._pos = this._startPos;
                        this.UpdateUI(e_Action.BEGAN);
                    } else if (this.pan.State == DigitalRubyShared.GestureRecognizerState.Executing) {
                        this._pos = new Vector2(this.pan.FocusX, this.pan.FocusY);
                        this.UpdateUI(e_Action.EXECUTING);
                    } else if (this.pan.State == DigitalRubyShared.GestureRecognizerState.Ended)  {
                        if (this._isPan) {
                            this._isPan = false;
                            this.UpdateUI(e_Action.ENDED);
                        }
                    }
                }
            };
            DigitalRubyShared.FingersScript.Instance.AddGesture(this.pan);
        }

        float GetMaxDistance() {
            return MAX_DISTANCE * this.ScreenScaleH();
        }

        private void UpdateUI(e_Action action) {
            this.iBg.gameObject.SetActive(this._isPan);
            if (this.isStatic == false) {
                float d = Vector3.Distance(this._startPos, this._pos);
                if (d > this.GetMaxDistance()) {
                    this._startPos = this._pos + Vector2.ClampMagnitude(this._startPos - this._pos, this.GetMaxDistance());
                }
            }
            this.iBg.transform.position = this._startPos;
            this.iPoint.transform.position = this._startPos + Vector2.ClampMagnitude(this._pos - this._startPos, this.GetMaxDistance());
            this.onMove.Invoke((this._pos - this._startPos) / this.ScreenScaleH() / MAX_DISTANCE, action);
        }
    }

}
