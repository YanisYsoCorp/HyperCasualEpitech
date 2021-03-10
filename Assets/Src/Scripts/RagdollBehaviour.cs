using UnityEngine;

namespace YsoCorp {

    public class RagdollBehaviour : YCBehaviour {

        public float force_up = 55;
        public float force = 100;
        public Transform hips;
        public Transform playerHips;
        public GameObject ragdoll;

        public void Reset() {
            this.ragdoll.SetActive(false);
            this.playerHips.parent.gameObject.SetActive(true);
        }

        public void EnableRagdoll(Transform target) {
            this.ragdoll.transform.position = this.transform.position;
            this.ragdoll.SetActive(true);
            this.playerHips.parent.gameObject.SetActive(false);

            this.RotateRagdollBones();
            this.ActivatePhysic();
            this.AddForce(target);
        }

        private void AddForce(Transform target) {
            Rigidbody rigid = hips.GetComponent<Rigidbody>();
            Vector3 forceDir = (this.transform.position - target.position).normalized;
            rigid.AddForce(Vector3.up * this.force_up + forceDir * this.force, ForceMode.Impulse);
        }

        private void ActivatePhysic() {
            Rigidbody[] rbs = this.ragdoll.GetComponentsInChildren<Rigidbody>();
            Collider[] colliders = this.ragdoll.GetComponentsInChildren<Collider>();
            foreach (Rigidbody rb in rbs) {
                rb.velocity = Vector3.zero;
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            foreach (Collider collider in colliders) {
                collider.enabled = true;
            }
        }

        private void RotateRagdollBones() {
            Transform ragdollBody = this.hips;
            Transform body = this.playerHips;

            this.SetBone(body, ragdollBody);
        }

        private void SetBone(Transform body, Transform copyBone) {
            copyBone.position = body.position;
            copyBone.rotation = body.rotation;

            if (body.childCount == 0 || copyBone.childCount == 0) {
                return;
            }

            int i = 0;
            foreach (Transform bone in body) {
                while (i < copyBone.childCount &&
                    copyBone.GetChild(i).CompareTag("NotRagdoll")) {
                    i++;
                }
                if (i >= copyBone.childCount) {
                    return;
                }
                this.SetBone(bone, copyBone.GetChild(i));
                i++;
            }
        }

    }

}