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

            private class SoundEffectSetup {
                public float delta = 0.03f;
                public string key = "";
                public float volume = 1f;
                public float pitch = 1f;
                public bool loop = false;
                public Transform spacialParent = null;
                public float spacialMaxDistance = 10f;
            }

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

            private void _PlayEffect(string song, SoundEffectSetup setup) {
                if (this.ycManager.ycConfig.SoundEffect && this.ycManager.dataManager.GetSoundEffect() == true) {
                    if (this._rc.effects.ContainsKey(song)) {
                        string key = song + setup.key;
                        if (this._effects.ContainsKey(key) == false || Time.time >= this._effects[key].time) {
                            SoundElement se = null;
                            AudioClip clip = this._rc.effects[song];
                            if (this._effects.ContainsKey(key) == false) {
                                se = new SoundElement();
                                se.audioSource = this.AddGameObject<AudioSource>("AudioSourceEffects-" + song);
                                this._effects.Add(key, se);
                            } else {
                                se = this._effects[key];
                            }
                            se.time = Time.time + setup.delta;
                            se.audioSource.volume = setup.volume;
                            se.audioSource.pitch = setup.pitch;
                            if (setup.spacialParent != null) {
                                se.audioSource.transform.SetParent(setup.spacialParent);
                                se.audioSource.transform.localPosition = Vector3.zero;
                                se.audioSource.maxDistance = setup.spacialMaxDistance;
                                se.audioSource.spatialBlend = 1f;
                            }
                            if (setup.loop == true) {
                                se.audioSource.clip = clip;
                                se.audioSource.loop = setup.loop;
                                se.audioSource.Play();
                            } else {
                                se.audioSource.PlayOneShot(clip);
                            }
                        }
                    } else {
                        Debug.LogError("[SOUNDMANAGER] EFFECT NOT FOUND " + song);
                    }
                }
            }

            public void PlayEffect(string song, float volume = 1f, string key = "", float delta = 0.03f) {
                SoundEffectSetup setup = new SoundEffectSetup();
                setup.volume = volume;
                setup.key = key;
                setup.delta = delta;
                this._PlayEffect(song, setup);
            }

            public void PlayEffect(string song, float volume, float pitch, string key = "", float delta = 0.03f) {
                SoundEffectSetup setup = new SoundEffectSetup();
                setup.volume = volume;
                setup.pitch = pitch;
                setup.key = key;
                setup.delta = delta;
                this._PlayEffect(song, setup);
            }

            public void PlayEffect(string song, float volume, float pitch, bool loop, string key = "", float delta = 0.03f) {
                SoundEffectSetup setup = new SoundEffectSetup();
                setup.volume = volume;
                setup.pitch = pitch;
                setup.loop = loop;
                setup.key = key;
                setup.delta = delta;
                this._PlayEffect(song, setup);
            }

            public void PlayEffect(string song, float volume, float pitch, Transform spacialParent,
                                    float spacialMaxDistance, bool loop = false, string key = "", float delta = 0.03f) {
                SoundEffectSetup setup = new SoundEffectSetup();
                setup.volume = volume;
                setup.pitch = pitch;
                setup.spacialParent = spacialParent;
                setup.spacialMaxDistance = spacialMaxDistance;
                setup.loop = loop;
                setup.key = key;
                setup.delta = delta;
                this._PlayEffect(song, setup);
            }

	    public void PauseEffect(string song) {
                if (this._effects.ContainsKey(song)) {
                    SoundElement se = this._effects[song];
                    if (se.audioSource != null && se.audioSource.loop == true) {
                        se.audioSource.Pause();
                    } else {
		       Debug.LogError("[SOUNDMANAGER] EFFECT NOT FOUND OR NOT LOOPING " + song);
                    }
                }
            }

            public void UnPauseEffect(string song) {
                if (this._effects.ContainsKey(song)) {
                    SoundElement se = this._effects[song];
                    if (se.audioSource != null && se.audioSource.loop == true) {
                        se.audioSource.UnPause();
                    } else {
		       Debug.LogError("[SOUNDMANAGER] EFFECT NOT FOUND OR NOT LOOPING " + song);
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