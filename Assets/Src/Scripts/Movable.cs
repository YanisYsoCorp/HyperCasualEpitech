using DG.Tweening;
using UnityEngine;

namespace YsoCorp {
    public class Movable : YCBehaviour {
        public virtual void GesturePanDelta(float x, float y) { }
        public virtual void GesturePanDelta(float x, float y, DigitalRubyShared.GestureRecognizer pan) { }
        public virtual void GesturePanDeltaX(float x) { }
        public virtual void GesturePanDeltaX(float x, DigitalRubyShared.GestureRecognizer pan) { }
        public virtual void GesturePanDeltaY(float y) { }
        public virtual void GesturePanDeltaY(float y, DigitalRubyShared.GestureRecognizer pan) { }
        public virtual void GesturePanDown() { }
        public virtual void GesturePanDown(DigitalRubyShared.GestureRecognizer pan) { }
        public virtual void GesturePanUp() { }
        public virtual void GesturePanUp(DigitalRubyShared.GestureRecognizer pan) { }
        public virtual void GesturePanDistance(float distanceX, float distanceY) { }
        public virtual void GesturePanPress(float focusX, float focusY) { }
    }
}