using UnityEngine;

#if PUSH_NOTIFICATION
#if UNITY_IOS
using System;
using Unity.Notifications.iOS;
using System.Collections;
#elif UNITY_ANDROID
using System;
using Unity.Notifications.Android;
#endif
#endif

namespace YsoCorp {
    namespace GameUtils {

        [DefaultExecutionOrder(-10)]
        public class PushNotificationManager : BaseManager {

#if PUSH_NOTIFICATION

            protected override void Awake() {
                base.Awake();
#if UNITY_IOS
                this.StartCoroutine(this.RequestAuthorization());
#elif UNITY_ANDROID
                AndroidNotificationCenter.RegisterNotificationChannel(new AndroidNotificationChannel() {
                    Id = "default",
                    Name = "Default",
                    Description = "Generic notifications",
                    Importance = Importance.Default,
                    CanShowBadge = true,
                    EnableVibration = true,
                });
                this.SendPushNotificationsAuto();
#endif
            }

#if UNITY_IOS

            AuthorizationOption GetAuthorizationOptions() {
                AuthorizationOption options = 0;
                if (this.ycManager.ycConfig.NotifAuthorizationAlert) { options |= AuthorizationOption.Alert; }
                if (this.ycManager.ycConfig.NotifAuthorizationBadge) { options |= AuthorizationOption.Badge; }
                if (this.ycManager.ycConfig.NotifAuthorizationSound) { options |= AuthorizationOption.Sound; }
                return options;
            }

            PresentationOption GetPresentationOptions(bool oAlert, bool oBadge, bool oSound) {
                PresentationOption options = PresentationOption.None;
                if (oAlert) { options |= PresentationOption.Alert; }
                if (oBadge) { options |= PresentationOption.Badge; }
                if (oSound) { options |= PresentationOption.Sound; }
                return options;
            }

            IEnumerator RequestAuthorization() {
                using (var req = new AuthorizationRequest(this.GetAuthorizationOptions(), true)) {
                    while (!req.IsFinished) {
                        yield return null;
                    };
                    if (req.Granted) {
                        this.SendPushNotificationsAuto();
                    }
                }
            }
#endif

            private void SendPushNotificationsAuto() {
                foreach (YCConfig.Notification notif in this.ycManager.ycConfig.NotificationsAuto) {
                    this.SendNotification(notif.key, notif.title, notif.message, notif.minutes, notif.optionAlert,
                                          notif.optionSound, notif.optionBadge, notif.numberBadge, notif.repeats);
                }

            }

            public void SendNotification(string key, string title, string message, int minutes, bool oAlert = true, bool oSound = true, bool oBadge = true, int numberBadge = 0,
                                         bool repeats = false, bool ShowInForeground = false, string category = "", string threadIosOnly = "") {
#if UNITY_IOS
                iOSNotificationCenter.ScheduleNotification(new iOSNotification() {
                    Identifier = key,
                    Title = title,
                    Body = message,
                    ShowInForeground = ShowInForeground,
                    Badge = numberBadge,
                    ForegroundPresentationOption = this.GetPresentationOptions(oAlert, oBadge, oSound),
                    CategoryIdentifier = category,
                    ThreadIdentifier = threadIosOnly,
                    Trigger = new iOSNotificationTimeIntervalTrigger() {
                        TimeInterval = new TimeSpan(0, 0, minutes, 0),
                        Repeats = repeats,
                    },
                });
#elif UNITY_ANDROID
                var notification = new AndroidNotification() {
                    Title = title,
                    Text = message,
                    Group = category,
                    FireTime = DateTime.Now.AddMinutes(minutes),
                    LargeIcon = "icon",
                };
                if (oBadge) {
                    notification.Number = numberBadge;
                    notification.Color = Color.red;
                }
                if (repeats) {
                    notification.RepeatInterval = new TimeSpan(0, 0, minutes, 0);
                }
                AndroidNotificationCenter.SendNotification(notification, "default");
#endif
            }
#endif

            public void DeleteNotification(string key) {
#if PUSH_NOTIFICATION
#if UNITY_IOS
                iOSNotificationCenter.RemoveScheduledNotification(key);
#elif UNITY_ANDROID
                //TODO
#endif
#endif
            }

            public string GetStatus() {
#if PUSH_NOTIFICATION
#if UNITY_IOS
                return iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus.ToString();
#else
                return "";
#endif
#else
                return "";
#endif
            }

        }

    }

}
