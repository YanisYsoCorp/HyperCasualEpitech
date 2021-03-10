using UnityEngine;
using UnityEngine.Animations;

namespace YsoCorp {

    public class Cam : YCBehaviour {

        public Camera ycCamera;
        public PositionConstraint constraint;
        public LookAtConstraint lookat;

        protected override void Awake() {
            base.Awake();
        }

        public void Follow(Transform target) {
            ConstraintSource source = this.constraint.GetSource(0);
            ConstraintSource sourceLook = this.lookat.GetSource(0);

            sourceLook.sourceTransform = target;
            sourceLook.weight = 1;
            source.sourceTransform = target;
            source.weight = 1;

            this.constraint.SetSource(0, source);
            this.lookat.SetSource(0, sourceLook);
        }

    }

}
