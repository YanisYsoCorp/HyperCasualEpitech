using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class I18nResourcesManager : AResourcesManager {

            public Dictionary<string, Dictionary<string, string> > i18ns = new Dictionary<string, Dictionary<string, string>>();

            protected override void Awake() {
                base.Awake();
                Dictionary<string, TextAsset> texts = this.LoadDictionary<TextAsset>("I18n");
                foreach (KeyValuePair<string,TextAsset> t in texts) {
                    this.i18ns[t.Key] = JsonConvert.DeserializeObject<Dictionary<string, string>>(t.Value.text);
                    TextAsset jsonBase = this.Load<TextAsset>("I18nBase/" + t.Key);
                    if (jsonBase) {
                        Dictionary<string, string> bases = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonBase.text);
                        foreach (KeyValuePair<string, string> tBase in bases) {
                            if (this.i18ns[t.Key].ContainsKey(tBase.Key) == false) {
                                this.i18ns[t.Key][tBase.Key] = tBase.Value;
                            }
                        }
                    }
                }
            }

            public Dictionary<string, string> GetStrings(string lang) {
                if (this.i18ns.ContainsKey(lang)) {
                    return this.i18ns[lang];
                }
                return null;
            }

            public string GetString(string key, string lang) {
                Dictionary<string, string> strings = this.GetStrings(lang);
                if (strings != null && strings.ContainsKey(key)) {
                    return strings[key];
                }
                return key;
            }

            public Sprite GetSprite(string lang) {
                return this.Load<Sprite>("Sprites/I18n/" + lang);
            }
        }

    }
}