using UnityEngine;

namespace YsoCorp {

    public class Map : YCBehaviour {

        public Transform playerPos;

        protected override void Awake() {
            base.Awake();
            this.game.menuHome.levelIndex.text = "Level " + this.resourcesManager.GetMapNumber();
        }

        public Transform GetStartingPos() {
            return this.playerPos;
        }

    }

}
