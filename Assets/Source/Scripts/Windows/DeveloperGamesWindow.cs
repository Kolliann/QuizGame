using Analytics;
using Controller;
using JetBrains.Annotations;
using UnityEngine;

namespace Source.Scripts.Windows
{
    /// <summary>
    /// Developer Games Window
    /// </summary>
    public class DeveloperGamesWindow : GameWindowBase
    {
        /// <summary>
        /// On ckick hide window action
        /// </summary>
        [UsedImplicitly]
        public void OnExitClick()
        {
            AudioController.Instance.PlayClickButton();
            AnimationController.Hide();
        }
        
        /// <summary>
        /// On ckick play game action
        /// </summary>
        [UsedImplicitly]
        public void OnPlayClick()
        {
            foreach (var questionPackage in GameController.Instance.QuestionPackages)
            {
                if (!questionPackage.IsComplite)
                {
                    AnalyticController.SendPlayOtherGame("Отличник", questionPackage.Id);
                    break;
                }
            }
            
            AudioController.Instance.PlayClickButton();
            Application.OpenURL ("https://redirect.appmetrica.yandex.com/serve/1179230644404828088");
        }
    }
}