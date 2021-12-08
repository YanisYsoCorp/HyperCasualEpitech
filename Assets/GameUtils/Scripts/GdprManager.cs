using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class GdprManager : BaseManager {

            [Serializable]
            public class Privacy {
                [YcReadOnly] public string label;
                [YcReadOnly] public string link;
                public bool display = false;

                public Privacy(string lab, string lin, bool display) {
                    this.label = lab;
                    this.link = lin;
                    this.display = display;
                }
            }

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

            private Privacy[] privacies = {
                new Privacy("AdColony", "https://www.adcolony.com/privacy-policy", true),
                new Privacy("Amazon", "https://advertising.amazon.com/resources/ad-policy/en/gdpr", false),
                new Privacy("AppLovin", "https://www.applovin.com/privacy/", true),
                new Privacy("Facebook", "https://m.facebook.com/about/privacy", true),
                new Privacy("Fyber", "https://www.fyber.com/Privacy-policy/", true),
                new Privacy("Google", "https://policies.google.com/privacy", true),
                new Privacy("InMobi", "https://www.inmobi.com/privacy-policy/", true),
                new Privacy("IronSource", "http://www.ironsrc.com/wp-content/uploads/2019/03/ironSource-Privacy-Policy.pdf", true),
                new Privacy("Line", "https://line.me/en/terms/policy/", true),
                new Privacy("Mintegral", "https://www.mintegral.com/en/privacy", true),
                new Privacy("MyTarget", "https://legal.my.com/us/mytarget/privacy/", true),
                new Privacy("ByteDance", "https://www.pangleglobal.com/privacy", true),
                new Privacy("Smaato", "https://www.smaato.com/privacy/", false),
                new Privacy("TapJoy", "https://www.tapjoy.com/legal/#privacy-policy", false),
                new Privacy("Tenjin", "https://www.tenjin.io/privacy/", true),
                new Privacy("TikTok", "https://www.tiktok.com/legal/privacy-policy", true),
                new Privacy("UnityAds", "https://unity3d.com/fr/legal/privacy-policy", true),
                new Privacy("Vungle", "https://vungle.com/privacy", true)
            };

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
                this.ycManager.dataManager.SetGdprAnalytics(true);
                this.ycManager.dataManager.SetGdprAds(true);
                this.HideAllSteps();
                this._action();
            }

            void InitPrivacies() {
                foreach (Privacy p in this.privacies) {
                    if (p.display == true) {
                        Button b = Instantiate(this.pfBPrivacy, this.gridPrivacies.transform);
                        b.gameObject.SetActive(true);
                        b.GetComponentInChildren<Text>().text = p.label;
                        b.onClick.AddListener(() => {
                            Application.OpenURL(p.link);
                        });
                    }
                }
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