using System;
using UnityEngine;

namespace Source.Scripts.Libs
{
    /// <summary>
    /// Local game push notifications controller
    /// </summary>
    public class LocalNotificationController : MonoBehaviour
    {
        [SerializeField, Tooltip("Off all local notification")]
        private bool _isOffLocalNotifications;

        [SerializeField, Tooltip("Minimum push sending time"), Range(6, 12)]
        private int _minPushSendingTime = 6;

        [SerializeField, Tooltip("Maximum push sending time"), Range(12, 24)]
        private int _maxPushSendingTime = 24;

        /// <summary>
        /// Pushes settings
        /// </summary>
        [SerializeField] private PushData[] _pushes;

        /// <summary>
        /// Created push setting
        /// </summary>
        /// <param name="data"></param>
        private void SendPush(PushData data)
        {
            var isInIntervalTime = TimeInIntervalTime(data.Delay);

            if (data.RepeatDelay > 0)
            {
                LocalNotification.SendRepeatingNotification(data.Id, data.Delay, data.RepeatDelay, data.Tittle,
                    data.Message, data.TittleColor, isInIntervalTime, isInIntervalTime, isInIntervalTime);
            }
            else
            {
                LocalNotification.SendNotification(data.Id, data.Delay, data.Tittle, data.Message, data.TittleColor,
                    isInIntervalTime, isInIntervalTime, isInIntervalTime);
            }
        }

        /// <summary>
        /// Check if push send in date time interval
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private bool TimeInIntervalTime(int delay)
        {
            var now = DateTime.Now;
            var pushTime = now.AddMilliseconds(delay);
            return (pushTime.Hour < _maxPushSendingTime) && (pushTime.Hour > _minPushSendingTime);
        }

        public void GetPermitions()
        {
            // NEED UNITY > 2018.2 
/*#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
                dialog = new GameObject();
            }
#endif*/
        }

        /// <summary>
        /// Remove all created pushes
        /// </summary>
        private void AllStop()
        {
            foreach (var push in _pushes)
            {
                LocalNotification.CancelNotification(push.Id);
            }
        }

        /// <summary>
        /// Created all pushes by settings
        /// </summary>
        private void CreateAll()
        {
            foreach (var push in _pushes)
            {
                SendPush(push);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                CreateAll();
            }
            else
            {
                AllStop();
            }
        }

        private void OnApplicationQuit()
        {
            CreateAll();
        }


        [Serializable]
        public struct PushData
        {
            public int Id;
            public string Tittle;
            public string Message;
            public Color TittleColor;
            public int Delay;
            public int RepeatDelay;
        }
    }
}