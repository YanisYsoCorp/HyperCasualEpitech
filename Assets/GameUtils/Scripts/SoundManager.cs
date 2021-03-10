using UnityEngine;
using System.Collections.Generic;

namespace YsoCorp {

    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class SoundResourcesManager : AResourcesManager {

            public Dictionary<string, AudioClip> effects;
            public Dictionary<string, AudioClip> musics;

            protected override void Awake() {
                base.Awake();
                this.effects = this.LoadDictionary<AudioClip>("Sounds/Effects");
                this.musics = this.LoadDictionary<AudioClip>("Sounds/Musics");
            }

        }

        [DefaultExecutionOrder(-10)]
        public class SoundManager : BaseManager {

            public class SoundElement {
                public AudioSource audioSource;
                public float time;
            };

            AudioSource _audioSourceMusics;

            Dictionary<string, SoundElement> _effects = new Dictionary<string, SoundElement>();

            SoundResourcesManager _rc;

            public T AddGameObject<T>(string n = "") where T : Component {
                if (n == null || n == "") {
                    n = typeof(T).Name;
                }
                GameObject g = new GameObject(n);
                g.transform.SetParent(this.gameObject.transform);
                return g.AddComponent<T>();
            }

            protected override void Awake() {
                base.Awake();
                this._rc = this.AddGameObject<SoundResourcesManager>();
            }

            private void _PlayEffect(string song, float volume, float delta, string key) {
                if (this._rc.effects.ContainsKey(song)) {
                    SoundElement se = null;
                    AudioClip clip = this._rc.effects[song];
                    if (this._effects.ContainsKey(key) == false) {
                        se = new SoundElement();
                        se.audioSource = this.AddGameObject<AudioSource>("AudioSourceEffects-" + song);
                        this._effects.Add(key, se);
                    } else {
                        se = this._effects[key];
                    }
                    se.time = Time.time + delta;
                    se.audioSource.volume = volume;
                    se.audioSource.PlayOneShot(clip, volume);
                } else {
                    Debug.LogError("[SOUNDMANAGER] EFFECT NOT FOUND " + song);
                }
            }

            public void PlayEffect(string song, float volume = 1f, float delta = 0.03f, string key = "") {
                if (this.ycManager.ycConfig.SoundEffect && this.ycManager.dataManager.GetSoundEffect() == true) {
                    key = song + key;
                    if (this._effects.ContainsKey(key)) {
                        if (Time.time >= this._effects[key].time) {
                            this._PlayEffect(song, volume, delta, key);
                        }
                    } else {
                        this._PlayEffect(song, volume, delta, key);
                    }
                }
            }

            private bool CanMusic() {
                return this.ycManager.ycConfig.SoundMusic == true && this.ycManager.dataManager.GetSoundMusic() == true;
            }

            public void PlayMusic(string song, float volume = 0.5f) {
                if (this.CanMusic()) {
                    if (this._audioSourceMusics == null) {
                        this._audioSourceMusics = this.gameObject.AddGameObject<AudioSource>("AudioSourceMusics");
                        this._audioSourceMusics.loop = true;
                    }
                    if (this._rc.musics.ContainsKey(song)) {
                        this._audioSourceMusics.clip = this._rc.musics[song];
                        this._audioSourceMusics.volume = volume;
                        this._audioSourceMusics.Play();
                    } else {
                        Debug.LogError("[SOUNDMANAGER] MUSIC NOT FOUND " + song);
                    }
                    this._audioSourceMusics.clip = this._rc.musics[song];
                    this._audioSourceMusics.volume = volume;
                    this._audioSourceMusics.Play();
                }
            }

            public void PauseMusic() {
                if (this._audioSourceMusics != null) {
                    this._audioSourceMusics.Pause();
                }
            }

            public void UnPauseMusic() {
                if (this._audioSourceMusics != null && this.CanMusic()) {
                    this._audioSourceMusics.UnPause();
                }
            }

            public void StopMusic() {
                if (this._audioSourceMusics != null) {
                    this._audioSourceMusics.clip = null;
                }
            }

            public void CheckStartStopMusic() {
                if (this._audioSourceMusics) {
                    if (this.ycManager.dataManager.GetSoundMusic() == true) {
                        this._audioSourceMusics.UnPause();
                    } else {
                        this._audioSourceMusics.Pause();
                    }
                }
            }

        }
    }
}