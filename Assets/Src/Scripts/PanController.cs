using DigitalRubyShared;

namespace YsoCorp {

    public class PanController : YCBehaviour {

        private Movable[] _movables;
        private PanGestureRecognizer _pan;

        protected override void OnDestroy() {
            base.OnDestroy();
            FingersScript.Instance.RemoveGesture(this._pan);
        }

        public void Reset() {
            this._movables = this.game.GetComponentsInChildren<Movable>();
            FingersScript.Instance.ShowTouches = false;
            this.InitPan();
        }

        void InitPan() {
            FingersScript.Instance.RemoveGesture(this._pan);
            this._pan = new PanGestureRecognizer();
            this._pan.ThresholdUnits = 0f;
            this._pan.StateUpdated += (GestureRecognizer gesture) => {
                foreach (Movable movable in this._movables) {
                    if (gesture.State == GestureRecognizerState.Began) {
                        movable.GesturePanDown();
                        movable.GesturePanDown(this._pan);
                    } else if (gesture.State == GestureRecognizerState.Executing) {
                        float deltaX = this._pan.DeltaX / this.ScreenScaleH();
                        float deltaY = this._pan.DeltaY / this.ScreenScaleH();
                        float distanceX = this._pan.DistanceX / this.ScreenScaleH();
                        float distanceY = this._pan.DistanceY / this.ScreenScaleH();
                        float focusX = this._pan.FocusX;
                        float focusY = this._pan.FocusY;
                        movable.GesturePanPress(focusX, focusY);
                        movable.GesturePanDistance(distanceX, distanceY);
                        movable.GesturePanDelta(deltaX, deltaY);
                        movable.GesturePanDelta(deltaX, deltaY, this._pan);
                        movable.GesturePanDeltaX(deltaX);
                        movable.GesturePanDeltaX(deltaX, this._pan);
                        movable.GesturePanDeltaY(deltaY);
                        movable.GesturePanDeltaY(deltaY, this._pan);
                    } else if (gesture.State == GestureRecognizerState.Ended) {
                        movable.GesturePanUp();
                        movable.GesturePanUp(this._pan);
                    }
                }
            };
            FingersScript.Instance.AddGesture(this._pan);
        }
    }
}