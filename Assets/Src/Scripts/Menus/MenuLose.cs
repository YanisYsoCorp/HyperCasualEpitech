using UnityEngine.UI;

namespace YsoCorp {

    public  class MenuLose : AMenu {

        public Button bRetry;

        void Start() {
            this.bRetry.onClick.AddListener(() => {
                this.ycManager.adsManager.ShowInterstitial(() => {
                    this.game.state = Game.States.Home;
                });
            });
        }

    }

}
