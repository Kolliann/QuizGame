using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Libs
{
    public static class Common
    {
        /// <summary>
        /// Shuffle list values
        /// <param>
        /// asymptotic complexity = O(N)
        /// </param>
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ShuffleList<T>(List<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var j = Random.Range(i, list.Count);

                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }

            return list;
        }


        /// <summary>
        /// Convert unix time to datetime object
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        /// <summary>
        /// Convert datetime object to unix time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (int)(dateTime - dtDateTime).TotalSeconds;
        }


        public static string GetLikeText(int count)
        {
            if (count < 1000)
                return count.ToString();
            if (count < 1000000)
                return $"{System.Math.Round(count / 1000.0f, 1)}{"ò"}";

            return $"{System.Math.Round(count / 1000000.0f, 1)}{"ì"}";
        }

        /// <summary>
        /// Gets 2 first parts of time (e.g.: D:H, H:M, M:S)
        /// </summary>
        public static string GetTime(float seconds)
        {
            int s = Mathf.CeilToInt(seconds);

            if (s < 60)
            {
                // "0s" - "59s"
                return $"{s}{"ñ"}";
            }
            if (s < 60 * 60)
            {
                // "1m 0s" - "59m 59s"
                int minutes = s / 60;
                int secondsInMinute = s % 60;
                string processedSecondsInMinuteWithUnits = secondsInMinute > 0 ?
                    " " + secondsInMinute.ToString("D2") + "ñ"
                    : "";

                return string.Format("{0}{1}{2}", minutes, "ì", processedSecondsInMinuteWithUnits);
            }
            if (s < 24 * 60 * 60)
            {
                // "1h 1m" - "12h 34m "
                int hours = s / (60 * 60);
                int minutes = (s / 60) % 60;
                string processedMinutesWithUnits = minutes > 0 ?
                    " " + minutes.ToString("D2") + "ì"
                    : "";

                return string.Format("{0}{1}{2}", hours, "÷", processedMinutesWithUnits);
            }
            else
            {
                // "1d 0h" - "12d 3h "
                int days = s / (24 * 60 * 60);
                int hours = ((s / (60 * 60)) % 24);
                string processedHoursWithUnits = hours > 0 ?
                    " " + hours.ToString("D2") + "÷"
                    : "";

                return string.Format("{0}{1}{2}", days, "ä", processedHoursWithUnits);
            }
        }
    }
}