using UnityEngine;
using System.Collections.Generic;

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class ABTestingManager : BaseManager {

            private static string PLAYER_SAMPLE = "YC_PLAYER_SAMPLE";

            Dictionary<string, bool> _results = new Dictionary<string, bool>();
            string _sample = null;

            protected override void Awake() {
                base.Awake();
            }

            public void Start() {
                Debug.Log("[AB Testing] : " + this.GetPlayerSample());
            }

            public bool ABSamplesContain(string[] abSamples, string test) {
                foreach (string sample in abSamples) {
                    if (sample.Equals(test) == true) {
                        return true;
                    }
                }
                return false;
            }

            private void SetSample(string sample) {
                if (sample == null) {
                    if (PlayerPrefs.HasKey(PLAYER_SAMPLE)) {
                        PlayerPrefs.DeleteKey(PLAYER_SAMPLE);
                    }
                } else {
                    PlayerPrefs.SetString(PLAYER_SAMPLE, sample);
                }
            }

            private string GetSample() {
                return PlayerPrefs.GetString(PLAYER_SAMPLE);
            }

            public bool IsSample() {
                return PlayerPrefs.HasKey(PLAYER_SAMPLE);
            }

            private string ConvertSample(string sample, bool allVersion = false) {
                if (allVersion) {
                    return sample.Trim();
                }
                return this.ycManager.ycConfig.ABVersion + "-" + sample.Trim();
            }

            private float GetABPercent() {
                return 1f / this.GetABSamples().Length;
            }

            private string[] GetABSamples() {
                List<string> abs = new List<string>();
                if (this.ycManager.ycConfig.ABSamples.Length > 0) {
                    abs.Add(this.ConvertSample("control"));
                    foreach (string ab in this.ycManager.ycConfig.ABSamples) {
                        abs.Add(this.ConvertSample(ab));
                    }
                }
                return abs.ToArray();
            }

            private void GenerateSample() {
                if (this.IsSample() == false) {
                    string[] abSamples = this.GetABSamples();
                    float r = Random.value;
                    string sample = "";
                    for (int i = 0; i < abSamples.Length; i++) {
                        if (r < (i + 1) * this.GetABPercent()) {
                            sample = abSamples[i];
                            break;
                        }
                    }
                    this.SetSample(sample);
                }
            }

            // PUBLIC
            public string GetPlayerSample() {
                if (this._sample == null) {
                    this.GenerateSample();
                    this._sample = this.GetSample();
                    if ((Debug.isDebugBuild || Application.isEditor) && this.ycManager.ycConfig.ABForcedSample != "") {
                        this._sample = this.ConvertSample(this.ycManager.ycConfig.ABForcedSample);
                    }
                }
                return this._sample;
            }

            public bool IsPlayerSample(string a) {
                if (this._results.ContainsKey(a) == false) {
                    this._results[a] = this.GetPlayerSample() == this.ConvertSample(a);
                }
                return this._results[a];
            }

            public bool IsPlayerSampleContaine(string a) {
                return this.GetPlayerSample().StartsWith(this.ycManager.ycConfig.ABVersion + "-") && this.GetPlayerSample().Contains(a);
            }
        }

    }
}
