using UnityEngine;
using MoreMountains.NiceVibrations;
using System.Collections.Generic;

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class VibrationManager : BaseManager {

            Dictionary<string, float> _vibrations = new Dictionary<string, float>();

            private void _Vibration(HapticTypes type, float delta, string key) {
                this._vibrations[key] = Time.time + delta;
                MMVibrationManager.Haptic(type);
#if UNITY_EDITOR
                if (this.ycManager.ycConfig.VibrationDebugLog) {
                    Debug.Log("[VIBRATION] " + type + " " + key);
                }
#endif
            }

            private void _VibrationCheck(HapticTypes type, float delta, string key) {
                if (this.ycManager.ycConfig.Vibration && this.ycManager.dataManager.GetVibration() == true) {
                    if (this._vibrations.ContainsKey(key)) {
                        if (Time.time >= this._vibrations[key]) {
                            this._Vibration(type, delta, key);
                        }
                    } else {
                        this._Vibration(type, delta, key);
                    }
                }
            }

            public void SoftImpact(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.SoftImpact, delta, "SoftImpact-" + key);
            }

            public void LightImpact(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.LightImpact, delta, "LightImpact-" + key);
            }

            public void MediumImpact(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.MediumImpact, delta, "MediumImpact-" + key);
            }

            public void HeavyImpact(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.HeavyImpact, delta, "HeavyImpact-" + key);
            }

            public void RigidImpact(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.RigidImpact, delta, "RigidImpact-" + key);
            }

            public void Failure(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.Failure, delta, "Failure-" + key);
            }

            public void Selection(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.Selection, delta, "Selection-" + key);
            }

            public void Success(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.Success, delta, "Success-" + key);
            }

            public void Warning(float delta = 0.03f, string key = "") {
                this._VibrationCheck(HapticTypes.Warning, delta, "Warning-" + key);
            }

            public void SmartImpact(float impact, float delta = 0.03f, string key = "") {
                if (impact > 0.66f) {
                    this.HeavyImpact(delta, key);
                } else if (impact > 0.33f) {
                    this.MediumImpact(delta, key);
                } else {
                    this.LightImpact(delta, key);
                }
            }
        }
    }
}