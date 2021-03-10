using System;
using UnityEngine;

namespace YsoCorp {
    public class Game : YCBehaviour {

        public enum States {
            None,
            Home,
            Playing,
            Lose,
            Win,
        }

        private States _state = States.None;
        public States state {
            get {
                return this._state;
            }
            set {
                if (this._state != value) {
                    this._state = value;
                    if (this.onStateChanged != null) {
                        this.onStateChanged(value);
                    }
                    if (value == States.Home) {
                        this.HideAllMenus();
                        this.menuHome.Display();
                        this.Reset();
                    } else if (value == States.Playing) {
                        this.ycManager.OnGameStarted(this.dataManager.GetLevel());
                        this.HideAllMenus();
                        this.menuGame.Display();
                    } else if (value == States.Lose) {
                        this.ycManager.OnGameFinished(false);
                        this.HideAllMenus();
                        this.menuLose.Display();
                    } else if (value == States.Win) {
                        this.ycManager.OnGameFinished(true);
                        this.dataManager.NextLevel();
                        this.HideAllMenus();
                        this.menuWin.Display();
                    }
                }
            }
        }

        public MenuHome menuHome;
        public MenuGame menuGame;
        public MenuLose menuLose;
        public MenuWin menuWin;

        public Transform trash;

        public event Action<States> onStateChanged;

        public Map map { get; set; } = null;

        private void Start() {
            this.game.state = States.Home;
        }

        public void Win() {
            this.game.state = States.Win;
        }

        public void Lose() {
            this.game.state = States.Lose;
        }

        private void ResetTrash() {
            if (this.trash != null) {
                DestroyImmediate(this.trash.gameObject);
                DestroyImmediate(this.map?.gameObject);
            }
            this.trash = new GameObject().transform;
            this.trash.name = "Trash";
        }

        public void Reset() {
            this.ResetTrash();

            this.map = Instantiate(this.resourcesManager.GetMap(), this.transform).GetComponent<Map>();
            this.player.Reset();
            this.GetComponent<PanController>().Reset();
        }

        void HideAllMenus() {
            this.menuHome.Hide();
            this.menuGame.Hide();
            this.menuLose.Hide();
            this.menuWin.Hide();
        }

    }
}