using UnityEngine;
using UnityEngine.UI;

namespace YsoCorp {
    namespace GameUtils {

        public class I18nElement : BaseManager {

            string _key;
            Text _text;

            protected override void Awake() {
                base.Awake();
                this._text = this.GetComponent<Text>();
                this._key = this._text.text;
                this.UpdateText();
                this.ycManager.i18nManager.AddElement(this);
            }

            protected override void OnDestroyNotQuitting() {
                this.ycManager.i18nManager.DelElement(this);
            }

            public void UpdateText() {
                this._text.text = this.ycManager.i18nManager.GetString(this._key);
            }

        }

    }
}