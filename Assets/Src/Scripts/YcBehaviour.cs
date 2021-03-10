using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YsoCorp {
    public class YCBehaviour : MonoBehaviour {

        private static Game GAME;
        private static DataManager DATAMANAGER;
        private static ResourcesManager RESOURCESMANAGER;
        private static GameUtils.YCManager YCMANAGER;
        private static Player PLAYER;
        private static Cam CAM;

        public Game game { get { return GAME; } private set { } }
        public DataManager dataManager { get { return DATAMANAGER; } private set { } }
        public ResourcesManager resourcesManager { get { return RESOURCESMANAGER; } private set { } }
        public GameUtils.YCManager ycManager { get { return YCMANAGER; } private set { } }
        public Player player { get { return PLAYER; } private set { } }
        public Cam cam { get { return CAM; } private set { } }

        protected bool isQuitting = false;

        virtual protected void Awake() {
            YCMANAGER = GameUtils.YCManager.instance;
            if (this.IsInit() == false) {
                foreach (GameObject g in SceneManager.GetActiveScene().GetRootGameObjects()) {
                    if (GAME == null) {
                        GAME = g.GetComponentInChildren<Game>(true);
                    }
                    if (DATAMANAGER == null) {
                        DATAMANAGER = g.GetComponentInChildren<DataManager>(true);
                    }
                    if (RESOURCESMANAGER == null) {
                        RESOURCESMANAGER = g.GetComponentInChildren<ResourcesManager>(true);
                    }
                    if (PLAYER == null) {
                        PLAYER = g.GetComponentInChildren<Player>(true);
                    }
                    if (CAM == null) {
                        CAM = g.GetComponentInChildren<Cam>(true);
                    }
                    if (this.IsInit()) {
                        return;
                    }
                }
            }

        }

        protected virtual void OnApplicationQuit() {
            this.isQuitting = true;
        }

        protected virtual void OnDestroy() {
            if (this.isQuitting == false) {
                this.OnDestroyNotQuitting();
            }
        }

        protected virtual void OnDestroyNotQuitting() { }

        bool IsInit() {
            return GAME != null && DATAMANAGER != null && RESOURCESMANAGER != null && PLAYER != null && CAM != null;
        }

        private IEnumerator _InvokeCallback(float delay, Action lambda, bool unscaleTIme) {
            if (unscaleTIme == true) {
                yield return new WaitForSecondsRealtime(delay);
            } else {
                yield return new WaitForSeconds(delay);
            }
            lambda();
        }

        public void InvokeCallback(float delay, Action lambda, bool unscaleTIme = false) {
            this.StartCoroutine(this._InvokeCallback(delay, lambda, unscaleTIme));
        }

        public float ScreenScaleW() {
            return Screen.width / 1248f;
        }

        public float ScreenScaleH() {
            return Screen.height / 2688f;
        }

    }
}
