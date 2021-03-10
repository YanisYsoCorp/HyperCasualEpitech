using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEditor.Android;
using Facebook.Unity.Settings;
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
#if UNITY_IOS
                if (ycConfig.AdMobIosAppId == "") {
                    throw new Exception("[GameUtils] Empty AdMob IOS Id");
                }
#endif
#if UNITY_ANDROID
                if (ycConfig.AdMobAndroidAppId == "") {
                    throw new Exception("[GameUtils] Empty AdMob Android Id");
                }
#endif
                this.SetAppIdFromAppLovinSettings(ycConfig.AdMobIosAppId, ycConfig.AdMobAndroidAppId);
            }

            private void GradleReplaces(string path, string file, List<KeyValuePair<string, string>> replaces) {
                string gradleBuildPath = Path.Combine(path, file);
                string content = File.ReadAllText(gradleBuildPath);
                foreach (KeyValuePair<string, string> r in replaces) {
                    content = content.Replace(r.Key, r.Value);
                }
                File.WriteAllText(gradleBuildPath, content);
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

#if UNITY_IOS
            [PostProcessBuild]
            public static void ChangeXcodePlist(BuildTarget buildTarget, string path) {
                if (buildTarget == BuildTarget.iOS) {
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
                    rootDict.values.Remove("UIApplicationExitsOnSuspend");
                    File.WriteAllText(plistPath, plist.WriteToString());
                }
            }
#endif

            [PostProcessBuild]
            public static void InitAndroidFiles(BuildTarget buildTarget, string path) {
                if (buildTarget == BuildTarget.Android) {
                    YCConfig ycConfig = YCConfig.Create();
                    string destManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");


                    string content = File.ReadAllText(destManifestPath);
                    System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex("<meta-data android:name=\"com\\.facebook\\.sdk\\.ApplicationId\".*");
                    System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex("<provider android:name=\"com\\.facebook\\.FacebookContentProvider\" android:authorities=\"com\\.facebook\\.app\\.FacebookContentProvider.*");
                    System.Text.RegularExpressions.Regex regexApplication = new System.Text.RegularExpressions.Regex("</application>");

                    content = regex1.Replace(content, "");
                    content = regex2.Replace(content, "");

                    string newId = "<meta-data android:name=\"com.facebook.sdk.ApplicationId\" android:value=\"fb" + ycConfig.FbAppId + "\" />" + Environment.NewLine +
                        "<provider android:name=\"com.facebook.FacebookContentProvider\" android:authorities=\"com.facebook.app.FacebookContentProvider" + ycConfig.FbAppId + "\" android:exported=\"true\" />" + Environment.NewLine +
                        "</application>";

                    content = regexApplication.Replace(content, newId);

                    File.WriteAllText(destManifestPath, content);
                }
            }

            [PostProcessBuild]
            public static void SetFacebook(BuildTarget buildTarget, string path) {
                YCConfig ycConfig = YCConfig.Create();
                FacebookSettings.AppIds = new List<string> { ycConfig.FbAppId };
                FacebookSettings.AppLabels = new List<string> { Application.productName };
            }

            public string GetAppLovinSettingsAssetPath() {
                string AppLovinSettingsExportPath = "MaxSdk/Resources/AppLovinSettings.asset";
                var defaultPath = Path.Combine("Assets", AppLovinSettingsExportPath);
                var guids = AssetDatabase.FindAssets("l:al_max_export_path-" + AppLovinSettingsExportPath);
                return guids.Length > 0 ? AssetDatabase.GUIDToAssetPath(guids[0]) : defaultPath;
            }

            public void SetAppIdFromAppLovinSettings(string adMobIosAppId, string adMobAndroidAppId) {
                var settingsFileName = this.GetAppLovinSettingsAssetPath();
                dynamic instance = AssetDatabase.LoadAssetAtPath(settingsFileName, Type.GetType("AppLovinSettings, MaxSdk.Scripts.IntegrationManager.Editor"));
                instance.AdMobIosAppId = adMobIosAppId;
                instance.AdMobAndroidAppId = adMobAndroidAppId;
                AssetDatabase.SaveAssets();
            }

        }

    }

}