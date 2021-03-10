using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class I18nManager : BaseManager {
            
            public I18nResourcesManager i18NResourcesManager;

            public UnityEvent onChange = new UnityEvent();

            private List<I18nElement> _elements = new List<I18nElement>();

            protected override void Awake() {
                base.Awake();
                this.InitLanguage();
            }

            public string GetString(string key) {
                string value = this.i18NResourcesManager.GetString(key, this.ycManager.dataManager.GetLanguage());
                if (value != null && value != "") {
                    return value;
                }
                return key;
            }

            public Sprite GetCurrentStrite() {
                return this.i18NResourcesManager.GetSprite(this.ycManager.dataManager.GetLanguage());
            }

            public string GetLanguage() {
                string lang = this.ycManager.dataManager.GetLanguage();
                if (this.i18NResourcesManager.i18ns.ContainsKey(lang)) {
                    return lang;
                }
                return "EN";
            }

            public void NextLanguages() {
                string[] keys = this.i18NResourcesManager.i18ns.Keys.ToArray();
                this.SetLanguage(keys[(Array.IndexOf(keys, this.ycManager.dataManager.GetLanguage()) + 1) % keys.Length]);
                foreach (I18nElement i18 in this._elements) {
                    i18.UpdateText();
                }
                this.onChange.Invoke();
            }

            void SetLanguage(string lang) {
                this.ycManager.dataManager.SetLanguage(lang);
            }

            public void AddElement(I18nElement i18n) {
                this._elements.Add(i18n);
            }

            public void DelElement(I18nElement i18n) {
                this._elements.Remove(i18n);
            }

            private void InitLanguage() {
                if (this.ycManager.dataManager.HasLanguage() == false) {
                    string res = "EN";
                    switch (Application.systemLanguage) {
                    case SystemLanguage.Afrikaans: res = "AF"; break;
                    case SystemLanguage.Arabic: res = "AR"; break;
                    case SystemLanguage.Basque: res = "ES"; break; // EU
                    case SystemLanguage.Belarusian: res = "RU"; break; // BY
                    case SystemLanguage.Bulgarian: res = "BG"; break;
                    case SystemLanguage.Catalan: res = "ES"; break; // CA
                    case SystemLanguage.Chinese: res = "ZH"; break;
                    case SystemLanguage.ChineseSimplified: res = "ZH"; break;
                    case SystemLanguage.ChineseTraditional: res = "ZH"; break;
                    case SystemLanguage.Czech: res = "CS"; break;
                    case SystemLanguage.Danish: res = "DA"; break;
                    case SystemLanguage.Dutch: res = "NL"; break;
                    case SystemLanguage.English: res = "EN"; break;
                    case SystemLanguage.Estonian: res = "ET"; break;
                    case SystemLanguage.Faroese: res = "FO"; break;
                    case SystemLanguage.Finnish: res = "FI"; break;
                    case SystemLanguage.French: res = "FR"; break;
                    case SystemLanguage.German: res = "DE"; break;
                    case SystemLanguage.Greek: res = "EL"; break;
                    case SystemLanguage.Hebrew: res = "IW"; break;
                    case SystemLanguage.Hungarian: res = "HU"; break;
                    case SystemLanguage.Icelandic: res = "IS"; break;
                    case SystemLanguage.Indonesian: res = "IN"; break;
                    case SystemLanguage.Italian: res = "IT"; break;
                    case SystemLanguage.Japanese: res = "JA"; break;
                    case SystemLanguage.Korean: res = "KO"; break;
                    case SystemLanguage.Latvian: res = "LV"; break;
                    case SystemLanguage.Lithuanian: res = "LT"; break;
                    case SystemLanguage.Norwegian: res = "NO"; break;
                    case SystemLanguage.Polish: res = "PL"; break;
                    case SystemLanguage.Portuguese: res = "PT"; break;
                    case SystemLanguage.Romanian: res = "RO"; break;
                    case SystemLanguage.Russian: res = "RU"; break;
                    case SystemLanguage.SerboCroatian: res = "SH"; break;
                    case SystemLanguage.Slovak: res = "SK"; break;
                    case SystemLanguage.Slovenian: res = "SL"; break;
                    case SystemLanguage.Spanish: res = "ES"; break;
                    case SystemLanguage.Swedish: res = "SV"; break;
                    case SystemLanguage.Thai: res = "TH"; break;
                    case SystemLanguage.Turkish: res = "TR"; break;
                    case SystemLanguage.Ukrainian: res = "RU"; break;
                    case SystemLanguage.Unknown: res = "EN"; break;
                    case SystemLanguage.Vietnamese: res = "VI"; break;
					}
					this.SetLanguage(res);
                }
            }

        }

    }
}