using UnityEngine;

namespace YsoCorp {

    public class ObstacleCollision : YCBehaviour {

        private void OnCollisionEnter(Collision collision) {
            if (collision.transform.CompareTag("Obstacle") == true) {
                this.KillPlayer(collision.transform);
            }
        }

        private void OnTriggerEnter(Collider collision) {
            if (collision.transform.CompareTag("Obstacle") == true) {
                this.KillPlayer(collision.transform);
            } else if (collision.transform.CompareTag("Finish") == true) {
                this.Finish();
            }
        }

        private void KillPlayer(Transform killer) {
            this.player.Die(killer);
            this.game.Lose();
        }

        private void Finish() {
            this.game.Win();
        }

    }

}