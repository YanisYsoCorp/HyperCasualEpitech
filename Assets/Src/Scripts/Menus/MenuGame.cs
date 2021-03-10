using UnityEngine.UI;

namespace YsoCorp {

    public class MenuGame : AMenu {

        public Button bBack;
        public Joystick joystick;

        void Start() {
            this.bBack.onClick.AddListener(() => {
                this.ycManager.adsManager.ShowInterstitial(() => {
                    this.game.state = Game.States.Home;
                });
            });
        }

    }

}
