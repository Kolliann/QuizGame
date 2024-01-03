using System.Collections.Generic;
using Model;

namespace Analytics
{
    public static class AnalyticController
    {
        public static void SendCompleteQuest(QuestionModel question, int seconds)
        {
            var time = ConvertSecondsInInterval(seconds).ToString();
            AppMetrica.Instance.ReportEvent("QuestComplete", new Dictionary<string, object>
            {
                {
                    question.Id,
                    time
                }
            });
        }

        public static void SendOpenOneLetter(string questId, string letter)
        {
            AppMetrica.Instance.ReportEvent("OpenOneLetter", new Dictionary<string, object>() {{questId, letter}});
        }

        public static void SendPlayOtherGame(string gameName, string currentQuest)
        {
            AppMetrica.Instance.ReportEvent("PlayOtherGame", new Dictionary<string, object>() {{gameName, currentQuest}});
        }
        
        public static void SendClickBunner()
        {
            AppMetrica.Instance.ReportEvent("BunnerIsClick");
        }
        
        public static void SendRatingAction(string actionName, string completeQuestsCount)
        {
            AppMetrica.Instance.ReportEvent(actionName, new Dictionary<string, object>() {{completeQuestsCount, completeQuestsCount}});
        }

        private static TimeInterval ConvertSecondsInInterval(int seconds)
        {
            if (seconds <= 60)
                return TimeInterval.ZeroToOneMinutes;
            if (seconds >= 60 && seconds <= 120)
                return TimeInterval.OneToTwoMinutes;
            if (seconds >= 120 && seconds <= 180)
                return TimeInterval.TwoToThreeMinutes;
            if (seconds >= 180 && seconds <= 240)
                return TimeInterval.ThreeToFourMinutes;
            if (seconds >= 240 && seconds <= 300)
                return TimeInterval.FourToFiveMinutes;
            if (seconds >= 300 && seconds <= 600)
                return TimeInterval.FiveToTenMinutes;
            if (seconds >= 600)
                return TimeInterval.TenToMoreMinutes;

            return TimeInterval.None;
        }
        
        public enum TimeInterval
        {
            None,
            ZeroToOneMinutes,
            OneToTwoMinutes,
            TwoToThreeMinutes,
            ThreeToFourMinutes,
            FourToFiveMinutes,
            FiveToTenMinutes,
            TenToMoreMinutes
        }
    }
}