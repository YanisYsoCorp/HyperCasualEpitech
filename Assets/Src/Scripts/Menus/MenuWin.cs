using UnityEngine.UI;

namespace YsoCorp {

    public  class MenuWin : AMenu {

        public Button bNext;

        void Start() {
            this.bNext.onClick.AddListener(() => {
                this.ycManager.adsManager.ShowInterstitial(() => {
                    this.game.state = Game.States.Home;
                });
            });
        }

    }

}
