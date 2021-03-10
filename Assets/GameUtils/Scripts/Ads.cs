using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace YsoCorp {
    namespace GameUtils {

        public class Ads : BaseManager {

            public UtilsAds utilsAds;
            public VideoPlayer videoPlayer;
            public RawImage view;
            public Text nameGame;
            public Button button;
            public AdJson ad { get; set; } = null;
            private bool _willShow = false;
            private bool _needRequesting = true;

            protected override void Awake() {
                base.Awake();
                this._HideBox();
            }

            void _HideBox() {
                this.button.transform.GetComponent<RectTransform>().localPosition = Vector3.left * 10000f;
            }

            void _ShowBox() {
                this.button.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
                this.button.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            }

            void Start() {
                this.button.onClick.AddListener(() => {
                    this.utilsAds.ClickAd();
                    Application.OpenURL(this.ad.link);
                });
                this.ycManager.inAppManager.onPurchased.AddListener((string id) => {
                    if (id == this.ycManager.ycConfig.InAppRemoveAds) {
                        this.Hide();
                    }
                });
                this.RequestAd();
            }

            public void Hide() {
                if (this.button.gameObject.activeSelf) {
                    this.RequestAd();
                }
                this._willShow = false;
                this._HideBox();
            }

            public void Show() {
                if (this.ycManager.dataManager.GetAdsShow()) {
                    this._willShow = true;
                }
            }

            void Play() {
                this._ShowBox();
                this.utilsAds.DisplayAd();
                this.videoPlayer.Play();
                this._willShow = false;
                this._needRequesting = true;
            }

            void Update() {
                if (this._willShow && this.videoPlayer.isPrepared) {
                    this.Play();
                }
                if (this.button.gameObject.activeSelf) {
                    this.view.texture = this.videoPlayer.texture;
                }
            }

            public void RequestAd() {
                if (this._needRequesting == true) {
                    this._needRequesting = false;
                    this.utilsAds.RequestAd((AdJson a) => {
                        if (a != null) {
                            this.ad = a;
                            this.videoPlayer.url = this.ad.video;
                            this.nameGame.text = this.ad.name;
                            this.videoPlayer.Prepare();
                        } else {
                            this._needRequesting = true;
                            this.Invoke("RequestAd", 5f);
                        }
                    });
                }
            }

        }

    }

}
