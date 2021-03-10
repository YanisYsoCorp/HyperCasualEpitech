using UnityEngine;
using UnityEngine.UI;
using System;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class GdprManager : BaseManager {

            public Canvas canvas;
            public Button pfBPrivacy;
            public GameObject gridPrivacies;

            public GameObject step1;
            public GameObject step2;
            public GameObject step3;

            public Button bS1Accept;
            public Button bS1No;
            public Button bS2Accept;
            public Button bS2Next;
            public Button bS3Accept;
            public Button bS3Back;
            public Toggle togAnalytics;
            public Toggle togAds;

            public Text[] texts;

            private Action _action;

            protected override void Awake() {
                base.Awake();
                this.InitPrivacies();
                this.HideAllSteps();
                foreach (Text t in this.texts) {
                    t.text = t.text.Replace("[GAME]", Application.productName);
                }
                this.bS1Accept.onClick.AddListener(this.Accept);
                this.bS1No.onClick.AddListener(() => {
                    this.ShowStep2();
                });

                this.bS2Accept.onClick.AddListener(this.Accept);
                this.bS2Next.onClick.AddListener(() => {
                    this.ShowStep3();
                });

                this.bS3Accept.onClick.AddListener(() => {
                    this.ycManager.dataManager.SetGdprValidate();
                    this.ycManager.dataManager.SetGdprAnalytics(this.togAnalytics.isOn);
                    this.ycManager.dataManager.SetGdprAds(this.togAds.isOn);
                    this.HideAllSteps();
                    this._action();
                });
                this.bS3Back.onClick.AddListener(() => {
                    this.ShowStep2();
                });
            }

            void Accept() {
                this.ycManager.dataManager.SetGdprValidate();
                this.ycManager.dataManager.SetGdprAnalytics(true);
                this.ycManager.dataManager.SetGdprAds(true);
                this.HideAllSteps();
                this._action();
            }

            void InitPrivacies() {
                this.pfBPrivacy.gameObject.SetActive(true);
                foreach (YCConfig.Privacy p in this.ycManager.ycConfig.GetGdprPrivacies()) {
                    if (p.display == true) {
                        Button b = Instantiate<Button>(this.pfBPrivacy, this.gridPrivacies.transform);
                        b.GetComponentInChildren<Text>().text = p.label;
                        b.onClick.AddListener(() => {
                            Application.OpenURL(p.link);
                        });
                    }
                }
                this.pfBPrivacy.gameObject.SetActive(false);
            }

            void ShowStep(GameObject step) {
                this.HideAllSteps();
                this.canvas.gameObject.SetActive(true);
                step.SetActive(true);
            }

            public void Show(Action action) {
                this.togAnalytics.isOn = this.ycManager.dataManager.GetGdprAnalytics();
                this.togAds.isOn = this.ycManager.dataManager.GetGdprAds();
                this._action = action;
                this.ShowStep(this.step1);
            }

            void ShowStep2() {
                this.ShowStep(this.step2);
            }

            void ShowStep3() {
                this.ShowStep(this.step3);
            }

            void HideAllSteps() {
                this.canvas.gameObject.SetActive(false);
                this.step1.SetActive(false);
                this.step2.SetActive(false);
                this.step3.SetActive(false);
            }

        }
    }
}