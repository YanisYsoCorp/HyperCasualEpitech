PACKAGE=template

VERSION=1.19.0
DATE=2021-12-06

================================================================================ VERSION 1.19.0
Update Mediation
Upgrade Tenjin 1.12.7

================================================================================ VERSION 1.18.1
Update Mediation
Set MaxSdk.SetUserId

================================================================================ VERSION 1.18.0
Update Mediation
Analytics add tenjin informations
Analytics add display ads informations

================================================================================ VERSION 1.17.0
Update Mediation
Improve Fps

================================================================================ VERSION 1.16.0
remove Firebase SDK
Update Max
Update GDPR
Add IsInterstitialOrRewardedVisible in AdManager

================================================================================ VERSION 1.15.1
Add removed files firebase

================================================================================ VERSION 1.15.0
Can import Firebase buy clicking inport config
Default AbMaxPercentage
Display AB version in setting
Remove Google in Mediation

================================================================================ VERSION 1.14.3
Add Firebase

================================================================================ VERSION 1.14.2
Fix "R" shortcut
Added game version in the setting Manager
Fix traduction Win/Lose (i18 Element was missing)
Update Max
Load maps when needed
Debug Max

================================================================================ VERSION 1.14.1
Bug Build

================================================================================ VERSION 1.14.0
Update MAX
Correct InApp with IN_APP_PURCHASING

================================================================================ VERSION 1.13.0
NSAdvertisingAttributionReportEndpoint => https://tenjin-skan.com
Update MAX
gitignore ignores Rider files

================================================================================ VERSION 1.12.5
Update MAX
Default ads 20 seconds
Bug fps unscale
Add sdk_version in analytics

================================================================================ VERSION 1.12.4
Bug Tenjin Android

================================================================================ VERSION 1.12.3
Update SoundManager
Update MAX
Update Tenjin
Removed android:debuggable=true

================================================================================ VERSION 1.12.2
Bug SoundManager

================================================================================ VERSION 1.12.1
Change AB other => control
Bug FB with ATT

================================================================================ VERSION 1.12.0
Update FB with ATT
Update Mediation
Update SoundManager

================================================================================ VERSION 1.11.0
Update I18n
YcManager Add NoInternetManager
Bug event Unity <= 2019

================================================================================ VERSION 1.10.0
Update Max
Update FB 11.0.0
Game Invokes OnStateChanged

================================================================================ VERSION 1.9.1
BUG Tenjin android
Update Mediation

================================================================================ VERSION 1.8.4
Optimisation AbTesting
Keep random map on lose
Update MAX
Update Tenjin

================================================================================ VERSION 1.8.3
Bug multiscene YcManager

================================================================================ VERSION 1.8.2
Can Build IOS without module

================================================================================ VERSION 1.8.1
Bug FB

================================================================================ VERSION 1.8.0
Update Max
Update FB 9.2.0
Remove consent flow on start

================================================================================ VERSION 1.7.2
Bug setting
Bug build android
Update Max

================================================================================ VERSION 1.7.1
Bug notification

================================================================================ VERSION 1.7.0
Update Max
Enable ATT
Analytics add app_tracking_status session
Analytics add push_notif_status session

================================================================================ VERSION 1.6.1
Bug PushNotification
Update FB
Update Max

================================================================================ VERSION 1.6.0
Add PushNotification
[EXT] Array RemoveAt

================================================================================ VERSION 1.5.0
Smatter setting FB
Smatter setting MAX
Move script
Add BannerDisplayOnInit in YcConfig
Add More Analytics infos :
public class AppData {
    ...
    public string device_model;
    public string device_os_version;
    public int device_memory_size;
    public int device_processor_count;
    public int device_processor_frequency;
    public string device_processor_type;
    public SessionData session = {
        ...
        public int fps;
    }
    public Dictionary<string, int> events = {
        ...
        banner_show = 0;
        banner_click = 0;
    }
}

================================================================================ VERSION 1.4.5
Update MAX Bug

================================================================================ VERSION 1.4.4
Update MAX

================================================================================ VERSION 1.4.3
Remove Asking Tracking link Tenjin
Review Design Buttons

================================================================================ VERSION 1.4.2
Desable Consent Flow IOS 14
Move _.gitgnore into Assets
Update MAX

================================================================================ VERSION 1.4.1
Add OpenKeyboard in YCBehaviour
Add Object and Array in ADataManager
Add DuplicateReadable in YcTexture2DExtensions
Update MAX
Add Default _.gitgnore (remove _ when import)

================================================================================ VERSION 1.4.0
Change workflow Max for IOS 14
Change workflow Tenjin for IOS 14
Check if google exist before check AdMobIds

================================================================================ VERSION 1.3.1
BUG compilation when InApp Purchase Not Activate

================================================================================ VERSION 1.3.0
Add Default Workflow Maps
Add Shortcuts a, z, w, l
InApp Purchases only activate if service activate
Review Template (Menu Win, Menu Lose)