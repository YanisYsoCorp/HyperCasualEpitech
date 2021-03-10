using UnityEngine;

namespace YsoCorp {
    public class Shortcut : YCBehaviour {

#if UNITY_EDITOR
        private void Update() {
            if (Input.GetKeyDown("r")) {
                this.dataManager.DeleteAll();
            }
            if (Input.GetKeyDown("a")) {
                this.dataManager.PrevLevel();
                this.game.state = Game.States.Home;
                this.game.state = Game.States.Playing;
            }
            if (Input.GetKeyDown("z")) {
                this.dataManager.NextLevel();
                this.game.state = Game.States.Home;
                this.game.state = Game.States.Playing;
            }
            if (Input.GetKeyDown("w")) {
                if (this.game.state == Game.States.Playing) {
                    this.game.state = Game.States.Win;
                }
            }
            if (Input.GetKeyDown("l")) {
                if (this.game.state == Game.States.Playing) {
                    this.game.state = Game.States.Lose;
                }
            }
        }
#endif
    }
}
