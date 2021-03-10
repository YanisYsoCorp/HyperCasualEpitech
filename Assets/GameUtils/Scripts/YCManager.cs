using UnityEngine;

namespace YsoCorp {
    namespace GameUtils {
        [DefaultExecutionOrder(-20)]
        public class YCManager : BaseManager {

            public static YCManager instance;

            #region Static Field
            public static string VERSION = "0.1.0";

            private static string PLAYER_LAUNCH_COUNT = "YC_PLAYER_LAUNCH_COUNT";
            private static string PLAYER_GAME_COUNT = "YC_PLAYER_GAME_COUNT";

            public bool HasGameStarted { get; private set; } = false;
            #endregion

            public YCConfig ycConfig;

            public ABTestingManager abTestingManager { get; set; }
            public AdsManager adsManager { get; set; }
            public AnalyticsManager analyticsManager { get; set; }
            public FBManager fbManager { get; set; }
            public GdprManager gdprManager { get; set; }
            public I18nManager i18nManager { get; set; }
            public InAppManager inAppManager { get; set; }
            public MmpManager mmpManager { get; set; }
            public RateManager rateManager { get; set; }
            public RequestManager requestManager { get; set; }
            public SettingManager settingManager { get; set; }
            public SoundManager soundManager { get; set; }
            public VibrationManager vibrationManager { get; set; }
            public YCDataManager dataManager { get; set; }

            protected override void Awake() {
                if (instance != null) {
                    Destroy(this.gameObject);
                } else {
                    instance = this;
                    base.Awake();
                    DontDestroyOnLoad(this.gameObject);
                    this.dataManager = this.GetComponentInChildren<YCDataManager>();
                    this.abTestingManager = this.GetComponentInChildren<ABTestingManager>();
                    this.adsManager = this.GetComponentInChildren<AdsManager>();
                    this.analyticsManager = this.GetComponentInChildren<AnalyticsManager>();
                    this.fbManager = this.GetComponentInChildren<FBManager>();
                    this.gdprManager = this.GetComponentInChildren<GdprManager>();
                    this.i18nManager = this.GetComponentInChildren<I18nManager>();
                    this.inAppManager = this.GetComponentInChildren<InAppManager>();
                    this.mmpManager = this.GetComponentInChildren<MmpManager>();
                    this.rateManager = this.GetComponentInChildren<RateManager>();
                    this.requestManager = this.GetComponentInChildren<RequestManager>();
                    this.settingManager = this.GetComponentInChildren<SettingManager>(true);
                    this.soundManager = this.GetComponentInChildren<SoundManager>();
                    this.vibrationManager = this.GetComponentInChildren<VibrationManager>();
                    PlayerPrefs.SetInt(PLAYER_LAUNCH_COUNT, PlayerPrefs.GetInt(PLAYER_LAUNCH_COUNT, 0) + 1);
                    Debug.Log("YCManager : Initialize !");
                }
            }

            public bool IsFirstTimeAppLaunched() {
                return PlayerPrefs.GetInt(PLAYER_LAUNCH_COUNT, 0) == 1;
            }

            public int GetPlayerLaunchCount() {
                return PlayerPrefs.GetInt(PLAYER_LAUNCH_COUNT, 0);
            }

            public void OnGameStarted(int level) {
                if (this.HasGameStarted == false) {
                    this.HasGameStarted = true;
                    this.analyticsManager.OnGameStarted(level);
                    PlayerPrefs.SetInt(PLAYER_GAME_COUNT, PlayerPrefs.GetInt(PLAYER_GAME_COUNT, 0) + 1);
                }
            }

            public void OnGameFinished(bool levelComplete, float score = 0f) {
                if (this.HasGameStarted == true) {
                    this.HasGameStarted = false;
                    this.analyticsManager.OnGameFinished(levelComplete, score);
                }
            }

        }
    }
}
