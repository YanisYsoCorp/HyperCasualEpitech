using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace YsoCorp {

    public class UiGear : YCBehaviour {

        public class OnChange : UnityEvent<int> {}

        static public float SPEED = 0.1f;

        private Image _iLever;
        private Image _iKms;
        private Text _tKms;
        private Button _b0;
        private GameObject _gPositions;

        public OnChange onChange { get; set; } = new OnChange();

        protected override void Awake() {
            base.Awake();
            this._gPositions = this.gameObject.GetChildGameObjectByName("GPositions");
            this._iLever = this.gameObject.GetChildGameObjectByName("ILever").GetComponent<Image>();
            this._iKms = this.gameObject.GetChildGameObjectByName("IKmh").GetComponent<Image>();
            this._iKms.gameObject.SetActive(false);
            this._tKms = this._iKms.gameObject.GetChildGameObjectByName("TKmh").GetComponent<Text>();
            this._b0 = this.gameObject.GetChildGameObjectByNameRec("0").GetComponent<Button>();
            foreach (Button b in this.GetComponentsInChildren<Button>()) {
                b.onClick.AddListener(() => { this.OnClick(b); });
            }
        }

        public void SetLeverPosition(int i) {
            if (i > 0 && i <= 6) {
                this.OnClick(this._gPositions.GetComponentsInChildren<Button>()[i]);
            } 
        }

        void OnClick(Button b) {
            Sequence s = DOTween.Sequence();
            if (this._iLever.transform.position.x != b.transform.position.x) {
                s.Append(this._iLever.transform.DOMoveY(this._b0.transform.position.y, SPEED));
                s.Append(this._iLever.transform.DOMoveX(b.transform.position.x, SPEED));
            }
            s.Append(this._iLever.transform.DOMoveY(b.transform.position.y, SPEED));
            s.Play();
            this.onChange.Invoke(int.Parse(b.name));
        }

        public void Reset() {
            this._iLever.transform.position = this._b0.transform.position;
        }

        public void SetKmh(int kmh) {
            this._iKms.gameObject.SetActive(true);
            this._tKms.text = "" + kmh;
        }

    }

}
