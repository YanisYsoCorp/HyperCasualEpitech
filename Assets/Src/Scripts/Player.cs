using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;

namespace YsoCorp {

    public class Player : Movable {

        private static float LEFT_LIMIT = -1.5f;
        private static float RIGHT_LIMIT = 1.5f;
        private static float SPEED_ROTATION = 25f;
        private static float SPEED_ACCELERATION = 0.5f;
        private static float SPEED = 4f;
        private static float ROTATION_SENSITIVITY = 0.2f;
        private static float MOVE_SENSITIVITY = 0.01f; 
        private static float TELEPORTATION_SENSITIVITY = 80f; 
        private static float TELEPORTATION_OFFSET = 1.5f;
        private static float MAX_ANGLE = 25f;

        public bool clampRotation;
        public bool slide;

        private bool _isMoving;
        private Vector3 _slideMove;
        private Animator _animator;
        private Quaternion _rotation;
        private Rigidbody _rigidbody;
        private RagdollBehaviour _ragdollBehviour;
        private TweenerCore<float, float, FloatOptions> _rotationTween;

        public bool isAlive { get; protected set; }
        public float speed { get; private set; }

        protected override void Awake() {
            this._rigidbody = this.GetComponent<Rigidbody>();
            this._animator = this.GetComponentInChildren<Animator>();
            this._ragdollBehviour = this.GetComponent<RagdollBehaviour>();
            this.isAlive = true;

            this.game.onStateChanged += this.Launch;
        }

        private void Launch(Game.States states) {
            if (states == Game.States.Playing) {
                this._isMoving = true;
                this._animator?.SetBool("Moving", true);
            } else if (states == Game.States.Win) {
                this._isMoving = false;
                this._animator?.SetBool("Moving", false);
                this._animator?.SetTrigger("Win");
            } else if (states == Game.States.Lose) {
                this._isMoving = false;
            }
        }

        public void Reset() {
            this.isAlive = true;
            this._isMoving = false;
            if (this._animator != null) {
                this._animator.enabled = true;
                this._animator.SetBool("Moving", false);
            }

            Transform spot = this.game.map.GetStartingPos();
            this.transform.position = spot.position;
            this.transform.rotation = spot.rotation;
            this._rigidbody.velocity = Vector3.zero;
            this._ragdollBehviour?.Reset();
            this.cam.Follow(this.transform);

            this._rotation = Quaternion.Euler(0f, this.transform.rotation.eulerAngles.y, 0);
        }

        public void Die(Transform killer) {
            this.isAlive = false;
            if (this._ragdollBehviour != null) {
                this._ragdollBehviour.EnableRagdoll(killer);
                this.cam.Follow(this._ragdollBehviour.hips);
            }
        }

        private void FixedUpdate() {
            if (this.game.state != Game.States.Playing || this.isAlive == false) {
                return;
            }

            if (this._isMoving == true) {
                this.speed += SPEED_ACCELERATION;
            } else {
                this.speed -= SPEED_ACCELERATION * 3f;
            }

            this.speed = Mathf.Clamp(this.speed, 0, SPEED);
            if (this.speed != 0) {
                this._rigidbody.MovePosition(this._rigidbody.position + this.transform.forward * this.speed * Time.fixedDeltaTime + this._slideMove);
                this._rigidbody.MoveRotation(Quaternion.RotateTowards(this._rigidbody.rotation, this._rotation, SPEED_ROTATION));
                this._slideMove = Vector3.zero;
            }
        }

        public override void GesturePanDown() {
            if (this.game.state != Game.States.Playing || this.isAlive == false) {
                return;
            }

            this._rotationTween.Kill();

            this._isMoving = true;
        }

        public override void GesturePanDeltaX(float deltaX) {
            if (this.game.state != Game.States.Playing || this.isAlive == false) {
                return;
            }

            if (this.clampRotation == true) {
                this.RotateClamped(deltaX);
            } else if (this.slide == true) {
                this.Slide(deltaX);
            } else {
                this.Teleport(deltaX);
            }
        }

        private void Slide(float deltaX) {
            this._slideMove = new Vector3(deltaX * MOVE_SENSITIVITY, 0 , 0);
        }

        private bool CheckFall(Vector3 movement) {
            Vector3 nextPos = this._rigidbody.position + movement;
            return nextPos.x < LEFT_LIMIT || nextPos.x > RIGHT_LIMIT;
        }

        private void Teleport(float deltaX) {
            Vector3 movement = this.transform.right * TELEPORTATION_OFFSET * Mathf.Sign(deltaX);

            if (Mathf.Abs(deltaX) > TELEPORTATION_SENSITIVITY && this.CheckFall(movement) == false) {
                this._rigidbody.MovePosition(this._rigidbody.position + movement);
            }
        }

        public override void GesturePanUp() {
            if (this.game.state != Game.States.Playing || this.isAlive == false) {
                return;
            }

            this.ResetRotation();
        }

        private void ResetRotation() {
            float duration = this._rotation.y / 40;
            this._rotationTween.Kill();

            this._rotationTween = DOTween.To(
                () => this._rotation.y,
                (value) => this._rotation = Quaternion.Euler(0f, value, 0),
                0, duration).SetEase(Ease.Linear);
        }

        public void RotateClamped(float deltaX) {
            float eulerAngle = transform.localEulerAngles.y;
            
            eulerAngle = (eulerAngle > 180) ? eulerAngle - 360 : eulerAngle;
            
            float angle = eulerAngle + deltaX * ROTATION_SENSITIVITY;
            float clampedAngle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE);

            this._rotation = Quaternion.Euler(0f, clampedAngle, 0);
        }

    }

}