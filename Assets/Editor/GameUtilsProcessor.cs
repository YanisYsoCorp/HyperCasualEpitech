using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEditor.Android;
using System.Collections.Generic;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace YsoCorp {

    namespace GameUtils {
        public class GameUtilsProcessor : IPreprocessBuildWithReport, IPostGenerateGradleAndroidProject {

            public int callbackOrder {
                get { return int.MaxValue; }
            }

            public void OnPreprocessBuild(BuildReport report) {
                YCConfig ycConfig = YCConfig.Create();
                if (ycConfig.gameYcId == "") {
                    throw new Exception("[GameUtils] Empty Game Yc Id");
                }
                if (ycConfig.FbAppId == "") {
                    throw new Exception("[GameUtils] Empty Fb App Id");
                }
                if (Directory.Exists("Assets/MaxSdk/Mediation/Google")) {
#if UNITY_IOS
                    if (ycConfig.AdMobIosAppId == "") {
                        throw new Exception("[GameUtils] Empty AdMob IOS Id");
                    }
#elif UNITY_ANDROID
                    if (ycConfig.AdMobAndroidAppId == "") {
                        throw new Exception("[GameUtils] Empty AdMob Android Id");
                    }
#endif
                }
                ycConfig.InitFacebook();
                ycConfig.InitMax();
            }

            private void GradleReplaces(string path, string file, List<KeyValuePair<string, string>> replaces) {
                try {
                    string gradleBuildPath = Path.Combine(path, file);
                    string content = File.ReadAllText(gradleBuildPath);
                    foreach (KeyValuePair<string, string> r in replaces) {
                        content = content.Replace(r.Key, r.Value);
                    }
                    File.WriteAllText(gradleBuildPath, content);
                } catch { }
            }

            public void OnPostGenerateGradleAndroidProject(string path) {
#if UNITY_ANDROID
                this.GradleReplaces(path, "../build.gradle", new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("com.android.tools.build:gradle:3.4.0", "com.android.tools.build:gradle:3.4.+")
                });
                this.GradleReplaces(path, "../unityLibrary/Tenjin/build.gradle", new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("implementation fileTree(dir: 'libs', include: ['*.jar'])", "implementation fileTree(dir: 'libs', include: ['*.jar', '*.aar'])")
                });
#endif
            }

            [PostProcessBuild(int.MaxValue)]
            public static void ChangeXcodePlist(BuildTarget buildTarget, string path) {
                if (buildTarget == BuildTarget.iOS) {
#if UNITY_IOS
                    YCConfig ycConfig = YCConfig.Create();
                    string plistPath = path + "/Info.plist";
                    PlistDocument plist = new PlistDocument();
                    plist.ReadFromFile(plistPath);
                    PlistElementDict rootDict = plist.root;

                    PlistElementArray rootCapacities = (PlistElementArray)rootDict.values["UIRequiredDeviceCapabilities"];
                    rootCapacities.values.RemoveAll((PlistElement elem) => {
                        return elem.AsString() == "metal";
                    });

                    rootDict.SetString("NSCalendarsUsageDescription", "Used to deliver better advertising experience");
                    rootDict.SetString("NSLocationWhenInUseUsageDescription", "Used to deliver better advertising experience");
                    rootDict.SetString("NSPhotoLibraryUsageDescription", "Used to deliver better advertising experience");
                    rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://tenjin-skan.com");
                    rootDict.values.Remove("UIApplicationExitsOnSuspend");
                    File.WriteAllText(plistPath, plist.WriteToString());
#endif
                }
            }

        }

    }

}