using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Net;
using Facebook.Unity.Settings;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YsoCorp {
    namespace GameUtils {
#if UNITY_EDITOR
        [CustomEditor(typeof(YCConfig))]
        public class YCConfigEditor : Editor {
            public override void OnInspectorGUI() {
                this.DrawDefaultInspector();
                YCConfig myTarget = (YCConfig)this.target;
                if (GUILayout.Button("Import Config")) {
                    myTarget.EditorImportConfig();
                    EditorUtility.SetDirty(myTarget);
                }
#if PUSH_NOTIFICATION
                if (GUILayout.Button("Disactivate Notification")) {
                    myTarget.EditorToogleNotification();
                }
#else
                if (GUILayout.Button("Activate Notification")) {
                    myTarget.EditorToogleNotification();
                }
#endif

            }
        }
#endif

        [CreateAssetMenu(fileName = "YCConfigData", menuName = "YsoCorp/Configuration", order = 1)]
        public class YCConfig : ScriptableObject {

            public static string VERSION = "1.12.5";

            [Serializable]
            public class Notification {
                public string key;
                public string title;
                public string message;
                public int minutes;
                public bool repeats;
                public bool optionAlert = true;
                public bool optionSound = true;
                public bool optionBadge = true;
                [YcBoolHide("optionBadge", true)]
                public int numberBadge = 0;
            }

            [Serializable]
            public struct DataData {
                public InfosData data;
            }

            [Serializable]
            public struct InfosData {
                public string key;
                public string name;
                public string android_key;
                public string ios_key;
                public string facebook_app_id;
                public string admob_android_app_id;
                public string admob_ios_app_id;
                public ApplovinData applovin;
                public MmpsData mmps;
            }

            // APPLOVIN
            [Serializable]
            public struct ApplovinData {
                public ApplovinAdUnitsData adunits;
            }
            [Serializable]
            public struct ApplovinAdUnitsData {
                public ApplovinAdUnitsOsData ios;
                public ApplovinAdUnitsOsData android;
            }
            [Serializable]
            public struct ApplovinAdUnitsOsData {
                public string interstitial;
                public string rewarded;
                public string banner;
            }

            [Serializable]
            public struct MmpData {
                public bool active;
            }
            [Serializable]
            public struct AdjustMmpData {
                public bool active;
                public string app_token;
            }

            [Serializable]
            public struct MmpsData {
                public AdjustMmpData adjust;
                public MmpData tenjin;
            }

            static private Privacy[] GDPRPRIVACIES = {
                new Privacy("AdColony", "https://www.adcolony.com/privacy-policy"),
                new Privacy("Amazon", "https://advertising.amazon.com/resources/ad-policy/en/gdpr"),
                new Privacy("AppLovin", "https://www.applovin.com/privacy/", true),
                new Privacy("Facebook", "https://m.facebook.com/about/privacy", true),
                new Privacy("Fyber", "https://www.fyber.com/Privacy-policy/"),
                new Privacy("GameAnalytics", "https://gameanalytics.com/privacy", true),
                new Privacy("Google", "https://policies.google.com/privacy"),
                new Privacy("InMobi", "https://www.inmobi.com/privacy-policy/"),
                new Privacy("IronSource", "http://www.ironsrc.com/wp-content/uploads/2019/03/ironSource-Privacy-Policy.pdf"),
                new Privacy("Mintegral", "https://www.mintegral.com/en/privacy"),
                new Privacy("ByteDance", "https://www.pangleglobal.com/privacy"),
                new Privacy("Smaato", "https://www.smaato.com/privacy/"),
                new Privacy("TapJoy", "https://www.tapjoy.com/legal/#privacy-policy"),
                new Privacy("Tenjin", "https://www.tenjin.io/privacy/", true),
                new Privacy("TikTok", "https://www.tiktok.com/legal/privacy-policy"),
                new Privacy("UnityAds", "https://unity3d.com/fr/legal/privacy-policy"),
                new Privacy("Vungle", "https://vungle.com/privacy")
            };

            [Serializable]
            public class Privacy {
                public string label;
                public string link;
                public bool display = true;

                public Privacy(string lab, string lin, bool dis = false) {
                    this.label = lab;
                    this.link = lin;
                    this.display = dis;
                }
            }

            [Header("------------------------------- CONFIG")]
            public string gameYcId;

            [Header("Sound")]
            public bool SoundEffect = false;
            public bool SoundMusic = false;


#if UNITY_PURCHASING
            [Header("InApp")]
            public string InAppRemoveAds;
            public bool InAppRemoveAdsCanRemoveInBanner = true;
            public string[] InAppConsumables;
#else
            [Header("InApp (Enable & Import InApp in Service)")]
            [YcReadOnly] public string InAppRemoveAds;
            [YcReadOnly] public bool InAppRemoveAdsCanRemoveInBanner;
            public string[] InAppConsumables { get; set; } = { };
#endif

#if PUSH_NOTIFICATION
            [Header("Notifications")]
            public bool NotifAuthorizationAlert = true;
            public bool NotifAuthorizationBadge = true;
            public bool NotifAuthorizationSound = true;
            public Notification[] NotificationsAuto;
#else
            [Header("Notifications (Import in package manage & activate)")]
            [YcReadOnly] public bool NotifAuthorizationAlert = true;
            [YcReadOnly] public bool NotifAuthorizationBadge = true;
            [YcReadOnly] public bool NotifAuthorizationSound = true;
            public Notification[] NotificationsAuto { get; set; }
#endif

            [Header("A/B Tests")]
            public int ABVersion = 1;
            public bool ABPercentMax = true;
            [YcBoolHide("ABPercentMax", false)]
            public float ABPercent = 0.05f;
            public string ABForcedSample = "";
            public string[] ABSamples = { };

            [Header("Ads")]
            public bool BannerDisplayOnInit = true;
            public float InterstitialInterval = 20;

            [Header("------------------------------- AUTO IMPORT")]

            [YcReadOnly] public string Name;
            [Space(10)]
            [YcReadOnly] public string FbAppId;
            [YcReadOnly] public string appleId = "";

            [Header("Mmp")]
            [YcReadOnly] public bool MmpAdjust = true;
            [YcReadOnly] public string MmpAdjustAppToken;
            [YcReadOnly] public bool MmpTenjin = true;

            [Header("Google AdMob")]
            [YcReadOnly] public string AdMobAndroidAppId = "";
            [YcReadOnly] public string AdMobIosAppId = "";

            [Header("Max AppLovin")]
            [YcReadOnly] public string IosInterstitial;
            [YcReadOnly] public string IosRewarded;
            [YcReadOnly] public string IosBanner;
            [Space(5)]
            [YcReadOnly] public string AndroidInterstitial;
            [YcReadOnly] public string AndroidRewarded;
            [YcReadOnly] public string AndroidBanner;

            public string deviceKey {
                get { return SystemInfo.deviceUniqueIdentifier; }
                set { }
            }

            public Privacy[] GetGdprPrivacies() {
                foreach (Privacy p in GDPRPRIVACIES) {
                    if (p.display == false) {
                        p.display = System.IO.Directory.Exists("Assets/MaxSdk/Mediation/" + p.label);
                    }
                }
                return GDPRPRIVACIES;
            }

            void CheckValues() {
                if (this.gameYcId == "" || this.gameYcId == "8f3f33") {
                    Debug.LogError("[GAMEUTILS] key not set");
                }
            }

            public static YCConfig Create() {
                return Resources.Load<YCConfig>("YCConfigData");
            }

            public void LogWarning(string msg) {
                Debug.LogWarning("[GameUtils][CONFIG]" + msg);
            }

            public bool HasInApps() {
                if (this.InAppRemoveAds != null && this.InAppRemoveAds != "") {
                    return true;
                }
                if (this.InAppConsumables.Length > 0) {
                    return true;
                }
                return false;
            }

            public string GetAndroidId() {
                return Application.identifier;
            }

            public void DisplayDialog(string title, string msg, string ok) {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog(title, msg, ok);
#endif
            }

            public void EditorImportConfig() {
                if (this.gameYcId != "") {
                    string url = RequestManager.GetUrlEmptyStatic("games/setting/" + this.gameYcId + "/" + Application.identifier, true);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Method = "Get";
                    request.ContentType = "application/json";
                    try {
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                            using (var reader = new System.IO.StreamReader(response.GetResponseStream())) {
                                InfosData infos = Newtonsoft.Json.JsonConvert.DeserializeObject<DataData>(reader.ReadToEnd()).data;
                                if (infos.name != "") {
                                    this.Name = infos.name;
                                    this.FbAppId = infos.facebook_app_id;
                                    this.appleId = infos.ios_key;

                                    this.AdMobAndroidAppId = infos.admob_android_app_id;
                                    this.AdMobIosAppId = infos.admob_ios_app_id;

                                    this.IosInterstitial = infos.applovin.adunits.ios.interstitial;
                                    this.IosRewarded = infos.applovin.adunits.ios.rewarded;
                                    this.IosBanner = infos.applovin.adunits.ios.banner;
                                    this.AndroidInterstitial = infos.applovin.adunits.android.interstitial;
                                    this.AndroidRewarded = infos.applovin.adunits.android.rewarded;
                                    this.AndroidBanner = infos.applovin.adunits.android.banner;
                                    // MMPs
                                    this.MmpAdjust = infos.mmps.adjust.active;
                                    this.MmpAdjustAppToken = infos.mmps.adjust.active ? infos.mmps.adjust.app_token : "";
                                    this.MmpTenjin = infos.mmps.tenjin.active;
                                } else {
                                    this.DisplayDialog("Error", "Impossible to import config. Check your Game Yc Id or your connection.", "Ok");
                                }
                                this.InitFacebook();
                                this.InitMax();
                            }
                        }
                    } catch (Exception e) {
                        this.DisplayDialog("Error", "Impossible to import config. Check your Game Yc Id or your connection.", "Ok");
                    }
                } else {
                    this.DisplayDialog("Error", "Please enter Game Yc Id", "Ok");
                }
            }

            public void EditorToogleNotification() {
#if UNITY_EDITOR
#if PUSH_NOTIFICATION
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Replace(";PUSH_NOTIFICATION", ""));
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS).Replace(";PUSH_NOTIFICATION", ""));
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Replace(";PUSH_NOTIFICATION", ""));
#else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone) + ";PUSH_NOTIFICATION");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS) + ";PUSH_NOTIFICATION");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android) + ";PUSH_NOTIFICATION");
#endif
                AssetDatabase.SaveAssets();
#endif
            }

            public void InitFacebook() {
#if UNITY_EDITOR
                FacebookSettings.AppIds = new List<string> { this.FbAppId };
                FacebookSettings.AppLabels = new List<string> { Application.productName };
                EditorUtility.SetDirty(FacebookSettings.Instance);
                AssetDatabase.SaveAssets();

                string destManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
                string content = File.ReadAllText(destManifestPath);
                content = new Regex("fb[0-9]+").Replace(content, "fb" + this.FbAppId);
                content = new Regex("FacebookContentProvider[0-9]+").Replace(content, "FacebookContentProvider" + this.FbAppId);
                File.WriteAllText(destManifestPath, content);
#endif
            }

            public void InitMax() {
#if UNITY_EDITOR
                AppLovinSettings.Instance.AdMobIosAppId = this.AdMobIosAppId;
                AppLovinSettings.Instance.AdMobAndroidAppId = this.AdMobAndroidAppId;
                EditorUtility.SetDirty(AppLovinSettings.Instance);
                AssetDatabase.SaveAssets();
#endif
            }

        }

    }

}
